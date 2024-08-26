using UnityEngine;
using UnityEngine.Serialization;

[CreateAssetMenu]
public class WeaponProperties : ItemProperties
{
    public float Damage => damage;
    public float Handling => handling;

    [SerializeField] private float damage;
    [SerializeField] private float handling;
}