using UnityEngine;

[CreateAssetMenu]
public class GunStats : WeaponStats
{
    public float Recoil => recoil;
    
    [SerializeField] private float recoil;
}