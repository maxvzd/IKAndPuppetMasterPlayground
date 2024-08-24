using UnityEngine;

[CreateAssetMenu]
public class GunPositions : ScriptableObject
{
    public Vector3 GunUpPosition => gunUpPosition;
    public Quaternion GunUpRotation  => Quaternion.Euler(gunUpRotation);
    public Vector3 GunAimPosition  => gunAimPosition;
    public Quaternion GunAimRotation  => Quaternion.Euler(gunAimRotation);
    public Vector3 GunLoweredPosition  => gunLoweredPosition;
    public Quaternion GunLoweredRotation  => Quaternion.Euler(gunLoweredRotation);
    
    [SerializeField] private Vector3 gunUpPosition;
    [SerializeField] private Vector3 gunUpRotation;
    [SerializeField] private Vector3 gunAimPosition;
    [SerializeField] private Vector3 gunAimRotation;
    [SerializeField] private Vector3 gunLoweredPosition;
    [SerializeField] private Vector3 gunLoweredRotation;
}