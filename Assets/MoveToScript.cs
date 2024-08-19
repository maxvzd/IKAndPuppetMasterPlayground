using UnityEngine;

public class MoveToScript : MonoBehaviour
{
    [SerializeField] private Transform objectToMoveTo;
    

    // Update is called once per frame
    void Update()
    {
        transform.SetPositionAndRotation(objectToMoveTo.position, objectToMoveTo.rotation);
    }
}
