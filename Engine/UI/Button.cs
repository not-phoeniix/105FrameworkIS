using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.UI;

/// <summary>
/// A button that can be pressed
/// </summary>
public class Button : IUIElement
{
    private readonly string text;
    private readonly Vector2 textSize;
    private readonly SpriteFont font;
    private bool hovered;
    private bool pressed;
    private bool pressedPrev;

    /// <summary>
    /// Event called when button is clicked
    /// </summary>
    public event Action OnClick;

    /// <summary>
    /// Gets/sets the bounds of this button
    /// </summary>
    public Rectangle Bounds { get; set; }

    /// <summary>
    /// Gets/sets the top-left aligned position of this button
    /// </summary>
    public Vector2 Position
    {
        get => Bounds.Location.ToVector2();
        set
        {
            Bounds = new Rectangle(
                (int)MathF.Floor(value.X),
                (int)MathF.Floor(value.Y),
                Bounds.Width,
                Bounds.Height
            );
        }
    }

    /// <summary>
    /// Gets/sets the background color of this button
    /// </summary>
    public Color BackgroundColor { get; set; } = Color.Black;

    /// <summary>
    /// Gets/sets the background color of this button when it is hovered
    /// </summary>
    public Color HoverColor { get; set; } = Color.Gray;

    /// <summary>
    /// Gets/sets the background color of this button when it is pressed
    /// </summary>
    public Color PressedColor { get; set; } = Color.DarkGray;

    /// <summary>
    /// Gets/sets the outline color of this button
    /// </summary>
    public Color OutlineColor { get; set; } = Color.Black;

    /// <summary>
    /// Gets/sets the text color of this button
    /// </summary>
    public Color TextColor { get; set; } = Color.White;

    /// <summary>
    /// Gets/sets the size of the outline of this button
    /// </summary>
    public int OutlineSize { get; set; } = 2;

    /// <summary>
    /// Creates a new button
    /// </summary>
    /// <param name="bounds">Bounds of button on the screen</param>
    /// <param name="font">SpriteFont of text on this button</param>
    public Button(Rectangle bounds, string text, SpriteFont font)
    {
        Bounds = bounds;
        this.text = text;
        this.font = font;
        this.textSize = font.MeasureString(text);
    }

    /// <summary>
    /// Updates the logic for this button
    /// </summary>
    /// <param name="dt">Time passed since last frame</param>
    public void Update(float dt)
    {
        hovered = Bounds.Contains(Input.MousePos);
        pressedPrev = pressed;
        pressed = Input.IsLeftMouseDown();

        if (!pressed && pressedPrev)
        {
            OnClick?.Invoke();
        }
    }

    /// <summary>
    /// Draws this button to the screen
    /// </summary>
    /// <param name="sb">SpriteBatch to draw with</param>
    public void Draw(SpriteBatch sb)
    {
        Color bgColor;

        if (pressed) bgColor = PressedColor;
        else if (hovered) bgColor = HoverColor;
        else bgColor = BackgroundColor;

        sb.DrawRectFill(Bounds, bgColor);
        sb.DrawRectOutline(Bounds, OutlineSize, OutlineColor);

        Vector2 textPos = Bounds.Center.ToVector2() - (textSize / 2);
        sb.DrawString(font, text, textPos, TextColor);
    }
}
