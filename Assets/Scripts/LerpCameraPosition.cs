using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LerpCameraPosition : MonoBehaviour
{
    [SerializeField] private Transform cameraTargetTransform;
    private float lerpDuration;
    private float timeElapsed;
    
    // Start is called before the first frame update
    void Start()
    {
        lerpDuration = 0.01f;
    }

    // Update is called once per frame
    void Update()
    {
        Transform currentTransform = transform;
        Vector3 targetPosition = cameraTargetTransform.position;
       
        //if (currentTransform.position != cameraTargetTransform.position)
        //{
        //  transform.position = Vector3.Lerp(currentTransform.position, targetPosition, Time.deltaTime);
        //}


        if (timeElapsed < lerpDuration)
        {
            transform.position = Vector3.Lerp(currentTransform.position, targetPosition, timeElapsed);
            timeElapsed += Time.deltaTime;
        }
        else
        {
            transform.position = targetPosition;
            timeElapsed = 0;
        }
    }
}
