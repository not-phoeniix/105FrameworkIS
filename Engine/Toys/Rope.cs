using Engine.Physics;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Engine.Toys;

/// <summary>
/// Contains a collection of rope nodes, a segmented physics rope
/// </summary>
public class Rope : ITransform, IDrawable, IUpdateable
{
    // https://www.owlree.blog/posts/simulating-a-rope.html

    private readonly PhysicsComponent[] nodes;
    private float segmentLength;
    private PhysicsComponent attachedComponent;

    /// <summary>
    /// Color to draw rope
    /// </summary>
    public Color DrawColor { get; set; } = Color.White;

    /// <summary>
    /// Thickness of rope
    /// </summary>
    public float DrawThickness { get; set; } = 1f;

    /// <summary>
    /// Gets/sets whether or not to anchor or lock the start of rope to
    /// a position, should be true when start position is set manually
    /// </summary>
    public bool EnableStartAnchor { get; set; } = true;

    /// <summary>
    /// Gets/sets whether or not to anchor or lock the end of rope to
    /// a position, should be true when end position is set manually
    /// </summary>
    public bool EnableEndAnchor { get; set; } = false;

    /// <summary>
    /// Gets/sets the start position of this rope
    /// </summary>
    public Vector2 Position
    {
        get => StartPos;
        set => StartPos = value;
    }

    /// <summary>
    /// Anchor position to stick the start of a rope to
    /// </summary>
    public Vector2 StartPos { get; set; }

    /// <summary>
    /// Anchor position to stick the end of a rope to
    /// </summary>
    public Vector2 EndPos { get; set; }

    /// <summary>
    /// Gets/sets the friction applied whenever an entity is
    //  attached to the end of the rope
    /// </summary>
    public float EntityAttachFriction { get; set; } = 0;

    /// <summary>
    /// Gets/sets the value of the "desired length" of this rope
    /// </summary>
    public float Length
    {
        get { return segmentLength * (nodes.Length - 2); }
        set
        {
            segmentLength = value / (nodes.Length - 2);
            if (segmentLength <= 0)
            {
                segmentLength = 0.01f;
            }
        }
    }

    /// <summary>
    /// Get/sets whether or not to enable gravity for this rope
    /// </summary>
    public bool EnableGravity
    {
        get { return nodes[0].EnableGravity; }
        set
        {
            foreach (PhysicsComponent node in nodes)
            {
                node.EnableGravity = value;
            }
        }
    }

    /// <summary>
    /// Gets/sets the gravity scale for the rope
    /// </summary>
    public float GravityScale
    {
        get { return nodes[0].GravityScale; }
        set
        {
            foreach (PhysicsComponent node in nodes)
            {
                node.GravityScale = value;
            }
        }
    }

    /// <summary>
    /// Creates a new rope
    /// </summary>
    /// <param name="startPoint">Starting position of the rope</param>
    /// <param name="endPoint">Ending position of the rope</param>
    /// <param name="segments">Number of rope segments</param>
    public Rope(Vector2 startPoint, Vector2 endPoint, int segments)
    {
        if (segments <= 0)
        {
            throw new Exception("Error creating rope: segments cannot be less than one!");
        }

        StartPos = startPoint;
        EndPos = endPoint;

        // when there's 0 subdivisions, there will be 2 nodes
        int numNodes = segments + 1;

        // initialize arrays
        nodes = new PhysicsComponent[numNodes];

        // calculate distances
        float totalLength = Vector2.Distance(startPoint, endPoint);
        segmentLength = totalLength / segments;

        // offset applied each iteration
        Vector2 diff = (endPoint - startPoint) / segments;

        // creating nodes/components
        for (int i = 0; i < numNodes; i++)
        {
            Vector2 position = startPoint + (diff * i);

            nodes[i] = new PhysicsComponent(
                position,
                new Rectangle(0, 0, 1, 1),
                1f,
                1000,
                this)
            {
                Solver = PhysicsSolver.Verlet
            };
        }
    }

    /// <summary>
    /// Attaches a physics component to the end of this rope
    /// </summary>
    /// <param name="component">Component to attach</param>
    public void AttachEnd(PhysicsComponent component)
    {
        attachedComponent = component;
    }

    /// <summary>
    /// Removes the item currently attached to the end of this rope
    /// </summary>
    public void ResetEndAttachment()
    {
        attachedComponent = null;
    }

    /// <summary>
    /// Applies a force to all segments of the rope
    /// </summary>
    /// <param name="force">Force vector to apply</param>
    public void ApplyForce(Vector2 force)
    {
        foreach (PhysicsComponent node in nodes)
        {
            node.ApplyForce(force);
        }
    }

    /// <summary>
    /// Applies a friction force to all segments of the rope
    /// </summary>
    /// <param name="coeff">Coefficient of friction to apply</param>
    public void ApplyFriction(float coeff)
    {
        foreach (PhysicsComponent node in nodes)
        {
            node.ApplyFriction(coeff);
        }
    }

    /// <summary>
    /// Updates rope movement logic/physics
    /// </summary>
    /// <param name="deltaTime">Time passed since last frame</param>
    public void Update(float deltaTime)
    {
        if (EnableStartAnchor)
        {
            nodes[0].Enabled = false;
            nodes[0].CenterPosition = StartPos;
        }
        else
        {
            nodes[0].Enabled = true;
            StartPos = nodes[0].CenterPosition;
        }

        if (EnableEndAnchor)
        {
            nodes[nodes.Length - 1].Enabled = false;
            nodes[nodes.Length - 1].CenterPosition = EndPos;
        }
        else
        {
            nodes[nodes.Length - 1].Enabled = true;
            EndPos = nodes[nodes.Length - 1].CenterPosition;
        }

        // rope position correction
        int iterations = nodes.Length;
        for (int i = 0; i < iterations; i++)
        {
            for (int j = 1; j < nodes.Length; j++)
            {
                RelaxConstraint(
                    nodes[j - 1],
                    nodes[j],
                    segmentLength
                );
            }
        }

        // draw entity and node together with a force
        if (attachedComponent != null)
        {
            PhysicsComponent node = nodes[nodes.Length - 1];
            Vector2 diff = node.CenterPosition - attachedComponent.CenterPosition;

            float scale = 2000;

            attachedComponent.ApplyForce(diff * scale);
            attachedComponent.ApplyFriction(EntityAttachFriction);
            node.ApplyForce(-diff * scale * (attachedComponent.Mass / 2));
        }

        // update physics
        foreach (PhysicsComponent node in nodes)
        {
            node.Update(deltaTime);
        }
    }

    /// <summary>
    /// Draws this rope (all internal segments)
    /// </summary>
    /// <param name="sb">SpriteBatch to draw with</param>
    public void Draw(SpriteBatch sb)
    {
        // draw lines between nodes
        for (int i = 1; i < nodes.Length; i++)
        {
            sb.DrawLineCentered(
                nodes[i].CenterPosition,
                nodes[i - 1].CenterPosition,
                DrawThickness,
                DrawColor
            );
        }
    }

    public void DrawBackground(SpriteBatch sb) { }
    public void DrawForeground(SpriteBatch sb) { }

    /// <summary>
    /// Draws rope debug info (node location and connections)
    /// </summary>
    /// <param name="sb">SpriteBatch to draw with</param>
    public void DebugDraw(SpriteBatch sb)
    {
        // draw lines between nodes
        for (int i = 1; i < nodes.Length; i++)
        {
            sb.DrawLine(
                nodes[i].CenterPosition,
                nodes[i - 1].CenterPosition,
                1f,
                Color.DarkGray
            );
        }

        // draw nodes themselves
        foreach (PhysicsComponent node in nodes)
        {
            sb.DrawCircleOutline(
                node.CenterPosition,
                2f,
                10,
                1f,
                Color.White
            );
        }
    }

    private static void RelaxConstraint(PhysicsComponent phys1, PhysicsComponent phys2, float desiredDist)
    {
        // direction vector
        Vector2 dir = phys2.CenterPosition - phys1.CenterPosition;
        if (dir != Vector2.Zero) dir.Normalize();

        // change in distance between current dist and desired dist
        float deltaDist = Vector2.Distance(phys1.CenterPosition, phys2.CenterPosition) - desiredDist;

        // apply half of each dist to each component
        if (phys1.Enabled) phys1.CenterPosition += dir * deltaDist / 2;
        if (phys2.Enabled) phys2.CenterPosition -= dir * deltaDist / 2;
    }
}
