using UnityEngine;

public class MoveToHeadPosition : MonoBehaviour
{
    [SerializeField] private Transform target;


    //Doing it this way as moving directly to the game object causes weird jittering
    private void FixedUpdate()
    {
        transform.SetPositionAndRotation(target.position, target.rotation);
    }
}