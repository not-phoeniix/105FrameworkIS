using Engine.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Toys
{
    public enum ConveyorDirection
    {
        Left,
        Right
    }

    /// <summary>
    /// Moves entities in collision to either left or right directions 
    /// </summary>
    public class Conveyor : Entity
    {
        private Rectangle field;
        private ConveyorDirection direction;
        private Vector2[] forces = { new Vector2(-1, 0), new Vector2(1, 0) };

        public Conveyor(Vector2 position, int length, float maxSpeed, ConveyorDirection direction) : base(position, new Rectangle(0, 20, length, 50), 1, maxSpeed)
        {
            field = new Rectangle((int)position.X, (int)position.Y, length, 70);
            Physics.EnableGravity = false;
            Physics.Enabled = false;

            this.direction = direction;
        }

        /// <summary>
        /// Updates this conveyor belt
        /// </summary>
        /// <param name="deltaTime">Time since last frame</param>
        public override void Update(float deltaTime)
        {
            IEnumerable<ICollidable> collidables = Scene.Collidables;

            foreach (ICollidable c in collidables)
            {
                if (field.Intersects(c.Bounds))
                {
                    if (c is Entity e)
                        e.ApplyForce(forces[(int)direction] * Physics.MaxSpeed * 100);
                }
            }
        }

        /// <summary>
        /// Draws conveyor interaction field to background
        /// </summary>
        /// <param name="sb"></param>
        public override void DrawBackground(SpriteBatch sb)
        {
            DrawExtensions.DrawRectFill(sb, field, Color.Green * .15f);
        }

        /// <summary>
        /// Draws conveyor to this scene
        /// </summary>
        /// <param name="sb"></param>
        public override void Draw(SpriteBatch sb)
        {
            DrawExtensions.DrawRectFill(sb, Bounds, Color.DarkGray);
            DrawExtensions.DrawRectOutline(sb, Bounds, 5, Color.Gray);
        }
    }
}
