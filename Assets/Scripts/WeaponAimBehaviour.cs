using System.Collections;
using RootMotion.FinalIK;
using UnityEngine;

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

    [SerializeField] private Transform weaponSlot;
    [SerializeField] private Camera fpCamera;
    [SerializeField] private float aimFOV;
    [SerializeField] private float aimSpeed;
    [SerializeField] private AimIK headIk;
    [SerializeField] private AimIK aimIk;
    [SerializeField] private Transform weaponAimTarget;

    private float _originalFOV;
    private IEnumerator _weaponRotLerpCoroutine;

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
            _originalFOV,
            1f,
            0f);

        _animator.SetBool(Constants.IsWeaponUp, _isWeaponUp);
        _animator.SetBool(Constants.IsAiming, _isWeaponAiming);
    }

    private void MoveWeaponToAimPosition()
    {
        _isWeaponUp = true;
        _isWeaponAiming = true;

        UpdateWeaponTargetPositionAndRotation(
            _currentlyEquippedGun.Positions.GunAimPosition,
            _currentlyEquippedGun.Positions.GunAimRotation,
            aimSpeed,
            aimFOV,
            1f,
            0f);

        _animator.SetBool(Constants.IsWeaponUp, _isWeaponUp);
        _animator.SetBool(Constants.IsAiming, _isWeaponAiming);
    }

    private void MoveWeaponToLowerPosition()
    {
        _isWeaponUp = false;
        _isWeaponAiming = false;

        UpdateWeaponTargetPositionAndRotation(
            _currentlyEquippedGun.Positions.GunLoweredPosition,
            _currentlyEquippedGun.Positions.GunLoweredRotation,
            0.25f,
            _originalFOV, 
            0f,
            1f);

        _animator.SetBool(Constants.IsWeaponUp, _isWeaponUp);
        _animator.SetBool(Constants.IsAiming, _isWeaponAiming);
    }

    private void UpdateWeaponTargetPositionAndRotation(Vector3 targetPosition, Quaternion targetRotation, float lerpLength, float fov, float aimIkWeight, float headIkWeight)
    {
        if (ReferenceEquals(_weaponTransform, null)) return;

        if (!ReferenceEquals(_weaponRotLerpCoroutine, null))
        {
            StopCoroutine(_weaponRotLerpCoroutine);
        }

        _weaponRotLerpCoroutine = LerpToWeaponPositionAndRotation(targetPosition, targetRotation, lerpLength, fov, aimIkWeight, headIkWeight);
        StartCoroutine(_weaponRotLerpCoroutine);
    }

    private IEnumerator WaitForNextRoundToBeReadyToFire()
    {
        yield return new WaitForSeconds(_weaponLockWaitTime);
        _roundsPerMinuteLock = true;
    }

    private IEnumerator LerpToWeaponPositionAndRotation(Vector3 position, Quaternion rotation, float lerpLength, float targetFOV, float aimIkWeight, float headIkWeight)
    {
        float timeElapsed = 0f;

        Vector3 posAtStart = _weaponTransform.localPosition;
        Quaternion rotAtStart = _weaponTransform.localRotation;
        float currentFov = fpCamera.fieldOfView;
        float originalAimIk = aimIk.solver.IKPositionWeight;
        float originalHeadIk = headIk.solver.IKPositionWeight;

        while (timeElapsed < lerpLength)
        {
            float t = timeElapsed / lerpLength;
            _weaponTransform.localPosition = Vector3.Lerp(posAtStart, position, t);
            _weaponTransform.localRotation = Quaternion.Lerp(rotAtStart, rotation, t);
            fpCamera.fieldOfView = Mathf.Lerp(currentFov, targetFOV, t);
            aimIk.solver.IKPositionWeight = Mathf.Lerp(originalAimIk, aimIkWeight, t);
            headIk.solver.IKPositionWeight = Mathf.Lerp(originalHeadIk, headIkWeight, t);

            yield return new WaitForEndOfFrame();
            timeElapsed += Time.deltaTime;
        }

        _weaponTransform.localPosition = position;
        _weaponTransform.localRotation = rotation;
        fpCamera.fieldOfView = targetFOV;
        aimIk.solver.IKPositionWeight = aimIkWeight;
        headIk.solver.IKPositionWeight = headIkWeight;
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
                        _weaponTransform.localEulerAngles,
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
    }
}