using System;
using UnityEngine;

public class AnimationEventListener : MonoBehaviour
{
    public delegate void FinishedTurningEvent(object sender, EventArgs e);
    public event FinishedTurningEvent OnFinishedTurning;

    private void FinishedTurning()
    {
        OnFinishedTurning?.Invoke(this, EventArgs.Empty);
    } 
}