public class Timer
{
    private float duration;
    private float timeLeft;

    public Timer(float duration)
    {
        this.duration = duration;
        timeLeft = duration;
    }

    public bool HasFinished { get; private set; }

    public void Tick(float elapsed)
    {
        timeLeft -= elapsed;

        if (timeLeft <= 0)
        {
            HasFinished = true;
        }
    }

    public void Reset()
    {
        timeLeft = duration;
    }

    public string PrintTimeLeftInMinutes()
    {
        float minutes = timeLeft / 60f;
        float seconds = timeLeft % 60f;

        return $"{(int)minutes:00}:{seconds:00}";
    }
}
