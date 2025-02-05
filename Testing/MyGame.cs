using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Testing;

public class MyGame : Engine.Game
{
    public void Init()
    {
        SetResolution(500, 500);
    }

    public void Draw()
    {
        DrawColor = Color.Red;
        DrawRectFilled(100, 100, 200, 200);

        DrawColor = Color.Black;
        StrokeWidth = 4;
        DrawRectOutline(100, 100, 200, 200);
    }
}