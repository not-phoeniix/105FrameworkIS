using Engine;
using Microsoft.Xna.Framework;
using Engine.Toys;
using System;

namespace Testing;

public class MyGame : Engine.Game
{
    private Rope rope;

    public void Init()
    {
        SetResolution(500, 500);

        AddToScene(new Character(new Vector2(250, 250), new Rectangle(0, 0, 50, 50), 5, 5, 1000, 35));

        rope = new(
            new Vector2(250, 20),
            new Vector2(400, 20),
            20)
        {
            DrawColor = Color.Black,
            DrawThickness = 5
        };

        AddToScene(rope);
    }

    public void Draw()
    {
        // spawn boxes in with left click
        if (Input.IsLeftMouseDownOnce())
        {
            AddToScene(new TestEntity(Input.MousePos, Color.Red));
            Console.WriteLine("test...?");
        }

        if (Input.IsRightMouseDown())
        {
            rope.Position = Input.MousePos;
        }
    }
}