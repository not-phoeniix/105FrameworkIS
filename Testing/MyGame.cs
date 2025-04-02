using Engine;
using Microsoft.Xna.Framework;
using Engine.Toys;
using System;
using System.ComponentModel;

namespace Testing;

public class MyGame : Engine.Game
{
    private Rope rope;
    private Rope rope2;
    private Ball ball;
    private Character character;

    public void Init()
    {
        SetResolution(500, 500);

        character = new Character(new Vector2(250, 250), new Rectangle(0, 0, 50, 50), 5, 5, 1000, 35);
        ball = new Ball(Vector2.One * 150, 25, 1, Color.Red);
        rope = new(
            new Vector2(250, 20),
            new Vector2(400, 20),
            10)
        {
            DrawColor = Color.Black,
            DrawThickness = 5
        };
        rope2 = new(
            new Vector2(400, 20),
            new Vector2(400, 20),
            10)
        {
            DrawColor = Color.Black,
            DrawThickness = 5
        };

        character.AttachRope(rope);
        character.AttachRope(rope2);

        AddToScene(character);
        AddToScene(ball);
        AddToScene(rope2);
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