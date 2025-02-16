public class Timer
{
    private float elapsedTime = 0;
    public float duration { get; private set; }
    public bool isRunning { get; private set; }
    public bool isLooping;
    public bool isFinished => elapsedTime >= duration;

    public Action? Callback { private get; set; }

    public Timer(Action? callback, float duration, bool isLooping = true)
    {
        this.duration = duration;
        this.isLooping = isLooping;
        Callback = callback;
        elapsedTime = 0;
        isRunning = true;
    }

    public void Update(float dt)
    {
        if (!isRunning) return;

        elapsedTime += dt;
        if (elapsedTime >= duration)
        {
            Callback?.Invoke();
            if (isLooping)
                elapsedTime = 0;
            else
                Stop();
        }
    }

    public void Stop()
    {
        isRunning = false;
    }

    public void Start()
    {
        isRunning = true;
    }

    public void Restart()
    {
        elapsedTime = 0;
        isRunning = true;
    }

    public void setDuration(float duration)
    {
        this.duration = duration;
    }
}