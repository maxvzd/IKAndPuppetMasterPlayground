using RootMotion.FinalIK;
using UnityEngine;

public class AimingScript : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private AimIK aimIk;
    [SerializeField] private Transform gunObject;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetMouseButtonDown(1))
        {
            animator.SetBool(Constants.IsAiming, true);
           // aimIk.
        }

        if (Input.GetMouseButtonUp(1))
        {
            animator.SetBool(Constants.IsAiming, false);
        }
    }
}
