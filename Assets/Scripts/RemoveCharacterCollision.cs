using RootMotion.Dynamics;
using UnityEngine;
using UnityEngine.Serialization;

public class RemoveCharacterCollision : MonoBehaviour
{
    [SerializeField] private GameObject gunGameObject; 
    
    // Start is called before the first frame update
    void Start()
    {
        Collider[] ragdollColliders = GetComponentsInChildren<Collider>();
        Collider[] gunColliders = gunGameObject.GetComponentsInChildren<Collider>();

        foreach (Collider ragdollCollider in ragdollColliders)
        {
            foreach (Collider gunCollider in gunColliders)
            {
                Physics.IgnoreCollision(ragdollCollider, gunCollider);
            }
        }
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
