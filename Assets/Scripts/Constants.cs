using UnityEngine;

public static class Constants
{
    public static readonly int ForwardDirection = Animator.StringToHash("ForwardDirection");
    public static readonly int RightDirection = Animator.StringToHash("RightDirection");
    public static readonly int IsAiming = Animator.StringToHash("IsAiming");
    public static readonly int IsWeaponUp = Animator.StringToHash("IsWeaponUp");
    public static readonly int TurnRightTrigger = Animator.StringToHash("TurnRightTrigger");
    public static readonly int TurnLeftTrigger = Animator.StringToHash("TurnLeftTrigger");

    public static readonly string ForwardDirectionKey = "Vertical";
    public static readonly string RightDirectionKey = "Horizontal";
}