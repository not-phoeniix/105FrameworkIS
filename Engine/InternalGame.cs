using System.Reflection;
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
        // TODO: Add your initialization logic here

        base.Initialize();
    }

    protected override void LoadContent()
    {
        SpriteBatch = new SpriteBatch(GraphicsDevice);

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

        scene.Update(fps.DeltaTime);

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(ClearColor);

        SpriteBatch.Begin();
        scene.Draw(SpriteBatch);
        gameDrawMethod?.Invoke(game, null);
        SpriteBatch.End();

        base.Draw(gameTime);
    }
}
