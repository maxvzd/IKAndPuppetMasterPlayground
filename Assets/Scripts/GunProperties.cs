using UnityEngine;

[CreateAssetMenu]
public class GunProperties : WeaponProperties
{
    public float Recoil => recoil;
    public float RoundsPerMinute => roundsPerMinute;
    public AudioClip FireSound => fireSound;
    
    [SerializeField] private float recoil;
    [SerializeField] private float roundsPerMinute;
    [SerializeField] private AudioClip fireSound;
}