using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class WeaponStats : ItemStats
{
    public float Damage => damage;
    public float Handling => handling;

    [SerializeField] private float damage;
    [SerializeField] private float handling;
}