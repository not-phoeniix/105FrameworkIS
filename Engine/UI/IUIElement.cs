using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.UI;

/// <summary>
/// Describes a ui element that can be drawn and interacted with
/// </summary>
public interface IUIElement
{
    /// <summary>
    /// Gets the bounds of this element
    /// </summary>
    public Rectangle Bounds { get; set; }

    /// <summary>
    /// Updates this ui element
    /// </summary>
    /// <param name="dt">Time passed since last frame</param>
    public void Update(float dt);

    /// <summary>
    /// Draws this ui element
    /// </summary>
    /// <param name="sb">SpriteBatch to draw with</param>
    public void Draw(SpriteBatch sb);
}