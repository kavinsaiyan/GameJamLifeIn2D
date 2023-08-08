using Microsoft.Xna.Framework.Graphics;

public interface ITimedAction
{
    public double Duration { get; } 
    public double CurrentTime { get; set;}
    public double PercentComplete { get => CurrentTime/Duration;}
    public event System.Action OnBegin;
    public event System.Action OnComplete;
    public void Start();
    public void Finish();
    public void Update();
}