using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NegativeLookAt : MonoBehaviour
{
    [SerializeField] private Transform targetTransform;
    
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 offset = transform.position - targetTransform.position;
        transform.LookAt(transform.position + offset);
    }
}
