using System.Collections;
using System.Diagnostics;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponAimBehaviour : MonoBehaviour
{
    private Animator animator;
    private bool isWeaponUp;
    //private bool isWeaponAiming;
    //private FullBodyBipedIK bodyIK;
    private IEnumerator lowerWeaponCoRoutine;

    private Vector3 gunUpPosition;
    private Quaternion gunUpRotation;
    private Vector3 gunAimPosition;
    private Quaternion gunAimRotation;
    private Vector3 gunLoweredPosition;
    private Quaternion gunLoweredRotation;

    [SerializeField] private Transform weaponTransform;
    [SerializeField] private AimIK weaponAimIK;
    [SerializeField] private Camera camera;
    [SerializeField] private float aimFOV;
    [SerializeField] private float aimSpeed;
    
    private float timeElapsed;
    private float lerpDuration = 1f;
    private Vector3 currentWeaponPos;
    private Quaternion currentWeaponRot;
    private Vector3 targetWeaponPos;
    private Quaternion targetWeaponRot;

    private float originalFOV;
    private float targetFOV;
    private float currentFOV;

    //[SerializeField] private Transform rightHandIKTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        gunUpPosition = new Vector3(-0.0417f, -0.1447f, 0.4391f);
        gunUpRotation = Quaternion.Euler(new Vector3(211.174f, -4.632f, 91.028f));
        gunAimPosition = new Vector3(-0.227f, -0.197f, 0.446f);
        gunAimRotation = Quaternion.Euler(new Vector3(217.257f, 1.814f, 83.89f));
        gunLoweredPosition = new Vector3(0.076f, -0.242f, 0.227f);
        gunLoweredRotation = Quaternion.Euler(new Vector3(252.846f, 38.435f, 52.246f));


        originalFOV = camera.fieldOfView;
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
        if (!ReferenceEquals(lowerWeaponCoRoutine, null))
        {
            StopCoroutine(lowerWeaponCoRoutine);
        }

        lowerWeaponCoRoutine = LowerWeaponAfterXSeconds(3);
        StartCoroutine(lowerWeaponCoRoutine);
    }

    private void MoveWeaponToUpPosition()
    {
        isWeaponUp = true;

        UpdateWeaponTargetPositionAndRotation(gunUpPosition, gunUpRotation, 0.25f, originalFOV);
        
        weaponAimIK.solver.IKPositionWeight = 1;
        
        animator.SetBool(Constants.IsWeaponUp, isWeaponUp);
    }
    
    private void MoveWeaponToAimPosition()
    {
        isWeaponUp = true;
        
        UpdateWeaponTargetPositionAndRotation(gunAimPosition, gunAimRotation, aimSpeed, aimFOV);
        weaponAimIK.solver.IKPositionWeight = 1f;
        
        animator.SetBool(Constants.IsWeaponUp, isWeaponUp);
    }

    private void MoveWeaponToLowerPosition()
    {
        isWeaponUp = false;
        UpdateWeaponTargetPositionAndRotation(gunLoweredPosition, gunLoweredRotation, 0.25f, originalFOV);
        weaponAimIK.solver.IKPositionWeight = 0;
        animator.SetBool(Constants.IsWeaponUp, isWeaponUp);
    }

    private void UpdateWeaponTargetPositionAndRotation(Vector3 targetPosition, Quaternion targetRotation, float timeLength, float fov)
    {
        currentWeaponPos = weaponTransform.localPosition;
        currentWeaponRot = weaponTransform.localRotation;
        
        lerpDuration = timeLength;
        timeElapsed = 0;
        currentFOV = camera.fieldOfView;
        targetFOV = fov;
        
        targetWeaponPos = targetPosition;
        targetWeaponRot = targetRotation;
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isWeaponUp)
            {
                //fire
                if (!ReferenceEquals(lowerWeaponCoRoutine, null))
                {
                    StopCoroutine(lowerWeaponCoRoutine);
                }
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
            if (!ReferenceEquals(lowerWeaponCoRoutine, null))
            {
                StopCoroutine(lowerWeaponCoRoutine);
            }
        
            lowerWeaponCoRoutine = null;
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            MoveWeaponToUpPosition();
            ResetLowerWeaponCoRoutine();
        }
        
        if (timeElapsed < lerpDuration)
        {
            float t = timeElapsed / lerpDuration;
            
            camera.fieldOfView = Mathf.Lerp(currentFOV, targetFOV, t);
            weaponTransform.localPosition = Vector3.Lerp(currentWeaponPos, targetWeaponPos, t);
            weaponTransform.localRotation = Quaternion.Lerp(currentWeaponRot, targetWeaponRot, t);
            
            timeElapsed += Time.deltaTime;
        }
    }
}
