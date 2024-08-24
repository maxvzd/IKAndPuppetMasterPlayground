using UnityEngine;
using UnityEngine.Serialization;

public class Gun : MonoBehaviour
{
    public GunStats Stats => stats;
    public GunPositions Positions => positions;

    [SerializeField] private GunStats stats;
    [SerializeField] private GunPositions positions;
}