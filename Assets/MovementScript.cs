using UnityEngine;

public class MovementScript : MonoBehaviour
{
    private Animator animator;
    [SerializeField] private Transform cameraTransform;

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
        
        animator.SetFloat(Constants.ForwardDirection, forwardDirection);
        animator.SetFloat(Constants.RightDirection, rightDirection);


        if (forwardDirection > 0)
        {
            transform.rotation = Quaternion.Euler( 0, cameraTransform.eulerAngles.y, 0);
        }
    }
}
