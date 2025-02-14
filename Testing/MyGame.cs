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
        // spawn boxes in with left click
        if (Input.IsLeftMouseDownOnce())
        {
            AddToScene(new TestEntity(Input.MousePos, Color.Red));
        }
    }
}