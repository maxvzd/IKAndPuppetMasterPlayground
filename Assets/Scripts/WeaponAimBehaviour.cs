using System.Collections;
using RootMotion.FinalIK;
using Unity.Mathematics;
using UnityEngine;
using Random = UnityEngine.Random;

public class WeaponAimBehaviour : MonoBehaviour
{
    private Animator _animator;
    private bool _isWeaponUp;
    private bool _isWeaponAiming;
    private IEnumerator _lowerWeaponCoRoutine;
    
    private Gun _currentlyEquippedGun;
    private Transform _weaponTransform;
    private FireWeapon _weaponFireScript;
    private GunSwayAndRecoilBehaviour _gunSwayBehaviour;

    //[SerializeField] private AimIK weaponAimIK;
    [SerializeField] private Transform weaponSlot; 
    [SerializeField] private Camera fpCamera;
    [SerializeField] private float aimFOV;
    [SerializeField] private float aimSpeed;
    //[SerializeField] private BipedIK bipedIk;
    [SerializeField] private AimIK headIk;
    [SerializeField] private AimIK aimIk;
    [SerializeField] private Transform weaponAimTarget;
    
    private float _timeElapsed;
    private float _lerpDuration = 1f;
    
    private Vector3 _currentWeaponPos;
    private Quaternion _currentWeaponRot;
    private Vector3 _targetWeaponPos;
    private Quaternion _targetWeaponRot;

    private float _originalFOV;
    private float _targetFOV;
    private float _currentFOV;

    private bool _roundsPerMinuteLock;
    private float _weaponLockWaitTime;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();

        _roundsPerMinuteLock = true;
        _weaponTransform = null;
        if (weaponSlot.childCount > 0)
        {
            _weaponTransform = weaponSlot.GetChild(0);
            
            _weaponFireScript = _weaponTransform.GetComponent<FireWeapon>();
            _currentlyEquippedGun = _weaponTransform.GetComponent<Gun>();
            _gunSwayBehaviour = weaponAimTarget.GetComponent<GunSwayAndRecoilBehaviour>();
            
            //This will eventually rely more on player skill
            _gunSwayBehaviour.SetSwayAmount(_currentlyEquippedGun.Properties.Handling * 0.01f);
            
            float roundsPerMinute = _currentlyEquippedGun.Properties.RoundsPerMinute;
            float roundsPerSecond = roundsPerMinute / 60f;
            _weaponLockWaitTime = 1 / roundsPerSecond;
        }

        _originalFOV = fpCamera.fieldOfView;
        MoveWeaponToLowerPosition();
        
        //MoveWeaponToUpPosition();
        //MoveWeaponToAimPosition();
    }

    private IEnumerator LowerWeaponAfterXSeconds(int numberOfSecondsToWait)
    {
        yield return new WaitForSeconds(numberOfSecondsToWait);
        MoveWeaponToLowerPosition();
    }

    private void ResetLowerWeaponCoRoutine()
    {
        if (!ReferenceEquals(_lowerWeaponCoRoutine, null))
        {
            StopCoroutine(_lowerWeaponCoRoutine);
        }

        _lowerWeaponCoRoutine = LowerWeaponAfterXSeconds(3);
        StartCoroutine(_lowerWeaponCoRoutine);
    }

    private void MoveWeaponToUpPosition()
    {
        if (ReferenceEquals(_currentlyEquippedGun, null)) return;
        
        _isWeaponUp = true;
        _isWeaponAiming = false;
        
        UpdateWeaponTargetPositionAndRotation(
            _currentlyEquippedGun.Positions.GunUpPosition, 
            _currentlyEquippedGun.Positions.GunUpRotation, 
            0.25f, 
            _originalFOV);

        aimIk.solver.IKPositionWeight = 1;
        headIk.solver.IKPositionWeight = 0;
        // bipedIk.solvers.aim.IKPositionWeight = 1;
        // bipedIk.solvers.lookAt.IKPositionWeight = 0;

        _animator.SetBool(Constants.IsWeaponUp, _isWeaponUp);
    }

    private void MoveWeaponToAimPosition()
    {
        _isWeaponUp = true;
        _isWeaponAiming = true;

        UpdateWeaponTargetPositionAndRotation(
            _currentlyEquippedGun.Positions.GunAimPosition,
            _currentlyEquippedGun.Positions.GunAimRotation,
            aimSpeed, 
            aimFOV);
        
        // bipedIk.solvers.aim.IKPositionWeight = 1;
        // bipedIk.solvers.lookAt.IKPositionWeight = 0;
        
        aimIk.solver.IKPositionWeight = 1;
        headIk.solver.IKPositionWeight = 0;

        _animator.SetBool(Constants.IsWeaponUp, _isWeaponUp);
    }

    private void MoveWeaponToLowerPosition()
    {
        _isWeaponUp = false;
        _isWeaponAiming = false;
        
        UpdateWeaponTargetPositionAndRotation(
            _currentlyEquippedGun.Positions.GunLoweredPosition, 
            _currentlyEquippedGun.Positions.GunLoweredRotation, 
            0.25f, 
            _originalFOV);
        
        // bipedIk.solvers.aim.IKPositionWeight = 0;
        // bipedIk.solvers.lookAt.IKPositionWeight = 1;
        
        aimIk.solver.IKPositionWeight = 0;
        headIk.solver.IKPositionWeight = 1;
        
        _animator.SetBool(Constants.IsWeaponUp, _isWeaponUp);
    }

    private void UpdateWeaponTargetPositionAndRotation(Vector3 targetPosition, Quaternion targetRotation, float timeLength, float fov)
    {
        if (ReferenceEquals(_weaponTransform, null)) return;
        
        _currentWeaponPos = _weaponTransform.localPosition;
        _currentWeaponRot = _weaponTransform.localRotation;

        _lerpDuration = timeLength;
        _timeElapsed = 0;
        _currentFOV = fpCamera.fieldOfView;
        _targetFOV = fov;

        _targetWeaponPos = targetPosition;
        _targetWeaponRot = targetRotation;
    }

    private IEnumerator WaitForNextRoundToBeReadyToFire()
    {
        yield return new WaitForSeconds(_weaponLockWaitTime);
        _roundsPerMinuteLock = true;
    }

    // Update is called once per frame
    private void Update()
    {
        if (ReferenceEquals(_weaponTransform, null)) return;
        if (ReferenceEquals(_currentlyEquippedGun, null)) return;

        FireMode fireMode = _currentlyEquippedGun.FireMode;
        
        if (Input.GetButton(Constants.Fire1) && fireMode == FireMode.Auto ||
            Input.GetButtonDown(Constants.Fire1) && fireMode == FireMode.SemiAuto)
        {
            if (_isWeaponUp)
            {
                if (_roundsPerMinuteLock)
                {
                    _roundsPerMinuteLock = false;
                    //fire
                    if (!ReferenceEquals(_lowerWeaponCoRoutine, null))
                    {
                        StopCoroutine(_lowerWeaponCoRoutine);
                    }
                
                    _weaponFireScript.Fire(
                        _gunSwayBehaviour, 
                        _targetWeaponRot.eulerAngles, 
                        _isWeaponAiming,
                        _currentlyEquippedGun.Properties,
                        weaponAimTarget);
                    
                    StartCoroutine(WaitForNextRoundToBeReadyToFire());
                    ResetLowerWeaponCoRoutine();
                }
            }
        }
        
        if (Input.GetButtonDown(Constants.Fire1) && !_isWeaponUp)
        {
            MoveWeaponToUpPosition();
            ResetLowerWeaponCoRoutine();
        }

        if (Input.GetButtonDown(Constants.RaiseLowerWeapon))
        {
            if (_isWeaponUp)
            {
                MoveWeaponToLowerPosition();
            }
            else
            {
                MoveWeaponToUpPosition();
            }
        }

        if (Input.GetButtonDown(Constants.Fire2))
        {
            MoveWeaponToAimPosition();
            if (!ReferenceEquals(_lowerWeaponCoRoutine, null))
            {
                StopCoroutine(_lowerWeaponCoRoutine);
            }

            _lowerWeaponCoRoutine = null;
        }

        if (Input.GetButtonUp(Constants.Fire2))
        {
            MoveWeaponToUpPosition();
            ResetLowerWeaponCoRoutine();
        }

        if (_timeElapsed < _lerpDuration)
        {
            float t = _timeElapsed / _lerpDuration;

            fpCamera.fieldOfView = Mathf.Lerp(_currentFOV, _targetFOV, t);
            _weaponTransform.localPosition = Vector3.Lerp(_currentWeaponPos, _targetWeaponPos, t);
            _weaponTransform.localRotation = Quaternion.Lerp(_currentWeaponRot, _targetWeaponRot, t);

            _timeElapsed += Time.deltaTime;
        }
    }
}