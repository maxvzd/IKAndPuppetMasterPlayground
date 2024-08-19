using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private float yRotation;
    private float xRotation;

    [SerializeField] private float sensitivity; 
    [SerializeField] private float maxVerticalAngle; 
    
    
    // Sart is called before the first frame update
    void Start()
    {
        yRotation = 0f;
        xRotation = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        xRotation += Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        yRotation += Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;

        xRotation = Mathf.Clamp(xRotation, -maxVerticalAngle, maxVerticalAngle);

        transform.eulerAngles = new Vector3(-xRotation, yRotation, 0);
    }
}
