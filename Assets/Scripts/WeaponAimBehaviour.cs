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

    //[SerializeField] private AimIK weaponAimIK;
    [SerializeField] private Transform weaponSlot; 
    [SerializeField] private Camera fpCamera;
    [SerializeField] private float aimFOV;
    [SerializeField] private float aimSpeed;
    [SerializeField] private BipedIK bipedIk;
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

    //[SerializeField] private Transform rightHandIKTarget;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();

        // _gunUpPosition = new Vector3(-0.0417f, -0.1447f, 0.4391f);
        // _gunUpRotation = Quaternion.Euler(new Vector3(211.174f, -4.632f, 91.028f));
        // _gunAimPosition = new Vector3(-0.18f, -0.156f, 0.499f);
        // _gunAimRotation = Quaternion.Euler(new Vector3(215.889f, 10.08f, 79.5f));
        // _gunLoweredPosition = new Vector3(0.076f, -0.242f, 0.227f);
        // _gunLoweredRotation = Quaternion.Euler(new Vector3(252.846f, 38.435f, 52.246f));

        //GameObject weaponGameObject = null;

        _weaponTransform = null;
        if (weaponSlot.childCount > 0)
        {
            _weaponTransform = weaponSlot.GetChild(0);
            
            _weaponFireScript = _weaponTransform.GetComponent<FireWeapon>();
            _currentlyEquippedGun = _weaponTransform.GetComponent<Gun>();
            _gunSwayBehaviour = weaponAimTarget.GetComponent<GunSwayAndRecoilBehaviour>();
            
            //This will eventually rely more on player skill
            _gunSwayBehaviour.SetSwayAmount(_currentlyEquippedGun.Stats.Handling * 0.01f);
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

        bipedIk.solvers.aim.IKPositionWeight = 1;
        bipedIk.solvers.lookAt.IKPositionWeight = 0;
        // weaponAimIK.solver.IKPositionWeight = 1;

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
        
        bipedIk.solvers.aim.IKPositionWeight = 1;
        bipedIk.solvers.lookAt.IKPositionWeight = 0;

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
        
        bipedIk.solvers.aim.IKPositionWeight = 0;
        bipedIk.solvers.lookAt.IKPositionWeight = 1;
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

    //Amount of rotation + recovery time influenced by weapon handling (and eventually skill)
    private IEnumerator RotateWeaponCoRoutine(float weaponHandling)
    {
        float weaponSwayModifier = 1f;
        if (_isWeaponAiming)
        {
            weaponSwayModifier = 0.5f;
        }

        float xRange = 6 * weaponHandling * weaponSwayModifier;
        float yRange = 3 * weaponHandling * weaponSwayModifier;
        float zRange = 2 * weaponHandling * weaponSwayModifier; 
        
        float xRot = Random.Range(0, xRange);
        float yRot = Random.Range(-yRange, yRange);
        float zRot = Random.Range(-zRange, zRange);
        
        float lerpTime = 0.2f * weaponHandling;
        
        Vector3 oldRot = _targetWeaponRot.eulerAngles;
        Vector3 recoilJitter = new Vector3(xRot, yRot, zRot);
        
        yield return LerpToRotation(lerpTime, oldRot, oldRot + recoilJitter);

        yield return LerpToRotation(lerpTime, oldRot + recoilJitter, oldRot);
    }

    private IEnumerator LerpToRotation(float lerpTime, Vector3 oldRot, Vector3 newRot)
    {
        float timeElapsed = 0;
        while (timeElapsed < lerpTime)
        {
            float t = timeElapsed / lerpTime;
            _weaponTransform.localEulerAngles = Vector3.Lerp(oldRot, newRot, t);
            
            timeElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
    }

    // Update is called once per frame
    private void Update()
    {
        if (ReferenceEquals(_weaponTransform, null)) return;
        if (ReferenceEquals(_currentlyEquippedGun, null)) return;
        
        if (Input.GetMouseButtonDown(0))
        {
            if (_isWeaponUp)
            {
                //fire
                if (!ReferenceEquals(_lowerWeaponCoRoutine, null))
                {
                    StopCoroutine(_lowerWeaponCoRoutine);
                }

                _weaponFireScript.Fire(_currentlyEquippedGun.Stats.Recoil, _gunSwayBehaviour);
                StartCoroutine(RotateWeaponCoRoutine(_currentlyEquippedGun.Stats.Handling));

            }
            else
            {
                MoveWeaponToUpPosition();
                ResetLowerWeaponCoRoutine();
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            MoveWeaponToAimPosition();
            if (!ReferenceEquals(_lowerWeaponCoRoutine, null))
            {
                StopCoroutine(_lowerWeaponCoRoutine);
            }

            _lowerWeaponCoRoutine = null;
        }

        if (Input.GetMouseButtonUp(1))
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