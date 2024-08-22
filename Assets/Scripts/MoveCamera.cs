using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private float yRotation;
    private float xRotation;

    [SerializeField] private float sensitivity; 
    [SerializeField] private float maxVerticalAngle;
    [SerializeField] private Camera fpCamera;
    
    // Sart is called before the first frame update
    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        yRotation = 0f;
        xRotation = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        float xDelta = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        float yDelta = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        xRotation += xDelta;
        yRotation += yDelta;

        xRotation = Mathf.Clamp(xRotation, -maxVerticalAngle, maxVerticalAngle);

        transform.eulerAngles = new Vector3(-xRotation, yRotation, 0);
        //fpCamera.transform.eulerAngles = new Vector3(-xRotation, yRotation, 0);
        //fpCamera.transform.Rotate(new Vector3(-xDelta, yDelta, -fpCamera.transform.eulerAngles.z));
    }
}
