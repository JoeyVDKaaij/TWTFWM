using System;

public class Timer
{
    private float _time = 0;

    private float _deadline;

    private Action _method;

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
            _method();
            _time = 0;
        }
    }

    public void UpdateDeadline(float pDeadline)
    {
        _deadline = pDeadline;
    }

    public float Time
    {
        get { return _time; }
    }
}