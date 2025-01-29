using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.Graphics;

namespace Engine;

public class Game
{
    private readonly InternalGame game;

    protected SpriteBatch SpriteBatch => game.SpriteBatch;
    protected ContentManager Content => game.Content;
    protected Color ClearColor
    {
        get { return game.ClearColor; }
        set { game.ClearColor = value; }
    }

    public Game()
    {
        game = new InternalGame(this);
    }

    public void Run()
    {
        game.Run();
    }
}
