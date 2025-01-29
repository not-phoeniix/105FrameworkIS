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

    public GraphicsDeviceManager Graphics { get; private set; }
    public SpriteBatch SpriteBatch { get; private set; }
    public Color ClearColor { get; set; }

    public InternalGame(Game game)
    {
        Graphics = new GraphicsDeviceManager(this);
        Content.RootDirectory = "Assets";
        IsMouseVisible = true;

        this.game = game;
        gameInitMethod = game.GetType().GetMethod("Init");
        gameDrawMethod = game.GetType().GetMethod("Draw");
        ClearColor = Color.CornflowerBlue;
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
        if (Keyboard.GetState().IsKeyDown(Keys.Escape))
        {
            Exit();
        }

        base.Update(gameTime);
    }

    protected override void Draw(GameTime gameTime)
    {
        GraphicsDevice.Clear(ClearColor);

        gameDrawMethod?.Invoke(game, null);

        base.Draw(gameTime);
    }
}
