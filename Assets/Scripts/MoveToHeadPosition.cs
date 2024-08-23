using RootMotion.FinalIK;
using UnityEngine;

public class MoveToHeadPosition : MonoBehaviour
{
    [SerializeField] private BipedIK bipedIk;
    
    //Doing it this way as moving directly to the game object causes weird jittering
    private void FixedUpdate()
    {
        //Do this
        Transform headTransform = bipedIk.references.head;
        transform.SetPositionAndRotation(headTransform.position, headTransform.rotation);
    }
}