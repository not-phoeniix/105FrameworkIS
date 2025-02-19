using Microsoft.Xna.Framework;

namespace Engine.Physics;

/// <summary>
/// Describes a collidable object that exists in the world
/// </summary>
public interface ICollidable : ITransform
{
    /// <summary>
    /// Gets/sets the reference to the scene this collidable can collide in
    /// </summary>
    public Scene Scene { get; set; }

    /// <summary>
    /// Gets the bounds of this collidable object
    /// </summary>
    public Rectangle Bounds { get; }

    /// <summary>
    /// Gets/sets whether or not this object can collide/be collided with
    /// </summary>
    public bool EnableCollisions { get; set; }
}