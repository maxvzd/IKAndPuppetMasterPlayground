using RootMotion.FinalIK;
using UnityEngine;

public class TurnBehaviour : MonoBehaviour
{
    [Header("AssignableObjects")] 
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform mouseTarget;
    
    [Header("Variables")] 
    [SerializeField] private float turnToleranceDegrees;
    [SerializeField] private float turnSpeed;

    private Animator _animator;
    private bool _isTurning;
    private AnimationEventListener _animationEventListener;

    // Start is called before the first frame update
    void Start()
    {
        _animator = GetComponent<Animator>();
        _animationEventListener = GetComponent<AnimationEventListener>();

        _animationEventListener.OnFinishedTurning += (sender, args) =>
        {
            _isTurning = false;
        };
    }

    // Update is called once per frame
    void Update()
    {
        float forwardDirection = Input.GetAxis(Constants.ForwardDirectionKey);
        float rightDirection = Input.GetAxis(Constants.RightDirectionKey);
        
        if (forwardDirection > 0 || forwardDirection < 0 || rightDirection > 0 || rightDirection < 0)
        {
            Vector3 mouseTargetPosition = mouseTarget.position;
            Vector3 currentPosition = transform.position;
            
            //TODO this lerping needs working as it's not right
            Vector3 relativePos = new Vector3(mouseTargetPosition.x, currentPosition.y, mouseTargetPosition.z) - currentPosition;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
        }

        if (!_isTurning)
        {
            float angleBetweenCameraAndBody = cameraTransform.eulerAngles.y - transform.eulerAngles.y;
            
            if (angleBetweenCameraAndBody < 0)
            {
                angleBetweenCameraAndBody += 360;
            }

            if (angleBetweenCameraAndBody > 0 + turnToleranceDegrees && angleBetweenCameraAndBody < 180)
            {
                _isTurning = true;
                _animator.SetTrigger(Constants.TurnRightTrigger);
            }

            if (angleBetweenCameraAndBody > 180 && angleBetweenCameraAndBody < 360 - turnToleranceDegrees)
            {
                _isTurning = true;
                _animator.SetTrigger(Constants.TurnLeftTrigger);
            }
        }
    }
}