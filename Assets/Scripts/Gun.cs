using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;
using WeaponFireBehaviours;

public class Gun : MonoBehaviour
{
    public GunProperties Properties => properties;
    public GunPositions Positions => positions;
    public FireMode FireMode => _weaponFireBehaviour.FireMode;
   
    [SerializeField] private GunProperties properties;
    [SerializeField] private GunPositions positions;
    [SerializeField] private Transform weaponAimTarget;
    [SerializeField] private Transform firePoint;
    [SerializeField] private VisualEffect muzzleFlashVFX;
    [SerializeField] private GameObject muzzleFlashLight;
    [SerializeField] private AnimationCurve recoilCurve;
    
    private IWeaponFireBehaviour _weaponFireBehaviour;

    private int _selectedFireMode = 0;
    private Magazine _magazine;
    //private bool _roundsPerMinuteLock;
    //private float _weaponLockWaitTime;
    //private FireWeapon _weaponFireScript;
    private GunSwayAndRecoilBehaviour _gunSwayBehaviour;
    private AudioSource _audioSource;
    private IReadOnlyList<IWeaponFireBehaviour> _fireModes;

    private void Start()
    {
        //_weaponFireScript = GetComponent<FireWeapon>();
        
        _gunSwayBehaviour = weaponAimTarget.GetComponent<GunSwayAndRecoilBehaviour>();
        _audioSource = GetComponent<AudioSource>();

        //This will eventually rely more on player skill
        _gunSwayBehaviour.SetSwayAmount(Properties.Handling * 0.01f);

        List<IWeaponFireBehaviour> fireModes = new List<IWeaponFireBehaviour>();
        foreach (FireMode fireMode in Properties.AvailableFireModes)
        {
            switch (fireMode)
            {
                case FireMode.SemiAuto:
                    fireModes.Add(new SemiAutoFireBehaviour(Properties));
                    break;
                case FireMode.Auto:
                    fireModes.Add(new AutomaticFireBehaviour(Properties));
                    break;
                default:
                    break;
            }
        }
        _fireModes = fireModes;
        _weaponFireBehaviour = fireModes[0];
    }

    public void TriggerDown()
    {
        _weaponFireBehaviour.TriggerDown(
            this,
            _gunSwayBehaviour,
            transform,
            weaponAimTarget,
            firePoint,
            muzzleFlashVFX,
            muzzleFlashLight,
            recoilCurve,
            _audioSource);
    }

    public void TriggerUp()
    {
        _weaponFireBehaviour.TriggerUp(this);
    }
    

    private void Update()
    {
        if (Input.GetButtonDown(Constants.ReloadGunKey))
        {
            ReloadWeapon();
        }
        
        
        // if (Input.GetButton(Constants.Fire1Key) && FireMode == FireMode.Auto ||
        //     Input.GetButtonDown(Constants.Fire1Key) && FireMode == FireMode.SemiAuto)
        // {
        //     if (_isWeaponUp)
        //     {
        //         if (_roundsPerMinuteLock)
        //         {
        //             _roundsPerMinuteLock = false;
        //             //fire
        //             // if (!ReferenceEquals(_lowerWeaponCoRoutine, null))
        //             // {
        //             //     StopCoroutine(_lowerWeaponCoRoutine);
        //             // }
        //
        //             _weaponFireScript.Fire(
        //                 _gunSwayBehaviour,
        //                 transform.localEulerAngles,
        //                 _isWeaponAiming,
        //                 Properties,
        //                 weaponAimTarget);
        //             
        //             StartCoroutine(WaitForNextRoundToBeReadyToFire());
        //             //ResetLowerWeaponCoRoutine();
        //         }
        //     }
        // }
    }

    public void CycleFireMode()
    {
        _selectedFireMode++;
        if (_selectedFireMode > _fireModes.Count - 1)
        {
            _selectedFireMode = 0;
        }
        
        _weaponFireBehaviour = _fireModes.Count > 0 ?
            _fireModes[_selectedFireMode] : 
            new SemiAutoFireBehaviour(properties);
    }

    public void ReloadWeapon()
    {
        _magazine.AddRounds(30);
    }
}