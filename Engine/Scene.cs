using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Physics;
using Engine.Toys;

namespace Engine;

/// <summary>
/// A game scene, contains a collection of entities
/// </summary>
public class Scene
{
    private readonly List<ITransform> objects;
    private StaticCollider leftCollider;
    private StaticCollider rightCollider;
    private StaticCollider topCollider;
    private StaticCollider bottomCollider;

    /// <summary>
    /// Gets an enumerable of all collidable objects 
    //  in this scene to be iterated across
    /// </summary>
    public IEnumerable<ICollidable> Collidables
    {
        get
        {
            foreach (ITransform obj in objects)
            {
                if (obj is ICollidable collidable)
                {
                    yield return collidable;
                }
            }
        }
    }

    /// <summary>
    /// Creates a new empty scene
    /// </summary>
    public Scene(Rectangle bounds)
    {
        objects = new List<ITransform>();
        Resize(bounds);
    }

    /// <summary>
    /// Updates all objects and entities in this scene
    /// </summary>
    /// <param name="deltaTime">Time passed since last frame</param>
    public void Update(float deltaTime)
    {
        for (int i = objects.Count - 1; i >= 0; i--)
        {
            if (objects[i] is IUpdateable updateable)
            {
                updateable.Update(deltaTime);
            }
        }
    }

    /// <summary>
    /// Draws all entities in this scene
    /// </summary>
    /// <param name="sb">SpriteBatch to draw with</param>
    public void Draw(SpriteBatch sb)
    {
        for (int i = objects.Count - 1; i >= 0; i--)
        {
            if (objects[i] is IDrawable drawable)
            {
                drawable.Draw(sb);
            }
        }
    }

    /// <summary>
    /// Adds an object with a transform to this scene
    /// </summary>
    /// <param name="obj">Object to add</param>
    public void Add(ITransform obj)
    {
        if (obj != null)
        {
            if (obj is ICollidable c)
            {
                c.Scene = this;
            }

            objects.Add(obj);
        }
    }

    /// <summary>
    /// Adds many transform objects to this scene
    /// </summary>
    /// <param name="objs">Set of objects to add</param>
    public void Add(params ITransform[] objs)
    {
        foreach (ITransform obj in objs)
        {
            Add(obj);
        }
    }

    /// <summary>
    /// Removes a tranform object from this scene
    /// </summary>
    /// <param name="obj">Object to remove</param>
    /// <returns>True if object was removed, false otherwise</returns>
    public bool Remove(ITransform obj)
    {
        // can never remove null from scene, prevent null ref exceptions
        if (obj == null) return false;

        return objects.Remove(obj);
    }

    /// <summary>
    /// Removes many transform objects from this scene
    /// </summary>
    /// <param name="objs">Set of objects to remove</param>
    public void Remove(params ITransform[] objs)
    {
        foreach (ITransform obj in objs)
        {
            Remove(obj);
        }
    }

    /// <summary>
    /// Clears all objects from this scene
    /// </summary>
    public void ClearObjects()
    {
        objects.Clear();
    }

    /// <summary>
    /// Resizes this scene's bounds
    /// </summary>
    /// <param name="bounds">Rectangle bounds on screen to resize</param>
    public void Resize(Rectangle bounds)
    {
        // =========================
        // resizing removes and re-creates 
        // the edge bounds in the scene 
        // =========================

        // remove current colliders from scene
        Remove(leftCollider, rightCollider, topCollider, bottomCollider);

        // re-create colliders with new bounds

        int colliderSize = 40;
        Color colliderColor = Color.DarkGray;

        leftCollider = new StaticCollider(
            colliderColor,
            new Rectangle(
                bounds.Left - colliderSize,
                bounds.Top,
                colliderSize,
                bounds.Height));

        rightCollider = new StaticCollider(
            colliderColor,
            new Rectangle(
                bounds.Right,
                bounds.Top,
                colliderSize,
                bounds.Height));

        topCollider = new StaticCollider(
            colliderColor,
            new Rectangle(
                bounds.Left,
                bounds.Top - colliderSize,
                bounds.Width,
                colliderSize));

        bottomCollider = new StaticCollider(
            colliderColor,
            new Rectangle(
                bounds.Left,
                bounds.Bottom,
                bounds.Width,
                colliderSize));

        // re-add new colliders
        Add(leftCollider, rightCollider, topCollider, bottomCollider);
    }
}
