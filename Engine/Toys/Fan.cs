using Engine.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

/// <summary>
/// Contains 3 points for a chevron to be drawn
/// </summary>
public struct Chevron { public Vector2 point1; public Vector2 point2; public Vector2 point3; public Color color; public float lifeSpan; public Vector2 startPosition; }

/// <summary>
/// The directions a fan can face when created
/// </summary>
public enum FanDirection { Right, Left, Up, Down }

/// <summary>
/// The direction of the force of the fan
/// </summary>
public enum FanBlower { In = -1, Out = 1}

namespace Engine.Toys
{
    public class Fan : Entity
    {
        protected FanDirection fanDirection;
        protected FanBlower blowerDirection;

        protected Rectangle blowerBounds;

        private Vector2 currentForce;
        private Matrix rotationMatrix;

        private float[] radians = { 0, MathF.PI, MathF.PI * 3 / 2, MathF.PI / 2 };
        private Vector2[] forces = { new Vector2(1, 0), new Vector2(-1, 0), new Vector2(0, -1), new Vector2(0, 1) };

        private int blowerBoundsLength = 200;
        private int fanWidth = 70;
        private int fanLength = 40;

        private float fanStrength;

        protected List<Chevron> chevrons;
        private int chevronCount = 5;
        private float chevronOffset;

        /// <summary>
        /// Constructor for Fan Toy
        /// </summary>
        /// <param name="position">Current position</param>
        /// <param name="strength">Strength of the fan's blower</param>
        /// <param name="fanDirection">Direction the fan is facing</param>
        /// <param name="blowerDirection">Direction of the blower's force</param>
        public Fan(Vector2 position, float strength, FanDirection fanDirection, FanBlower blowerDirection) : base(position, new Rectangle(0, 0, 40, 70), strength, 0)
        {
            this.fanDirection = fanDirection;
            this.blowerDirection = blowerDirection;
            this.fanStrength = strength;

            currentForce = forces[(int)fanDirection] * (int)blowerDirection;

            //PopulateChevronList(chevronCount);

            Rotate(fanDirection, blowerDirection);
        }

        public override void Update(float deltaTime)
        {
            //UpdateChevrons(fanStrength / 2, deltaTime);

            IEnumerable<ICollidable> collidables = Scene.Collidables;

            foreach (ICollidable c in collidables)
            {
                if (blowerBounds.Intersects(c.Bounds))
                {
                    if (c is Entity e)
                        e.ApplyForce(currentForce * fanStrength * 50);
                }
            }
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.DrawRectFill(Bounds, Color.Gray);
            sb.DrawRectOutline(Bounds, 5, Color.LightGray);
        }

        public override void DrawBackground(SpriteBatch sb)
        {
            sb.DrawRectFill(blowerBounds, Color.Green * .04f);
            sb.DrawRectOutline(blowerBounds, 5, Color.Green * .1f);

            //foreach (var chevron in chevrons)
            //{
            //    float value = 1;

            //    if (chevron.lifeSpan * fanStrength * 2 >= chevronOffset * chevronCount - 40)
            //    {
            //        value = (chevronOffset * chevronCount - 40) / (chevron.lifeSpan * fanStrength * 2);
            //    }
            //    if (chevron.lifeSpan * fanStrength * 2 <= chevronOffset * chevronCount - 60)
            //    {
            //        value = (chevron.lifeSpan * fanStrength * 2) / (chevronOffset * chevronCount - (chevronCount * 40) + 80);
            //    }
            //    sb.DrawShapeOutline(5, Color.Green * value, chevron.point1, chevron.point2, chevron.point3);
            //}
        }

        #region // Helper Methods

        protected void UpdateChevrons(float speed, float deltaTime)
        {
            for (int i = 0; i < chevrons.Count; i++)
            {
                Chevron chevron = chevrons[i];
                chevron.point1 += currentForce * speed * deltaTime;
                chevron.point2 += currentForce * speed * deltaTime;
                chevron.point3 += currentForce * speed * deltaTime;
                chevron.lifeSpan -= deltaTime;

                if (chevron.lifeSpan < 0)
                {
                    chevron = ResetChevron(chevron, i);
                }

                chevrons[i] = chevron;
            }
        }

        private Chevron ResetChevron(Chevron chevron, int i)
        {
            Vector2 origin = new Vector2(Bounds.Center.X, Bounds.Center.Y);

            if (fanDirection == FanDirection.Up)
                origin = new Vector2(Bounds.Location.X, Bounds.Location.Y);
            if (fanDirection == FanDirection.Down)
                origin = new Vector2(Bounds.Location.X + Bounds.Width, Bounds.Location.Y);

            chevron = CreateChevron(chevron.startPosition, new Vector2(1, 0) * (int)blowerDirection, fanWidth, 0);
            chevron.point1 = Vector2.RotateAround(chevron.point1, origin, radians[(int)fanDirection]);
            chevron.point2 = Vector2.RotateAround(chevron.point2, origin, radians[(int)fanDirection]);
            chevron.point3 = Vector2.RotateAround(chevron.point3, origin, radians[(int)fanDirection]);
            chevron.lifeSpan = (chevronOffset * chevronCount) / (fanStrength / 2);

            return chevron;
        }

        /// <summary>
        /// Rotates bounding fields based on fanDirection
        /// </summary>
        /// <param name="fanDirection"></param>
        protected void Rotate(FanDirection fanDirection, FanBlower blowerDirection)
        {
            int boxOffset = 1;

            currentForce = forces[(int)fanDirection] * (int)blowerDirection;

            switch (fanDirection)
            {
                case FanDirection.Up:
                case FanDirection.Down:

                    Physics.HorizontalCollisionBox = new Rectangle(
                        0 + (Math.Max(boxOffset, 0) / 2),
                        0,
                        fanWidth - Math.Max(boxOffset, 0),
                        fanLength
                        );
                    Physics.VerticalCollisionBox = new Rectangle(
                        0,
                        0 + (Math.Max(boxOffset, 0) / 2),
                        fanWidth,
                        fanLength - Math.Max(boxOffset, 0)
                        );

                    break;

                case FanDirection.Left:
                case FanDirection.Right:

                    Physics.HorizontalCollisionBox = new Rectangle(
                        0 + (Math.Max(boxOffset, 0) / 2),
                        0,
                        fanLength - Math.Max(boxOffset, 0),
                        fanWidth
                        );
                    Physics.VerticalCollisionBox = new Rectangle(
                        0,
                        0 + (Math.Max(boxOffset, 0) / 2),
                        fanLength,
                        fanWidth - Math.Max(boxOffset, 0)
                        );

                    break;
            }

            switch (fanDirection)
            {
                case FanDirection.Left:
                    this.blowerBounds = new Rectangle((int)Position.X - blowerBoundsLength, (int)Position.Y, blowerBoundsLength, fanWidth);
                    break;
                case FanDirection.Right:
                    this.blowerBounds = new Rectangle((int)Position.X + fanLength, (int)Position.Y, blowerBoundsLength, fanWidth);
                    break;
                case FanDirection.Up:
                    this.blowerBounds = new Rectangle((int)Position.X, (int)Position.Y - blowerBoundsLength, fanWidth, blowerBoundsLength);
                    break;
                case FanDirection.Down:
                    this.blowerBounds = new Rectangle((int)Position.X, (int)Position.Y + fanLength, fanWidth, blowerBoundsLength);
                    break;
            }

            Vector2 origin = new Vector2(Bounds.Center.X, Bounds.Center.Y);

            if (fanDirection == FanDirection.Up)
                origin = new Vector2(Bounds.Location.X, Bounds.Location.Y);
            if (fanDirection == FanDirection.Down)
                origin = new Vector2(Bounds.Location.X + Bounds.Width, Bounds.Location.Y);

            //for (int i = 0; i < chevrons.Count; i++)
            //{
            //    Chevron chevron = chevrons[i];
            //    chevron.point1 = Vector2.RotateAround(chevron.point1, origin, radians[(int)fanDirection]);
            //    chevron.point2 = Vector2.RotateAround(chevron.point2, origin, radians[(int)fanDirection]);
            //    chevron.point3 = Vector2.RotateAround(chevron.point3, origin, radians[(int)fanDirection]);
            //    chevrons[i] = chevron;
            //}
        }

        /// <summary>
        /// Creates a single chevron
        /// </summary>
        /// <param name="position">Middle of chevron along the direction</param>
        /// <param name="direction">Direction the chevron will point</param>
        /// <param name="width">Width of the chevron</param>
        /// <param name="offset">Offset along the direction vector</param>
        /// <returns></returns>
        Chevron CreateChevron(Vector2 position, Vector2 direction, float width, float offset)
        {
            Vector2 dirInv = new Vector2(direction.Y, direction.X);
            Vector2 halfWidth =  dirInv * width / 2;

            Chevron chevron = new Chevron
            {
                point1 = position + (direction * offset) - halfWidth,
                point2 = position + (direction * offset) + direction * 20,
                point3 = position + (direction * offset) + halfWidth,
                lifeSpan = (chevronOffset * chevronCount - offset) / fanStrength * 2,
                startPosition = position
            };

            return chevron;
        }

        private void PopulateChevronList(int count)
        {
            chevrons = new List<Chevron>(count);

            float offsetDistance = 40;
            chevronOffset = offsetDistance;

            for (int i = 0; i < count; i++)
            {
                Vector2 center = new Vector2(Bounds.Center.X, Bounds.Center.Y);
                Vector2 StartPosition = blowerDirection == FanBlower.Out ? center : center + new Vector2(blowerBoundsLength, 0);

                chevrons.Add(CreateChevron(StartPosition, new Vector2(1,0) * (int)blowerDirection, fanWidth, offsetDistance * i));
            }
        }

        #endregion
    }
}
