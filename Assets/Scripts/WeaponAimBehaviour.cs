using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponAimBehaviour : MonoBehaviour
{
    private Animator animator;
    private bool isWeaponUp;
    private bool isWeaponAiming;

    
    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            if (isWeaponUp)
            {
                //fire
            }
            else
            {
                isWeaponUp = true;
            }
        }
        
        
        if (Input.GetMouseButtonDown(1))
        {
            isWeaponAiming = true;
            isWeaponUp = true;
            
            animator.SetBool(Constants.IsAiming, isWeaponAiming);
        }

        if (Input.GetMouseButtonUp(1))
        {
            isWeaponAiming = false;
            
            animator.SetBool(Constants.IsAiming, isWeaponAiming);
        }
    }
}
