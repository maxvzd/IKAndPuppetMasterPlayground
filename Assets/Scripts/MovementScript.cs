using UnityEngine;

public class MovementScript : MonoBehaviour
{
    private Animator animator;
    
    [SerializeField] private float turnSpeed = 0.5f;
    [SerializeField] private Transform mouseTarget;

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


        //if (forwardDirection > 0)
        {
            Vector3 mouseTargetPosition = mouseTarget.position;
            Vector3 currentPosition = transform.position;
            
            Vector3 relativePos = new Vector3(mouseTargetPosition.x, currentPosition.y, mouseTargetPosition.z) - currentPosition;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
        }
    }
}
