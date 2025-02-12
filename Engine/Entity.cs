using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine;

/// <summary>
/// An abstract entity, parent of every object that can exist in scenes
/// </summary>
public abstract class Entity
{
    private Rectangle collisionBox;

    /// <summary>
    /// Gets the position of this entity
    /// </summary>
    public Vector2 Position { get; private set; }

    /// <summary>
    /// Gets the bounds of this entity
    /// </summary>
    public Rectangle Bounds => new(
        collisionBox.X + (int)Position.X,
        collisionBox.Y + (int)Position.Y,
        collisionBox.Width,
        collisionBox.Height
    );

    /// <summary>
    /// Creates an Entity
    /// </summary>
    /// <param name="collisionBox">Local collision box to align around Position for collisions</param>
    public Entity(Vector2 position, Rectangle collisionBox)
    {
        this.Position = position;
        this.collisionBox = collisionBox;
    }

    /// <summary>
    /// Updates logic for this entity
    /// </summary>
    /// <param name="deltaTime">Time passed since last frame</param>
    public virtual void Update(float deltaTime) { }

    /// <summary>
    /// Draws this entity
    /// </summary>
    /// <param name="sb">SpriteBatch to draw with</param>
    public virtual void Draw(SpriteBatch sb) { }
}