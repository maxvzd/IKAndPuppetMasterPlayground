using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] private Transform target; 
    
    private void Update()
    {
        transform.LookAt(target.position);
    }
}
