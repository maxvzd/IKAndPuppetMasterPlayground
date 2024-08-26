using UnityEngine;

public class MoveCamera : MonoBehaviour
{
    private float _yRotation;
    private float _xRotation;

    [SerializeField] private float sensitivity; 
    [SerializeField] private float maxVerticalAngle;
    
    // Sart is called before the first frame update
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
        Cursor.visible = false;
        
        _yRotation = 0f;
        _xRotation = 0f;
    }

    // Update is called once per frame
    private void Update()
    {
        float xDelta = Input.GetAxis("Mouse Y") * sensitivity * Time.deltaTime;
        float yDelta = Input.GetAxis("Mouse X") * sensitivity * Time.deltaTime;
        _xRotation += xDelta;
        _yRotation += yDelta;

        _xRotation = Mathf.Clamp(_xRotation, -maxVerticalAngle, maxVerticalAngle);

        transform.eulerAngles = new Vector3(-_xRotation, _yRotation, 0);
    }
}
