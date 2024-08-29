using UnityEngine;

public static class Constants
{
    public static readonly int ForwardDirection = Animator.StringToHash("ForwardDirection");
    public static readonly int RightDirection = Animator.StringToHash("RightDirection");
    public static readonly int IsAiming = Animator.StringToHash("IsAiming");
    public static readonly int IsWeaponUp = Animator.StringToHash("IsWeaponUp");
    public static readonly int TurnRightTrigger = Animator.StringToHash("TurnRightTrigger");
    public static readonly int TurnLeftTrigger = Animator.StringToHash("TurnLeftTrigger");
    public static readonly int ReloadTrigger = Animator.StringToHash("ReloadTrigger");

    public static readonly string ForwardDirectionKey = "Vertical";
    public static readonly string RightDirectionKey = "Horizontal";
    public static readonly string Fire1Key = "Fire1";
    public static readonly string Fire2Key = "Fire2";
    public static readonly string SwitchFireModeKey = "SwitchFireMode";
    public static readonly string RaiseLowerWeaponKey = "RaiseLowerWeapon";
    public static readonly string ReloadGunKey = "ReloadGun";
}