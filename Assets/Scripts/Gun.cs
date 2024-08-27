using System.Linq;
using UnityEngine;

public class Gun : MonoBehaviour
{
    public GunProperties Properties => properties;
    public GunPositions Positions => positions;
    public FireMode FireMode { get; private set; } = FireMode.SemiAuto;
   
    [SerializeField] private GunProperties properties;
    [SerializeField] private GunPositions positions;

    private int _selectedFireMode = 0;

    public void CycleFireMode()
    {
        _selectedFireMode++;
        if (_selectedFireMode > properties.AvailableFireModes.Count - 1)
        {
            _selectedFireMode = 0;
        }

        FireMode = properties.AvailableFireModes.Count > 0 ?
            properties.AvailableFireModes[_selectedFireMode] : 
            FireMode.SemiAuto;
    }
}