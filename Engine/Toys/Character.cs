using Engine.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Toys
{
    public class Character : Entity
    {
        private float jumpForce;
        private float playerSpeed;

        /// <summary>
        /// Simple character controller
        /// </summary>
        /// <param name="position">Where character is spawned in at</param>
        /// <param name="collisionBox">The collision box for the character</param>
        /// <param name="mass">How heavy the character is percieved</param>
        /// <param name="playerSpeed">How fast the character can move, not force related</param>
        /// <param name="maxSpeed">How fast can the character move at max speed</param>
        /// <param name="jumpForce">How hard does the character jump</param>
        public Character(Vector2 position, Rectangle collisionBox, float mass, float playerSpeed, float maxSpeed, float jumpForce) : base(position, collisionBox, mass, maxSpeed)
        {
            this.jumpForce = jumpForce * 10;
            this.playerSpeed = playerSpeed;
            Physics.GroundFrictionScale = 3f;
        }

        public override void Update(float deltaTime)
        {
            Vector2 movement = Vector2.Zero;

            if (Input.IsKeyDown(Keys.A))
                movement.X += -playerSpeed;
            if (Input.IsKeyDown(Keys.D))
                movement.X += playerSpeed;
            if (Input.IsKeyDownOnce(Keys.Space))
                movement.Y += -jumpForce;

            Physics.Velocity += movement;

            base.Update(deltaTime);
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.DrawRectFill(Bounds, Color.Blue);
            sb.DrawRectOutline(Bounds, 5, Color.DarkBlue);
        }

        public void AttachRope(Rope rope) => rope.AttachEnd(Physics);
    }
}
