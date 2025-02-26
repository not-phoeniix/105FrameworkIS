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

    /// <summary>
    /// Draws object to back of scene
    /// </summary>
    /// <param name="sb"></param>
    public void DrawBackground(SpriteBatch sb);

    /// <summary>
    /// Draws object to front of scene
    /// </summary>
    /// <param name="sb"></param>
    public void DrawForeground(SpriteBatch sb);
}