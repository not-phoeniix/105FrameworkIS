using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Physics;

namespace Engine.Toys;

/// <summary>
/// A simple collider class that represents an unmovable 
//  collidable axis-aligned box in the scene
/// </summary>
public class StaticCollider : ICollidable, IDrawable, ITransform
{
    /// <summary>
    /// Gets the bounding box of this collider
    /// </summary>
    public Rectangle Bounds { get; private set; }

    /// <summary>
    /// Gets/sets the scene this static collider exists in
    /// </summary>
    public Scene Scene { get; set; }

    /// <summary>
    /// Gets/sets whether or not this collider's collisions are enabled
    /// </summary>
    public bool EnableCollisions { get; set; }

    /// <summary>
    /// Gets/sets the center-anchored position in this static collider
    /// </summary>
    public Vector2 Position
    {
        get => Bounds.Center.ToVector2();
        set
        {
            Vector2 adjustedPos = value - (Bounds.Size.ToVector2() / 2);
            Bounds = new Rectangle(
                adjustedPos.ToPoint(),
                Bounds.Size
            );
        }
    }

    /// <summary>
    /// Gets/sets the color of this collider
    /// </summary>
    public Color Color { get; set; }

    /// <summary>
    /// Creates a new static collider
    /// </summary>
    /// <param name="color">Color of collider</param>
    /// <param name="bounds">Bounds of collider</param>
    public StaticCollider(Color color, Rectangle bounds)
    {
        Bounds = bounds;
        Color = color;
    }

    /// <summary>
    /// Draws this static collider to the window
    /// </summary>
    /// <param name="sb">SpriteBatch to draw with</param>
    public void Draw(SpriteBatch sb)
    {
        sb.DrawRectFill(Bounds, Color);
    }
}