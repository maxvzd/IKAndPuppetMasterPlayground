using System;
using UnityEngine;

public class AnimationEventListener : MonoBehaviour
{
    public delegate void FinishedTurningEvent(object sender, EventArgs e);
    public event FinishedTurningEvent OnFinishedTurning;
    
    public delegate void FinishedReloadingEvent(object sender, EventArgs e);
    public event FinishedReloadingEvent OnFinishedReloading;

    private void FinishedTurning()
    {
        OnFinishedTurning?.Invoke(this, EventArgs.Empty);
    }

    private void ReloadFinished()
    {
        OnFinishedReloading?.Invoke(this, EventArgs.Empty);
    }
}