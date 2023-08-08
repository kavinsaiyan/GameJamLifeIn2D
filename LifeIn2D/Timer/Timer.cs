using System;
using System.Collections.Generic;
public class Timer
{
    public List<ITimedAction> _timedActions;

    public Timer()
    {
        _timedActions = new List<ITimedAction>();
    }

    public void AddAction(ITimedAction timedAction)
    {
        _timedActions.Add(timedAction);
    }
    public void RemoveAction(ITimedAction timedAction)
    {
        _timedActions.Remove(timedAction);
    }
    public void Update(double deltaTime)
    {
        for (int i = _timedActions.Count - 1; i >= 0; i--)
        {
            ITimedAction timedAction = _timedActions[i];
            if (timedAction.Duration == timedAction.CurrentTime)
                timedAction.Start();
            timedAction.CurrentTime = timedAction.CurrentTime - deltaTime;
            if (timedAction.Duration <= 0)
            {
                timedAction.Finish();
                _timedActions.Remove(timedAction);
            }
        }
    }
}