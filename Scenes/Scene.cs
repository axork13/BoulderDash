using Raylib_cs;

public abstract class Scene
{
    private List<Timer> timers = new List<Timer>();


    public Timer AddTimer(Action? callback, float duration, bool isLooping = true)
    {
        var timer = new Timer(callback, duration, isLooping);
        timers.Add(timer);
        return timer;
    }

    public void RemoveTimer(Timer timer)
    {
        timers.Remove(timer);
    }

    public void UpdateTimers()
    {
        foreach (var timer in timers)
            timer.Update(Raylib.GetFrameTime());
    }
    public virtual void Load(object[]? args) { }

    public virtual void Update()
    {
        UpdateTimers();
    }

    public abstract void Draw();

    public virtual void Unload() 
    { 
        timers = new List<Timer>();
    }

}