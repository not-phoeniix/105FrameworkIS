using Microsoft.Xna.Framework.Graphics;

namespace Engine;

/// <summary>
/// A game scene, contains a collection of entities
/// </summary>
internal class Scene
{
    private readonly List<Entity> entities;

    /// <summary>
    /// Creates a new empty scene
    /// </summary>
    public Scene()
    {
        entities = new List<Entity>();
    }

    /// <summary>
    /// Updates this scene
    /// </summary>
    /// <param name="deltaTime">Time passed since last frame</param>
    public void Update(float deltaTime)
    {
        for (int i = entities.Count - 1; i >= 0; i--)
        {
            entities[i].Update(deltaTime);
        }
    }

    /// <summary>
    /// Draws all entities in this scene
    /// </summary>
    /// <param name="sb">SpriteBatch to draw with</param>
    public void Draw(SpriteBatch sb)
    {
        for (int i = entities.Count - 1; i >= 0; i--)
        {
            entities[i].Draw(sb);
        }
    }

    /// <summary>
    /// Adds an entity to this scene
    /// </summary>
    /// <param name="entity">Entity to add</param>
    public void AddEntity(Entity entity)
    {
        entities.Add(entity);
    }

    /// <summary>
    /// Removes an entity from this scene
    /// </summary>
    /// <param name="entity">Entity to remove</param>
    /// <returns>True if entity was removed, false otherwise</returns>
    public bool RemoveEntity(Entity entity)
    {
        return entities.Remove(entity);
    }

    /// <summary>
    /// Clears all entities from this scene
    /// </summary>
    public void ClearEntities()
    {
        entities.Clear();
    }
}
