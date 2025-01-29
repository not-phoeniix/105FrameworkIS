using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Testing;

public class MyGame : Engine.Game
{
    private Texture2D texture;

    public void Init()
    {
        texture = Content.Load<Texture2D>("Images/some_guy");
        ClearColor = Color.Red;
    }

    public void Draw()
    {
        SpriteBatch.Begin(samplerState: SamplerState.PointClamp);

        SpriteBatch.Draw(texture, new Rectangle(0, 0, 400, 400), Color.White);

        SpriteBatch.End();
    }
}