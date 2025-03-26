using Engine;
using Microsoft.Xna.Framework;
using Engine.Toys;
using System;

namespace Testing;

public class MyGame : Engine.Game
{
    public void Init()
    {
        SetResolution(500, 500);

        AddToScene(new Character(new Vector2(250,250), new Rectangle(0,0,50,50), 5, 5, 1000, 35));

        //AddToScene(new Conveyor(Vector2.One * 200, 200, 20, ConveyorDirection.Left));
        //AddToScene(new ForceField(Vector2.One * 100, 200, 200));
    }

    public void Draw()
    {
        // spawn boxes in with left click
        if (Input.IsLeftMouseDownOnce())
        {
            AddToScene(new TestEntity(Input.MousePos, Color.Red));
            Console.WriteLine("test...?");
        }
    }
}