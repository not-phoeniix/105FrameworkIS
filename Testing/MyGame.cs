using Engine;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using System.Runtime.InteropServices;
using System.Threading;

namespace Testing;

public class MyGame : Engine.Game
{
    int iterations = -125;
    int count = 0;

    int num = 2;

    public void Init()
    {
        SetResolution(500, 500);

        TestEntity testy = new(
            new Vector2(100, 100),
            Color.Red
        );
        AddToScene(testy);
    }

    public void Draw()
    {
        // DrawColor = Color.Red;
        // DrawRectFilled(100, 100, 200, 200);

        // DrawColor = Color.Black;
        // StrokeWidth = 4;
        // DrawRectOutline(100, 100, 200, 200);

        // StrokeWidth = 2;
        // int hue = 0;

        // for (int i = -125; i < iterations; i++)
        // {
        //     int offset = 2 * i;

        //     DrawColor = GetRGB(hue = (hue + 5) % 360, .95f, .99f);//GetRandomColor();//colors[(i < 0 ? -i : i) % 7];

        //     DrawShapeOutline(
        //     new Vector2(offset, offset * 2),
        //     new Vector2(250, 250 + offset),
        //     new Vector2(500 - offset, offset * 2),
        //     new Vector2(500 - offset, 500 - offset * 2),
        //     new Vector2(125 + 250, 500 - 125 - offset),
        //     new Vector2(250, 500 - offset),
        //     new Vector2(125, 500 - 125 - offset),
        //     new Vector2(offset, 500 - offset * 2));
        // }

        //     iterations += num;

        // if (iterations > 63 || iterations < -124)
        //     num = -num;
    }
}