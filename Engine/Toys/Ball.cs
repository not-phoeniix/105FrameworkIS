using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Engine.Toys
{
    public class Ball : Entity
    {
        Color color = Color.Blue;
        uint size = 0;

        private Vector2 lastVelocity;

        public Ball(Vector2 position, uint size, float mass, Color color) : base(position, new Rectangle(0,0,(int)size,(int)size),mass,1000)
        {
            this.size = size;
            this.color = color;
            Physics.GroundFrictionScale = 2;
        }

        public override void Update(float deltaTime)
        {
            
        }

        public override void Draw(SpriteBatch sb)
        {
            sb.DrawCircleFill(Position, size, 20, color);
        }
    }
}
