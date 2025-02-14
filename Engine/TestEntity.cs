using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine;

/// <summary>
/// Testing entity class, used to test functionality for entity inheritance system
/// </summary>
public class TestEntity : Entity
{
    private readonly Color color;

    /// <summary>
    /// Creates a new test entity
    /// </summary>
    /// <param name="position">Position to place entity at</param>
    /// <param name="color">Color of this test entity</param>
    public TestEntity(Vector2 position, Color color)
    : base(position, new Rectangle(0, 0, 50, 50), 1, 1000)
    {
        this.color = color;
    }

    public override void Draw(SpriteBatch sb)
    {
        sb.DrawRectFill(Bounds, color);
        sb.DrawRectOutline(Bounds, 3, Color.Black);
    }
}