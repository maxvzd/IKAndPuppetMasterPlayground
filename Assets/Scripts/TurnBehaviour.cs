using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TurnBehaviour : MonoBehaviour
{
    [Header("AssignableObjects")] 
    [SerializeField] private Transform cameraTransform;
    [SerializeField] private Transform mouseTarget;

    [Header("Variables")] 
    [SerializeField] private float turnToleranceDegrees;
    [SerializeField] private float turnSpeed;

    private Animator animator;
    private bool isTurning;
    private AnimationEventListener animationEventListener;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
        animationEventListener = GetComponent<AnimationEventListener>();

        animationEventListener.OnFinishedTurning += (sender, args) => { isTurning = false; };
    }

    // Update is called once per frame
    void Update()
    {
        float forwardDirection = Input.GetAxis(Constants.ForwardDirectionKey);
        if (forwardDirection > 0)
        {
            Vector3 mouseTargetPosition = mouseTarget.position;
            Vector3 currentPosition = transform.position;

            Vector3 relativePos = new Vector3(mouseTargetPosition.x, currentPosition.y, mouseTargetPosition.z) - currentPosition;
            Quaternion toRotation = Quaternion.LookRotation(relativePos);
            transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, turnSpeed * Time.deltaTime);
        }

        if (!isTurning)
        {
            float angleBetweenCameraAndBody = cameraTransform.eulerAngles.y - transform.eulerAngles.y;
            if (angleBetweenCameraAndBody < 0)
            {
                angleBetweenCameraAndBody += 360;
            }

            if (angleBetweenCameraAndBody > 0 + turnToleranceDegrees && angleBetweenCameraAndBody < 180)
            {
                isTurning = true;
                animator.SetTrigger(Constants.TurnRightTrigger);
            }

            if (angleBetweenCameraAndBody > 180 && angleBetweenCameraAndBody < 360 - turnToleranceDegrees)
            {
                isTurning = true;
                animator.SetTrigger(Constants.TurnLeftTrigger);
            }
        }
    }
}