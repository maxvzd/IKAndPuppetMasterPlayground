using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] private Transform target;
    
    void Update()
    {
        transform.LookAt(target);
    }
}
