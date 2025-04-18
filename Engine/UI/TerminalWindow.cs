using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.UI;

/// <summary>
/// Window that displays Console.WriteLine statements
/// </summary>
internal class TerminalWindow
{
    private readonly SpriteFont font;
    private readonly StringWriter writer;
    private readonly Button visibleButton;
    private bool visible;

    /// <summary>
    /// Gets/sets the bounds of this TerminalWindow
    /// </summary>
    public Rectangle Bounds { get; set; }

    /// <summary>
    /// Creates a new TerminalWindow object
    /// </summary>
    /// <param name="bounds">Bounds of terminal window to set</param>
    /// <param name="monoFont">Font of text to draw output in, should be monospaced</param>
    public TerminalWindow(Rectangle bounds, SpriteFont monoFont)
    {
        this.Bounds = bounds;
        this.font = monoFont;

        writer = new StringWriter();
        Console.SetOut(writer);
        Console.BackgroundColor = ConsoleColor.Black;
        Console.ForegroundColor = ConsoleColor.White;

        visibleButton = new Button(GetButtonBounds(120, 30), "Toggle terminal", monoFont);
        visibleButton.OnClick += () => visible = !visible;
    }

    /// <summary>
    /// Updates this TerminalWindow
    /// </summary>
    /// <param name="dt">Time passed since last frame</param>
    public void Update(float dt)
    {
        visibleButton.Bounds = GetButtonBounds(120, 30);
        visibleButton.Update(dt);
    }

    /// <summary>
    /// Draws this TerminalWindow to the screen
    /// </summary>
    /// <param name="sb">SpriteBatch to draw with</param>
    public void Draw(SpriteBatch sb)
    {
        if (visible)
        {
            sb.DrawRectFill(Bounds, ConsoleToMgColor(Console.BackgroundColor));
            sb.DrawString(
                font,
                writer.GetStringBuilder().ToString(),
                Bounds.Location.ToVector2(),
                ConsoleToMgColor(Console.ForegroundColor));
        }

        visibleButton.Draw(sb);
    }

    private static Color ConsoleToMgColor(ConsoleColor color)
    {
        return color switch
        {
            ConsoleColor.Black => Color.Black,
            ConsoleColor.DarkBlue => Color.DarkBlue,
            ConsoleColor.DarkGreen => Color.DarkGreen,
            ConsoleColor.DarkCyan => Color.DarkCyan,
            ConsoleColor.DarkRed => Color.DarkRed,
            ConsoleColor.DarkMagenta => Color.DarkMagenta,
            ConsoleColor.DarkYellow => Color.Goldenrod,
            ConsoleColor.Gray => Color.Gray,
            ConsoleColor.DarkGray => Color.DarkGray,
            ConsoleColor.Blue => Color.Blue,
            ConsoleColor.Green => Color.Green,
            ConsoleColor.Cyan => Color.Cyan,
            ConsoleColor.Red => Color.Red,
            ConsoleColor.Magenta => Color.Magenta,
            ConsoleColor.Yellow => Color.Yellow,
            ConsoleColor.White => Color.White,
            _ => throw new Exception("Color not recognized!")
        };
    }

    private Rectangle GetButtonBounds(int width, int height)
    {
        return new Rectangle(
            0,
            visible ? Bounds.Top - height : Bounds.Bottom - height,
            width,
            height);
    }
}
