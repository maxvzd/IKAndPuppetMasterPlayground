using UnityEngine;

public class MovementScript : MonoBehaviour
{
    private Animator animator;
    
    [SerializeField] private Transform mouseTarget;

    private float walkSpeedModifier;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        float forwardDirection = Input.GetAxis("Vertical");
        float rightDirection = Input.GetAxis("Horizontal");

        walkSpeedModifier += Input.GetAxis("Mouse ScrollWheel");
        walkSpeedModifier = Mathf.Clamp(walkSpeedModifier, 0.5f, 2f);
        
        animator.SetFloat(Constants.ForwardDirection, forwardDirection * walkSpeedModifier);
        animator.SetFloat(Constants.RightDirection, rightDirection * walkSpeedModifier);
    }
}