using Engine.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Windows.Markup;

namespace Engine;

public class ForceField : Entity
{
    private readonly float radius;

	public ForceField(Vector2 position, float radius, float mass) 
		: base(position, new Rectangle(0,0,10,10), mass * 100, 0)
	{
        this.radius = radius;
		base.physics.EnableGravity = false;
        base.physics.EnableCollisions = false;
	}

    public override void Update(float deltaTime)
    {
		if (Scene == null) return;

        IEnumerable<ICollidable> collidables = Scene.Collidables;

		foreach (ICollidable c in collidables)
		{
            if (!(c is Entity)) continue;

            float dis = Vector2.DistanceSquared(Position, c.Position);
            if (dis > radius * radius) continue;

            dis = MathF.Sqrt(dis);

            Vector2 force = Vector2.Normalize(Position - c.Position) * physics.Mass * MathF.Sqrt(1 / dis);

            (c as Entity).ApplyForce(force);
		}
    }

    public override void Draw(SpriteBatch sb)
    {
        DrawExtensions.DrawCircleOutline(sb, Position, radius, 20, 10, Color.MediumPurple * .1f);
        DrawExtensions.DrawCircleFill(sb, Position, 10, 10, Color.MediumPurple);
    }
}
