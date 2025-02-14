using Microsoft.Xna.Framework.Graphics;

namespace Engine;

/// <summary>
/// Describes an object that can be drawn in a scene
/// </summary>
public interface IDrawable : ITransform
{
    /// <summary>
    /// Draws this object to the scene
    /// </summary>
    /// <param name="sb">SpriteBatch to draw with</param>
    public void Draw(SpriteBatch sb);
}