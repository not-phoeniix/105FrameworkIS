using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Engine.Physics;

namespace Engine;

/// <summary>
/// An abstract entity, parent of every object that can exist in scenes
/// </summary>
public abstract class Entity : ICollidable, IDrawable, ITransform, IUpdateable
{
    private readonly PhysicsComponent physics;

    /// <summary>
    /// Gets the physics component of this entity
    /// </summary>
    protected virtual PhysicsComponent Physics => physics;

    /// <summary>
    /// Gets/sets whether or not collisions are enabled for this entity
    /// </summary>
    public bool EnableCollisions
    {
        get => physics.EnableCollisions;
        set => physics.EnableCollisions = value;
    }

    /// <summary>
    /// Gets/sets the position of this entity
    /// </summary>
    public Vector2 Position
    {
        get => physics.Position;
        set => physics.Position = value;
    }

    /// <summary>
    /// Gets the bounds of this entity
    /// </summary>
    public Rectangle Bounds => physics.Bounds;

    /// <summary>
    /// Gets/sets the reference of the scene of this entity
    /// </summary>
    public Scene? Scene { get; set; }

    /// <summary>
    /// Creates an Entity
    /// </summary>
    /// <param name="position">Starting position of this entity</param>
    /// <param name="collisionBox">Local collision box to align around Position for collisions</param>
    /// <param name="mass">Mass of this entity</param>
    /// <param name="maxSpeed">Max speed/velocity magnitude of this entity</param>
    public Entity(
        Vector2 position,
        Rectangle collisionBox,
        float mass,
        float maxSpeed)
    {
        physics = new PhysicsComponent(
            position,
            collisionBox,
            2,
            mass,
            maxSpeed,
            this
        );
    }

    public virtual void ApplyForce(Vector2 force)
    {
        physics.ApplyForce(force);
    }

    /// <summary>
    /// Updates logic for this entity
    /// </summary>
    /// <param name="deltaTime">Time passed since last frame</param>
    public virtual void Update(float deltaTime)
    {
        physics.Update(deltaTime, Scene);
    }

    /// <summary>
    /// Draws this entity
    /// </summary>
    /// <param name="sb">SpriteBatch to draw with</param>
    public virtual void Draw(SpriteBatch sb) { }

    public virtual void DrawBackground(SpriteBatch sb)
    {
    }

    public virtual void DrawForeground(SpriteBatch sb)
    {
    }
}