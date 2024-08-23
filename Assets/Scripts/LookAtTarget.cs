using UnityEngine;

public class LookAtTarget : MonoBehaviour
{
    [SerializeField] private Transform target; 
    //[SerializeField] private float cameraLookSpeed;
    
    private void Start()
    {        


        //aimIk.solver.OnPostUpdate += OnPostFBBIK;
    }

    private void Update()
    {

         //Vector3 direction = target.transform.position - transform.position;
         //Quaternion toRotation = Quaternion.LookRotation(direction);
         //transform.rotation = Quaternion.Lerp(transform.rotation, toRotation, cameraLookSpeed * Time.deltaTime);
        
        transform.LookAt(target.position);
        //transform.eulerAngles = new Vector3(transform.eulerAngles.x, transform.eulerAngles.y, 0);
    }
}
