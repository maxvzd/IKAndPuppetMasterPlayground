using System.Collections;
using RootMotion.FinalIK;
using UnityEngine;
using UnityEngine.Serialization;

public class WeaponAimBehaviour : MonoBehaviour
{
    private Animator animator;
    private bool isWeaponUp;
    //private bool isWeaponAiming;
    private FullBodyBipedIK bodyIK;
    private IEnumerator lowerWeaponCoRoutine;

    [SerializeField] private Transform gunUpRightHandPosition;
    [SerializeField] private Transform gunAimRightHandPosition;
    [SerializeField] private Transform rightHandIKTarget;
    [SerializeField] private AimIK weaponAimIK;
    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        bodyIK = GetComponent<FullBodyBipedIK>();
    }

    private IEnumerator LowerWeaponAfterXSeconds(int numberOfSecondsToWait)
    {
        yield return new WaitForSeconds(numberOfSecondsToWait);

        LowerWeapon();
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
                
        rightHandIKTarget.SetParent(gunUpRightHandPosition, false);
        TurnOnRightHandIK();
        
        animator.SetBool(Constants.IsWeaponUp, isWeaponUp);
    }
    
    private void MoveWeaponToAimPosition()
    {
        isWeaponUp = true;
                
        rightHandIKTarget.SetParent(gunAimRightHandPosition, false);
        TurnOnRightHandIK();
        
        animator.SetBool(Constants.IsWeaponUp, isWeaponUp);
    }

    private void LowerWeapon()
    {
        TurnOffRightHandIK();
        isWeaponUp = false;
        animator.SetBool(Constants.IsWeaponUp, isWeaponUp);
        //weaponAimIK.solver.
    }

    // Update is called once per frame
    private void Update()
    {
        // if (Input.GetMouseButtonDown(0))
        // {
        //     if (isWeaponUp)
        //     {
        //         //fire
        //     }
        //     else
        //     {
        //         MoveWeaponToUpPosition();
        //         ResetLowerWeaponCoRoutine();
        //     }
        // }
        //
        //
        // if (Input.GetMouseButtonDown(1))
        // {
        //     MoveWeaponToAimPosition();
        //     if (!ReferenceEquals(lowerWeaponCoRoutine, null))
        //     {
        //         StopCoroutine(lowerWeaponCoRoutine);
        //     }
        // }
        //
        // if (Input.GetMouseButtonUp(1))
        // {
        //     MoveWeaponToUpPosition();
        //     ResetLowerWeaponCoRoutine();
        // }
    }

    private void TurnOnRightHandIK()
    {
        SetRightHandIK(1f);
    }

    private void TurnOffRightHandIK()
    {
        SetRightHandIK(0f);
    }

    private void SetRightHandIK(float weight)
    {
        bodyIK.solver.rightHandEffector.positionWeight = weight;
        bodyIK.solver.rightHandEffector.rotationWeight = weight;
    }
}
