using UnityEngine;

public class EquipmentInputSystem : MonoBehaviour
{
    //This'll change eventually obviously
    [SerializeField] private Gun currentlyEquippedGun;
    
    // Start is called before the first frame update
    private void Start()
    {
        
    }

    // Update is called once per frame
    private void Update()
    {
        if (Input.GetButton(Constants.Fire1Key))
        {
            currentlyEquippedGun.TriggerDown();
        }

        if (Input.GetButtonUp(Constants.Fire1Key))
        {
            currentlyEquippedGun.TriggerUp();
        }
    }
}
