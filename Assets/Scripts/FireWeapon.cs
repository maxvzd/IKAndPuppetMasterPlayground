using UnityEngine;
using Random = UnityEngine.Random;

public class FireWeapon : MonoBehaviour
{
    //[SerializeField] private Transform fireAtTarget;
    //[SerializeField] private float weaponRecoil;
    
    //private GunSwayAndRecoilBehaviour _fireAtTargetLerper;

    private void Start()
    {
        //_fireAtTargetLerper = fireAtTarget.GetComponent<GunSwayAndRecoilBehaviour>();
    }

    public void Fire(float recoil, GunSwayAndRecoilBehaviour swayBehaviour)
    {
        float xSway = Random.Range(-0.01f, 0.01f); //Random bit of shake in the x direction so that it doesn't just go straight up
        swayBehaviour.AddRecoil(new Vector3(xSway, recoil, 0));
    }
}
