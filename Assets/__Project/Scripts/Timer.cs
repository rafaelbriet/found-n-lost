public class Timer
{
    public Timer(float duration)
    {
        Duration = duration;
        TimeLeft = duration;
    }

    public bool HasFinished { get; private set; }
    public float Duration { get; private set; }
    public float TimeLeft { get; private set; }

    public void Tick(float elapsed)
    {
        TimeLeft -= elapsed;

        if (TimeLeft <= 0)
        {
            HasFinished = true;
        }
    }

    public void Reset()
    {
        TimeLeft = Duration;
        HasFinished = false;
    }

    public void SetDuration(float duration)
    {
        Duration = duration;
    }

    public string PrintTimeLeftInMinutes()
    {
        float minutes = TimeLeft / 60f;
        float seconds = TimeLeft % 60f;

        return $"{(int)minutes:00}:{seconds:00}";
    }
}
