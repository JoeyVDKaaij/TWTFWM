using System;

public class Timer
{
    private float _time = 0;

    private float _deadline;

    private Action _method;

    /// <summary>
    /// An timer that gets updated on each call. Calls method when deadline has been reached.
    /// </summary>
    /// <param name="pDeadline">The deadline that calls a method when reached.</param>
    /// <param name="pMethod">The method that gets called.</param>
    public Timer(float pDeadline, Action pMethod)
    {
        _deadline = pDeadline;
        _method = pMethod;
    }

    public void UpdateTimer()
    {
        _time += UnityEngine.Time.deltaTime;

        if (_time >= _deadline)
        {
            SetOffTimer();
        }
    }

    public void UpdateDeadline(float pDeadline)
    {
        _deadline = pDeadline;
    }

    public void ResetTimer()
    {
        _time = 0;
    }

    // Set off the timer early if needed.
    public void SetOffTimer()
    {
        _method();
        ResetTimer();
    }

    public float Time
    {
        get { return _time; }
    }
}