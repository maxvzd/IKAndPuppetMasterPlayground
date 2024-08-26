using UnityEngine;

[CreateAssetMenu]
public class GunProperties : WeaponProperties
{
    public float Recoil => recoil;
    public float RoundsPerMinute => roundsPerMinute;
    public AudioClip FireSound => fireSound;
    
    //Metres per second
    public float MuzzleVelocity => muzzleVelocity;
    //Metres per second
    public float EffectiveRange => effectiveRange;
    
    [SerializeField] private float recoil;
    [SerializeField] private float roundsPerMinute;
    [SerializeField] private AudioClip fireSound;
    [SerializeField] private float muzzleVelocity;
    [SerializeField] private float effectiveRange;
}