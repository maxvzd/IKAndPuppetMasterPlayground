using System;
using UnityEngine;

public class MoveToScriptViaParent : MonoBehaviour
{
    [SerializeField] private Transform objectToMoveTo;
    

    // Update is called once per frame
    void Update()
    {
        //transform.SetPositionAndRotation(objectToMoveTo.position, objectToMoveTo.rotation);
        // transform.position = objectToMoveTo.position;
        // transform.rotation = objectToMoveTo.rotation;

        
    }

    private void Start()
    {
        transform.SetParent(objectToMoveTo, false);
        //transform.localPosition = Vector3.zero;
        //transform.localEulerAngles = Vector3.zero;
    }
}
