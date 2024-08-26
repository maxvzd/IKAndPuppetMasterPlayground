using UnityEngine;

public class Gun : MonoBehaviour
{
    public GunProperties Properties => properties;
    public GunPositions Positions => positions;

    [SerializeField] private GunProperties properties;
    [SerializeField] private GunPositions positions;
}