using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using LifeIn2D;
using System;

public class WaitForDelayAction : ITimedAction
{
    private double _duration;
    public double Duration => _duration;

    private double _currentTime;
    public double CurrentTime { get => _currentTime; set => _currentTime = value;}

    public event Action OnBegin;
    public event Action OnComplete;

    public WaitForDelayAction(float duration)
    {
        _duration = duration;
        _currentTime = duration;
    }

    public void Finish()
    {
        OnComplete?.Invoke();
    }

    public void Start()
    {
        OnBegin?.Invoke();
    }

    public void Update()
    {

    }
}