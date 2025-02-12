using Microsoft.Xna.Framework;

namespace Engine;

/// <summary>
/// A class that tracks and counts game FPS information
/// </summary>
internal class FpsCounter
{
    private float frameCounter = 0;
    private float now;
    private float start;

    // num seconds before reset for avg
    private readonly float sampleSeconds = 5;

    /// <summary>
    /// Total number of frames elapsed since game start
    /// </summary>
    public long TotalFrames { get; private set; }

    /// <summary>
    /// Instantaneous FPS at this moment
    /// </summary>
    public float CurrentFps { get; private set; }

    /// <summary>
    /// Average FPS over a sample period of 100 frames
    /// </summary>
    public float AvgFps { get; private set; }

    /// <summary>
    /// Time since last frame (in seconds)
    /// </summary>
    public float DeltaTime { get; private set; }

    /// <summary>
    /// Creates a new FpsCounter object, resetting internal timers to zero
    /// </summary>
    public FpsCounter()
    {
        TotalFrames = 0;
        start = 0;
    }

    /// <summary>
    /// Should be run every frame, updates FPS calculations
    /// </summary>
    public void Update(GameTime gt)
    {
        // timer/counter logic
        now = (float)gt.TotalGameTime.TotalSeconds;
        if (now > start + sampleSeconds)
        {
            start = (float)gt.TotalGameTime.TotalSeconds;
            frameCounter = 0;
        }

        // calculations
        DeltaTime = (float)gt.ElapsedGameTime.TotalSeconds;
        CurrentFps = 1.0f / DeltaTime;
        AvgFps = frameCounter / (now - start);

        frameCounter++;
        TotalFrames++;
    }
}
