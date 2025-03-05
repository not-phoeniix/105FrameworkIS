//
// PhysicsComponent written by Nikki Murello
//

using System;
using Microsoft.Xna.Framework;

namespace Engine.Physics;

/// <summary>
/// The types of physics solvers used in position calculation
/// </summary>
public enum PhysicsSolver
{
    Euler,
    Verlet
}

/// <summary>
/// Component that represents a "physics object," does collision
/// detection/resolution and gravity, as well as force-based movement.
/// </summary>
public class PhysicsComponent
{
    #region // Fields & Properties

    private Vector2 position;
    private Vector2 velocity;
    private Vector2 acceleration;
    private Vector2 prevPos;
    private bool toChangeVelocity;

    // tracks the class this component is attached/instantiated in
    private readonly object attachedTo;

    private static readonly float gravity = 670;

    /// <summary>
    /// Whether this component is enabled overall
    /// </summary>
    public bool Enabled { get; set; } = true;

    /// <summary>
    /// Whether or not gravity is enabled
    /// </summary>
    public bool EnableGravity { get; set; } = true;

    /// <summary>
    /// Whether or not object should collide with tiles
    /// </summary>
    public bool EnableCollisions { get; set; } = true;

    /// <summary>
    /// The type of solver used for this physics component, is Euler by default
    /// </summary>
    public PhysicsSolver Solver { get; set; } = PhysicsSolver.Euler;

    /// <summary>
    /// Boolean value whether or not object is colliding with the ground
    /// </summary>
    public bool OnGround { get; private set; }

    /// <summary>
    /// True when this object is colliding with any tile
    /// </summary>
    public bool IsColliding { get; private set; }

    /// <summary>
    /// Mass of current physics object
    /// </summary>
    public float Mass { get; private set; }

    /// <summary>
    /// Maximum speed of physics component before clamping, only applied with the
    /// </summary>
    public float MaxSpeed { get; private set; }

    /// <summary>
    /// Current position aligned to top left corner of sprite
    /// </summary>
    public Vector2 Position
    {
        get { return position; }
        set { position = value; }
    }

    /// <summary>
    /// Position at center of physics object, not top left
    /// </summary>
    public Vector2 CenterPosition
    {
        get { return Position + VerticalCollisionBox.Center.ToVector2(); }
        set { Position = value - VerticalCollisionBox.Center.ToVector2(); }
    }

    /// <summary>
    /// Current acceleration of physics object
    /// </summary>
    public Vector2 Acceleration
    {
        get { return acceleration; }
    }

    /// <summary>
    /// Gets the normalized direction vector of motion, or Vector2.Zero if stationary
    /// </summary>
    public Vector2 Direction { get; private set; }

    /// <summary>
    /// Current velocity vector of this object
    /// </summary>
    public Vector2 Velocity
    {
        get { return velocity; }
        set
        {
            velocity = value;
            toChangeVelocity = true;
        }
    }

    /// <summary>
    /// Magnitude of the velocity vector
    /// </summary>
    public float Speed
    {
        get { return velocity.Length(); }
    }

    /// <summary>
    /// Force of gravity (vector force) for this current component, applied every frame
    /// </summary>
    public Vector2 GravityForce { get; private set; }

    /// <summary>
    /// Scale of gravity applied to this object
    /// </summary>
    public float GravityScale { get; set; } = 1f;

    /// <summary>
    /// Scale of friction applied to this object when on
    /// the ground (colliding with a tile vertically)
    /// </summary>
    public float GroundFrictionScale { get; set; } = 20f;

    /// <summary>
    /// All forces applied to this component at any instantaneous point in time
    /// </summary>
    public Vector2 CurrentForces
    {
        get { return Acceleration + GravityForce; }
    }

    /// <summary>
    /// Local bounding box relative to sprite for
    /// top-bottom vertical collision in world
    /// </summary>
    public Rectangle VerticalCollisionBox { get; set; }

    /// <summary>
    /// Local bounding box relative to sprite for
    /// left-right horizontal collision in world
    /// </summary>
    public Rectangle HorizontalCollisionBox { get; set; }

    /// <summary>
    /// Vertical collision bounds in world-space,
    /// adjusted with Position variable
    /// </summary>
    public Rectangle VerticalBounds
    {
        get
        {
            return new Rectangle(
                VerticalCollisionBox.X + (int)Position.X,
                VerticalCollisionBox.Y + (int)Position.Y,
                VerticalCollisionBox.Width,
                VerticalCollisionBox.Height
            );
        }
    }

    /// <summary>
    /// Horizontal collision bounds in world-space,
    /// adjusted with Position variable
    /// </summary>
    public Rectangle HorizontalBounds
    {
        get
        {
            return new Rectangle(
                HorizontalCollisionBox.X + (int)Position.X,
                HorizontalCollisionBox.Y + (int)Position.Y,
                HorizontalCollisionBox.Width,
                HorizontalCollisionBox.Height
            );
        }
    }

    /// <summary>
    /// Combined surrounding world-space rectangle of both
    /// horizontal and vertical collision bounds
    /// </summary>
    public Rectangle Bounds
    {
        get
        {
            int minX = Math.Min(VerticalBounds.X, HorizontalBounds.X);
            int minY = Math.Min(VerticalBounds.Y, HorizontalBounds.Y);
            int maxX = Math.Max(VerticalBounds.Right, HorizontalBounds.Right);
            int maxY = Math.Max(VerticalBounds.Bottom, HorizontalBounds.Bottom);
            return new Rectangle(minX, minY, maxX - minX, maxY - minY);
        }
    }

    #endregion

    /// <summary>
    /// Creates a new physics component
    /// </summary>
    /// <param name="position">Initial position</param>
    /// <param name="verticalCollisionBox">
    /// Local-to-sprite collision box that defines where/how
    /// vertical collision takes place with this object
    /// </param>
    /// <param name="horizontalCollisionBox">
    /// Local-to-sprite collision box that defines where/how
    /// horizontal collision takes place with this object
    /// </param>
    /// <param name="mass">Mass of object</param>
    /// <param name="maxSpeed">Maximum speed of object</param>
    /// <param name="attachedTo">Object this component is being instantiated in, should be "this"</param>
    public PhysicsComponent(
        Vector2 position,
        Rectangle verticalCollisionBox,
        Rectangle horizontalCollisionBox,
        float mass,
        float maxSpeed,
        object attachedTo
    )
    {
        this.position = position;
        this.prevPos = position;
        this.VerticalCollisionBox = verticalCollisionBox;
        this.HorizontalCollisionBox = horizontalCollisionBox;
        this.Mass = mass;
        this.MaxSpeed = maxSpeed;
        this.attachedTo = attachedTo;
    }

    /// <summary>
    /// Creates a new physics component
    /// </summary>
    /// <param name="position">Initial position</param>
    /// <param name="collisionBox">
    /// Local-to-sprite bounding rectangle of this object, used
    /// for collision and bounds calculations
    /// </param>
    /// <param name="boxOffset">Unsigned integer offset of vertical/horizontal collision box modification</param>
    /// <param name="mass">Mass of object</param>
    /// <param name="maxSpeed">Maximum speed of object</param>
    /// <param name="attachedTo">Object this component is being instantiated in, should be "this"</param>
    public PhysicsComponent(
        Vector2 position,
        Rectangle collisionBox,
        int boxOffset,
        float mass,
        float maxSpeed,
        object attachedTo
    ) : this(
        position,
        new Rectangle(
            collisionBox.X + (Math.Max(boxOffset, 0) / 2),
            collisionBox.Y,
            collisionBox.Width - Math.Max(boxOffset, 0),
            collisionBox.Height
        ),
        new Rectangle(
            collisionBox.X,
            collisionBox.Y + (Math.Max(boxOffset, 0) / 2),
            collisionBox.Width,
            collisionBox.Height - Math.Max(boxOffset, 0)
        ),
        mass,
        maxSpeed,
        attachedTo
    )
    { }

    /// <summary>
    /// Creates a new physics component
    /// </summary>
    /// <param name="position">Initial position</param>
    /// <param name="collisionBox">
    /// Local-to-sprite bounding rectangle of this object, used
    /// for collision and bounds calculations
    /// </param>
    /// <param name="mass">Mass of object</param>
    /// <param name="maxSpeed">Maximum speed of object</param>
    /// <param name="attachedTo">Object this component is being instantiated in, should be "this"</param>
    public PhysicsComponent(
        Vector2 position,
        Rectangle collisionBox,
        float mass,
        float maxSpeed,
        object attachedTo
    ) : this(
        position,
        collisionBox,
        1,
        mass,
        maxSpeed,
        attachedTo
    )
    { }

    #region // Methods

    /// <summary>
    /// Does gravity calculations and collision detection with solid tiles in a chunk.
    /// </summary>
    /// <param name="deltaTime">Time passed since last frame</param>
    public void Update(float deltaTime, Scene? scene)
    {
        if (!Enabled) return;

        // apply gravity if enabled
        if (EnableGravity && !OnGround)
        {
            GravityForce = new Vector2(0, gravity);
            ApplyGravity(GravityForce * GravityScale);
        }

        // do physics solving algorithm
        switch (Solver)
        {
            case PhysicsSolver.Euler:
                EulerPosUpdate(deltaTime);
                break;
            case PhysicsSolver.Verlet:
                VerletPosUpdate(deltaTime);
                break;
        }

        // correct collisions if they are enabled and the scene isn't null
        if (scene != null && EnableCollisions)
        {
            CollisionCorrection(scene);
        }
        else
        {
            // if cannot correct collisions, will never be colliding or on ground
            OnGround = false;
            IsColliding = false;
        }

        // update direction at the end of update cycle
        Direction = Vector2.Zero;
        if (Velocity.LengthSquared() >= float.Epsilon)
        {
            // direction of motion equals normalized velocity vector
            Direction = Vector2.Normalize(Velocity);
        }
    }

    /// <summary>
    /// Does gravity calculations and force application, no collision detection
    /// </summary>
    /// <param name="deltaTime">Time passed since last frame</param>
    public void Update(float deltaTime)
    {
        Update(deltaTime, null);
    }

    private void EulerPosUpdate(float deltaTime)
    {
        velocity += acceleration * deltaTime;
        velocity = Utils.ClampMagnitude(velocity, MaxSpeed);
        position += velocity * deltaTime;
        acceleration = Vector2.Zero;
        toChangeVelocity = false;
        prevPos = position;
    }

    private void VerletPosUpdate(float deltaTime)
    {
        // if velocity is supposed to change,,, do a new inverse
        //   calculation to interpolate the value of prevPos
        if (toChangeVelocity)
        {
            // velocity = (position - prevPos) / (2 * deltaTime);
            // (2 * deltaTime) * velocity = position - prevPos
            // (2 * deltaTime) * velocity - position = -prevPos
            // position - (2 * deltaTime) * velocity = prevPos
            prevPos = position - 2 * deltaTime * velocity;
        }

        // do verlet integration itself
        Vector2 tmpPos = position;
        position = 2 * position - prevPos + acceleration * deltaTime * deltaTime;
        velocity = (position - prevPos) / (2 * deltaTime);
        prevPos = tmpPos;
        acceleration = Vector2.Zero;

        // set velocity flag back to false
        toChangeVelocity = false;
    }

    /// <summary>
    /// Applies a force to the acceleration, takes mass into account
    /// </summary>
    /// <param name="force">Vector2 force to apply</param>
    public void ApplyForce(Vector2 force)
    {
        force /= Mass;
        acceleration += force;
    }

    /// <summary>
    /// Applies a gravity force to the acceleration, does NOT take mass into account
    /// </summary>
    /// <param name="gravity">Vector2 gravity force to apply</param>
    public void ApplyGravity(Vector2 gravity)
    {
        acceleration += gravity;
    }

    /// <summary>
    /// Applies a friction force (opposite to velocity)
    /// </summary>
    /// <param name="coeff">Friction coefficient to scale velocity by</param>
    public void ApplyFriction(float coeff)
    {
        if (velocity != Vector2.Zero)
        {
            Vector2 friction = Vector2.Normalize(velocity) * coeff * -1;
            ApplyForce(friction);
        }
    }

    private void CollisionCorrection(Scene scene)
    {
        // reset properties before correction occurs
        OnGround = false;
        IsColliding = false;

        // iterate with collision correction a few times for accuracy
        int numCollisionIterations = 4;
        for (int i = 0; i < numCollisionIterations; i++)
        {
            // track whether or not any object is colliding 
            // at the end of collision loop
            bool anythingColliding = false;

            // iterate across all collidables in scene, resolve collisions with them
            foreach (ICollidable obj in scene.Collidables)
            {
                // do not resolve collisions with self
                if (obj == attachedTo) continue;

                // do not resolve collisions with objects with disabled collisions
                if (!obj.EnableCollisions) continue;

                if (ResolveCollision(obj) == true)
                {
                    anythingColliding = true;
                }
            }

            IsColliding = anythingColliding;

            if (!IsColliding)
            {
                i = numCollisionIterations;
            }
        }

        // only apply ground friction if velocity isn't a really really small number
        float velLen = velocity.Length();
        if (OnGround && velLen > 0.01f)
        {
            ApplyFriction(velLen * Mass * GroundFrictionScale);
        }
    }

    // returns whether or not a collision exists
    private bool ResolveCollision(ICollidable target)
    {
        bool collisionExists = false;

        Rectangle objBounds = target.Bounds;

        // detect by expanding bottom bounds by 1 pixel
        //   when object is colliding with ground
        bool isBelow = Utils.IsBelow(objBounds, VerticalBounds);
        bool verticalCollision = objBounds.Top <= VerticalBounds.Bottom + 1;
        if (isBelow && verticalCollision)
        {
            OnGround = true;
        }

        // vertical collision resolution
        if (objBounds.Intersects(VerticalBounds))
        {
            collisionExists = true;

            int displacement;
            bool tileBelow = objBounds.Top >= VerticalBounds.Top;

            if (tileBelow)
            {
                // for when object/entity is above the object
                displacement = VerticalBounds.Bottom - objBounds.Top;
            }
            else
            {
                // for when object/entity is below the object
                displacement = VerticalBounds.Top - objBounds.Bottom;
            }

            position.Y -= displacement;

            // ===================================
            //
            // THIS MIGHT MAKE THINGS JANK!
            //
            if (target is Entity e)
                e.ApplyForce(velocity);
            // ===================================

            velocity.Y = 0;
            toChangeVelocity = true;

            position.Y = (int)position.Y;
        }

        // horizontal collision resolution
        if (objBounds.Intersects(HorizontalBounds))
        {
            collisionExists = true;

            int displacement;
            bool tileToRight = objBounds.Left >= HorizontalBounds.Left;

            if (tileToRight)
            {
                // for when object/entity is to the left of the object
                displacement = HorizontalBounds.Right - objBounds.Left;
            }
            else
            {
                // for when object/entity is to the right of the object
                displacement = HorizontalBounds.Left - objBounds.Right;
            }

            position.X -= displacement;

            if (target is Entity e)
                e.ApplyForce(velocity);

            velocity.X = 0;
            toChangeVelocity = true;

            position.X = (int)position.X;
        }

        return collisionExists;
    }

    /// <summary>
    /// Rotate the bounds of the physics component
    /// </summary>
    /// <param name="angleInRadians">A rotation that will only support 0,90,180,270 degrees (in radians)</param>
    public void RotateBounds(float angleInRadians)
    {
        // Horizontal
        {
            Vector2 position = new Vector2(HorizontalCollisionBox.X, HorizontalCollisionBox.Y);
            Vector2 bounds = new Vector2(HorizontalCollisionBox.Width, HorizontalCollisionBox.Height);

            position = Vector2.Rotate(position, angleInRadians);

            HorizontalCollisionBox = new Rectangle((int)position.X, (int)position.Y, (int)bounds.X, (int)bounds.Y);
        }
        // Vertical
        {
            Vector2 position = new Vector2(VerticalCollisionBox.X, VerticalCollisionBox.Y);
            Vector2 bounds = new Vector2(VerticalCollisionBox.Width, VerticalCollisionBox.Height);

            position = Vector2.Rotate(position, angleInRadians);

            VerticalCollisionBox = new Rectangle((int)position.X, (int)position.Y, (int)bounds.X, (int)bounds.Y);
        }
    }

    #endregion
}
