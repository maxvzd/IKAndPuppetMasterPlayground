using System.Collections;
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

    [SerializeField] private Transform gunTransform;
    [SerializeField] private AimIK weaponAimIK;
    
    //[SerializeField] private Transform rightHandIKTarget;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();

        gunUpPosition = new Vector3(-0.0417f, -0.1447f, 0.4391f);
        gunUpRotation = Quaternion.Euler(new Vector3(211.174f, -4.632f, 91.028f));
        gunAimPosition = new Vector3(-0.1332f, -0.1233f, 0.4772f);
        gunAimRotation = Quaternion.Euler(new Vector3(210.351f, 2.5f, 84.615f));
        gunLoweredPosition = new Vector3(0.076f, -0.242f, 0.227f);
        gunLoweredRotation = Quaternion.Euler(new Vector3(252.846f, 38.435f, 52.246f));
        
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
                
        //rightHandIKTarget.SetParent(gunUpPosition, false);
        gunTransform.SetLocalPositionAndRotation(gunUpPosition, gunUpRotation);
        
        weaponAimIK.solver.IKPositionWeight = 1;
        //TurnOnRightHandIK();
        
        animator.SetBool(Constants.IsWeaponUp, isWeaponUp);
    }
    
    private void MoveWeaponToAimPosition()
    {
        isWeaponUp = true;
                
        //rightHandIKTarget.SetParent(gunAimPosition, false);
        //TurnOnRightHandIK();
        
        gunTransform.SetLocalPositionAndRotation(gunAimPosition, gunAimRotation);
       
        weaponAimIK.solver.IKPositionWeight = 1f;
        
        animator.SetBool(Constants.IsWeaponUp, isWeaponUp);
    }

    private void MoveWeaponToLowerPosition()
    {
        //TurnOffRightHandIK();
        isWeaponUp = false;
        gunTransform.SetLocalPositionAndRotation(gunLoweredPosition, gunLoweredRotation);

        weaponAimIK.solver.IKPositionWeight = 0;
        
        animator.SetBool(Constants.IsWeaponUp, isWeaponUp);
        //weaponAimIK.solver.
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isWeaponUp)
            {
                //fire
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
        }
        
        if (Input.GetMouseButtonUp(1))
        {
            MoveWeaponToUpPosition();
            ResetLowerWeaponCoRoutine();
        }
    }
}
