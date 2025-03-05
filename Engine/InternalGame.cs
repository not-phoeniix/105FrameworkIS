using System.Reflection;
using Engine.UI;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;

namespace Engine;

internal class InternalGame : Microsoft.Xna.Framework.Game
{
    private readonly MethodInfo gameInitMethod;
    private readonly MethodInfo gameDrawMethod;
    private readonly Game game;
    private readonly Scene scene;
    private readonly FpsCounter fps;
    private TerminalWindow terminal;

    /// <summary>
    /// Gets graphics device manager of this internal game
    /// </summary>
    public GraphicsDeviceManager Graphics { get; private set; }

    /// <summary>
    /// Gets the spritebatch of this intenral game
    /// </summary>
    public SpriteBatch SpriteBatch { get; private set; }

    /// <summary>
    /// Gets/sets the clear color (or background color) of the window
    /// </summary>
    public Color ClearColor { get; set; }

    /// <summary>
    /// Creates a new internal game object
    /// </summary>
    /// <param name="game">Engine.Game to grab game methods from</param>
    public InternalGame(Game game, Scene scene)
    {
        Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Assets";
        IsMouseVisible = true;

        this.game = game;
        this.scene = scene;
        this.fps = new FpsCounter();
        gameInitMethod = game.GetType().GetMethod("Init");
        gameDrawMethod = game.GetType().GetMethod("Draw");
        ClearColor = Color.White;
    }

    protected override void Initialize()
    {
        Graphics.PreferredBackBufferWidth = 800;
        Graphics.PreferredBackBufferHeight = 600;
        base.Initialize();
    }

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);

        SpriteFont terminalFont = Content.Load<SpriteFont>("Fonts/TerminalFont");
        terminal = new TerminalWindow(GetTerminalSize(200), terminalFont);

        gameInitMethod?.Invoke(game, null);
    }

    protected override void Update(GameTime gameTime)
    {
        fps.Update(gameTime);
        Input.Update();

        if (Input.IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        terminal.Bounds = GetTerminalSize(200);
        terminal.Update(fps.DeltaTime);
        scene.Update(fps.DeltaTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(ClearColor);

        SpriteBatch.Begin(/*SpriteSortMode.FrontToBack*/);
        scene.Draw(SpriteBatch);
        gameDrawMethod?.Invoke(game, null);
        terminal.Draw(SpriteBatch);
        SpriteBatch.End();

        base.Draw(gameTime);
    }

    private Rectangle GetTerminalSize(int height)
    {
        return new Rectangle(
            0,
            Window.ClientBounds.Height - height,
            Window.ClientBounds.Width,
            height);
    }
}
