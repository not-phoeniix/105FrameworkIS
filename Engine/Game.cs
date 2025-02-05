using Microsoft.Xna.Framework;

namespace Engine;

/// <summary>
/// An Engine Game, typically the starting point of an engine program
/// </summary>
public class Game
{
    private readonly InternalGame game;

    /// <summary>
    /// Gets/sets the clear color (or background color) of the window
    /// </summary>
    protected Color ClearColor
    {
        get { return game.ClearColor; }
        set { game.ClearColor = value; }
    }

    /// <summary>
    /// Gets/sets the draw color for all drawing on the window
    /// </summary>
    public Color DrawColor { get; set; } = Color.Black;

    /// <summary>
    /// Gets/sets the stroke width of outlined objects drawn in the window
    /// </summary>
    public int StrokeWidth { get; set; } = 1;

    /// <summary>
    /// Creates a new <c>Engine.Game</c>
    /// </summary>
    public Game()
    {
        game = new InternalGame(this);
    }

    /// <summary>
    /// Runs this game, should only be called once at the start of the program
    /// </summary>
    public void Run()
    {
        game.Run();
    }

    /// <summary>
    /// Changes the resolution of the window
    /// </summary>
    /// <param name="width">New width in pixels</param>
    /// <param name="height">New height in pixels</param>
    protected void SetResolution(int width, int height)
    {
        game.Graphics.PreferredBackBufferWidth = width;
        game.Graphics.PreferredBackBufferHeight = height;
        game.Graphics.ApplyChanges();
    }

    #region // Draw Methods

    /// <summary>
    /// Draws a filled rectangle on the window
    /// </summary>
    /// <param name="x">X position of rectangle</param>
    /// <param name="y">Y position of rectangle</param>
    /// <param name="width">Width of rectangle</param>
    /// <param name="height">Height of rectangle</param>
    protected void DrawRectFilled(int x, int y, int width, int height)
    {
        game.SpriteBatch.DrawRectFill(
            new Rectangle(x, y, width, height),
            DrawColor
        );
    }

    /// <summary>
    /// Draws a rectangle outline on the window
    /// </summary>
    /// <param name="x">X position of rectangle</param>
    /// <param name="y">Y position of rectangle</param>
    /// <param name="width">Width of rectangle</param>
    /// <param name="height">Height of rectangle</param>
    protected void DrawRectOutline(int x, int y, int width, int height)
    {
        game.SpriteBatch.DrawRectOutline(
            new Rectangle(x, y, width, height),
            StrokeWidth,
            DrawColor
        );
    }

    #endregion
}
