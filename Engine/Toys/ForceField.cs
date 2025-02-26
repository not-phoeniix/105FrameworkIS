using Engine.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Toys;

public class ForceField : Entity
{
    private readonly float radius;
    /// <summary>
    /// Object that applies a force within its radius based on how close it is
    /// </summary>
    /// <param name="position">Position at the center fo the field</param>
    /// <param name="radius">The depth extending outward of the field's effect</param>
    /// <param name="mass">How strong the field pulls in objects</param>
	public ForceField(Vector2 position, float radius, float mass) 
		: base(position, new Rectangle(0,0,10,10), mass * 100, 0)
	{
        this.radius = radius;
		base.Physics.EnableGravity = false;
        base.Physics.EnableCollisions = false;
	}

    /// <summary>
    /// Updates the logic for this force field
    /// </summary>
    /// <param name="deltaTime">Time since last frame</param>
    public override void Update(float deltaTime)
    {
		if (Scene == null) return;

        IEnumerable<ICollidable> collidables = Scene.Collidables;

		foreach (ICollidable c in collidables)
		{
            float dis = Vector2.DistanceSquared(Position, c.Position);
            if (dis > radius * radius) continue;

            dis = MathF.Sqrt(dis);

            Vector2 force = Vector2.Normalize(Position - c.Position) * Physics.Mass * MathF.Sqrt(1 / dis);

            if (c is Entity e)
                e.ApplyForce(force);
		}
    }

    /// <summary>
    /// Draw this force field's outline to the background
    /// </summary>
    /// <param name="sb"></param>
    public override void DrawBackground(SpriteBatch sb)
    {
        DrawExtensions.DrawCircleOutline(sb, Position, radius, 20, 10, Color.MediumPurple * .1f);
    }

    /// <summary>
    /// Draws this force field to the screen
    /// </summary>
    /// <param name="sb"></param>
    public override void Draw(SpriteBatch sb)
    {
        DrawExtensions.DrawCircleFill(sb, Position, 10, 10, Color.MediumPurple);
    }
}
