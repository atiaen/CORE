public abstract class Scene
{
    public abstract void Start();

    public abstract void Draw();

    public abstract void Update();

    public abstract void Stop();

    public void Reset()
    {
        Stop();
        Start();
    }
}