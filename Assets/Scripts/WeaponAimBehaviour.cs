using System.Collections;
using RootMotion.FinalIK;
using UnityEngine;

public class WeaponAimBehaviour : MonoBehaviour
{
    public bool IsWeaponUp => _isWeaponUp; 
    public bool IsWeaponAiming => _isWeaponAiming; 
    
    private Animator _animator;
    private bool _isWeaponUp;
    private bool _isWeaponAiming;
    private IEnumerator _lowerWeaponCoRoutine;

    private Gun _currentlyEquippedGun;
    private Transform _weaponTransform;

    [SerializeField] private Transform weaponSlot;
    [SerializeField] private Camera fpCamera;
    [SerializeField] private float aimFOV;
    [SerializeField] private float aimSpeed;
    [SerializeField] private AimIK headIk;
    [SerializeField] private AimIK aimIk;

    private float _originalFOV;
    private IEnumerator _weaponRotLerpCoroutine;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();

        _weaponTransform = null;
        if (weaponSlot.childCount > 0)
        {
            _weaponTransform = weaponSlot.GetChild(0);
            
            _currentlyEquippedGun = _weaponTransform.GetComponent<Gun>();
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

        _lowerWeaponCoRoutine = LowerWeaponAfterXSeconds(10);
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
        
        if (Input.GetButtonDown(Constants.Fire1Key))
        {
            if (!_isWeaponUp)
            {
                MoveWeaponToUpPosition();
            }
            ResetLowerWeaponCoRoutine();
        }
        
        if (Input.GetButtonDown(Constants.RaiseLowerWeaponKey))
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
        
        if (Input.GetButtonDown(Constants.Fire2Key))
        {
            MoveWeaponToAimPosition();
            if (!ReferenceEquals(_lowerWeaponCoRoutine, null))
            {
                StopCoroutine(_lowerWeaponCoRoutine);
            }
        
            _lowerWeaponCoRoutine = null;
        }
        
        if (Input.GetButtonUp(Constants.Fire2Key))
        {
            MoveWeaponToUpPosition();
            ResetLowerWeaponCoRoutine();
        }
    }
}