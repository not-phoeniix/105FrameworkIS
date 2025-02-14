namespace Engine;

/// <summary>
/// Describes an object that can be updated every frame
/// </summary>
public interface IUpdateable : ITransform
{
    /// <summary>
    /// Updates this object
    /// </summary>
    /// <param name="dt">Time passed since last frame</param>
    public void Update(float dt);
}