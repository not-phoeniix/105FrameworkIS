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

    /// <summary>
    /// Draws a shape outline on the window
    /// </summary>
    /// <param name="points">List of points to be drawn in order</param>
    protected void DrawShapeOutline(params Vector2[] points)
    {
        game.SpriteBatch.DrawShapeOutline(
            StrokeWidth,
            DrawColor,
            points
            );
    }

    protected Color GetRandomColor()
    {
        Random random = new Random();
        return GetRGB(random.Next(0, 360), (float)random.NextDouble(), (float)random.NextDouble());
    }

    protected Color GetRGB(int H, double S, double V)
    {
        double dC = (V * S);
        double Hd = ((double)H) / 60;
        double dX = (dC * (1 - Math.Abs((Hd % 2) - 1)));

        int C = (int)(dC * 255);
        int X = (int)(dX * 255);

        if (Hd < 1)
        {
            return new Color(C, X, 0);
        }
        else if (Hd < 2)
        {
            return new Color(X, C, 0);
        }
        else if (Hd < 3)
        {
            return new Color(0, C, X);
        }
        else if (Hd < 4)
        {
            return new Color(0, X, C);
        }
        else if (Hd < 5)
        {
            return new Color(X, 0, C);
        }
        else if (Hd < 6)
        {
            return new Color(C, 0, X);
        }
        return new Color(0, 0, 0);
    }

    #endregion
}
