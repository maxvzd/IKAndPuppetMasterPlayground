using System;
using System.Collections;
using System.Collections.Generic;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.Serialization;
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
    [SerializeField] private AudioClip emptyMagClick;

    [SerializeField] private Animator animator;
    [SerializeField] private AnimationEventListener animationEventListener;
    [SerializeField] private FullBodyBipedIK bipedIk;
    [SerializeField] private Transform rightHandWeaponSlot;
    [SerializeField] private Transform chestWeaponSlot;

    private IWeaponFireBehaviour _weaponFireBehaviour;
    private int _selectedFireMode = 0;
    private Magazine _magazine;
    private GunSwayAndRecoilBehaviour _gunSwayBehaviour;
    private AudioSource _audioSource;
    private IReadOnlyList<IWeaponFireBehaviour> _fireModes;
    private bool _isReloading;

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

        _magazine = gameObject.AddComponent<Magazine>();

        animationEventListener.OnFinishedReloading += OnFinishedReloading;
    }

    private void OnFinishedReloading(object sender, EventArgs e)
    {
        _isReloading = false;

        //FIX THIS
        // bipedIk.solver.leftArmMapping.weight = 1;
        // bipedIk.solver.rightArmMapping.weight = 1;
        // transform.SetParent(chestWeaponSlot, false);
        _magazine.AddRounds(30);
    }

    public void TriggerDown()
    {
        if (_isReloading) return;

        if (_weaponFireBehaviour.TriggerDown(
                this,
                _gunSwayBehaviour,
                transform,
                weaponAimTarget,
                firePoint,
                muzzleFlashVFX,
                muzzleFlashLight,
                recoilCurve,
                _audioSource,
                _magazine.NumberOfBullets,
                emptyMagClick))
        {
            _magazine.RemoveRound();
        }
    }

    public void TriggerUp()
    {
        _weaponFireBehaviour.TriggerUp();
    }


    private void Update()
    {
        if (Input.GetButtonDown(Constants.ReloadGunKey))
        {
            ReloadWeapon();
        }
    }

    public void CycleFireMode()
    {
        _selectedFireMode++;
        if (_selectedFireMode > _fireModes.Count - 1)
        {
            _selectedFireMode = 0;
        }

        _weaponFireBehaviour = _fireModes.Count > 0 ? _fireModes[_selectedFireMode] : new SemiAutoFireBehaviour(properties);
    }

    public void ReloadWeapon()
    {
        _isReloading = true;
        //bipedIk.solver.leftArmMapping.weight = 0;
        //bipedIk.solver.rightArmMapping.weight = 0;
        //transform.SetParent(rightHandWeaponSlot, false);
        animator.SetTrigger(Constants.ReloadTrigger);
    }
}