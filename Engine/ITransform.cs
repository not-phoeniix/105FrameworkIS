using Microsoft.Xna.Framework;

namespace Engine;

/// <summary>
/// Describes an object that can exist within the world
/// </summary>
public interface ITransform
{
    /// <summary>
    /// Gets/sets the position of this object
    /// </summary>
    public Vector2 Position { get; set; }
}
