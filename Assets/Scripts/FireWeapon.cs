using System.Collections;
using UnityEngine;
using UnityEngine.VFX;
using Random = UnityEngine.Random;

public class FireWeapon : MonoBehaviour
{
//     [SerializeField] private AnimationCurve recoilCurve;
//     [SerializeField] private VisualEffect muzzleFlashVFX;
//     [SerializeField] private GameObject muzzleFlashLight;
//     [SerializeField] private GameObject firePoint;
//
//     [SerializeField] private GameObject bulletImpact;

    //private AudioSource _audioSource;

    // private void Start()
    // {
    //     _audioSource = GetComponent<AudioSource>();
    // }
    
    //Calculates bullet drop
    //Is this even worth it tbh?? Do we really need to account for that much distance?
    //Changed it to a straight raycast for now - bullet physics is it's own entire thing
    private static void RayCastShot(GunProperties gunProps, Transform target, Transform from)
    {
        // float stepForEachRayCast = 5f;
        // float gravity = 9.81f;
        // bool isHit = false;
        // float distanceTravelled = 0f;
        //
        // Vector3 origin = firePoint.transform.position;
        // //Vector3 endPoint = new Vector3(origin.x, gravity * (stepForEachRayCast / gunProps.MuzzleVelocity), origin.z + gunProps.MuzzleVelocity);
        //
        // Vector3 endPoint = CalculateEndPointOfShotOverXMetres(gravity, stepForEachRayCast, gunProps.MuzzleVelocity, distanceTravelled);
        //
        // while (!isHit && distanceTravelled < gunProps.MuzzleVelocity)
        // {
        //     Vector3 direction = endPoint - origin;
        //     //Ray ray = new Ray(firePoint.transform.position, direction);
        //     Debug.DrawRay(origin, direction, Color.green, 5f);
        //     
        //     if(Physics.Raycast(origin, direction, out RaycastHit hit))
        //     {
        //         isHit = true;
        //
        //         Instantiate(bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
        //         Debug.DrawRay(hit.point, hit.normal * 0.2f, Color.red, 3f);
        //
        //         break;
        //     }
        //
        //     origin = endPoint;
        //     endPoint = CalculateEndPointOfShotOverXMetres(gravity, stepForEachRayCast, gunProps.MuzzleVelocity, distanceTravelled);
        //     distanceTravelled += stepForEachRayCast;
        // }

        //Doesn't account for bullet drop
        Vector3 origin = from.position;
        //minus forward cause gun is incorrectly orientated backwards
        Vector3 direction = (target.position - origin).normalized;
        
        Debug.DrawRay(origin, direction * gunProps.EffectiveRange, Color.green, 3);
        
        if (Physics.Raycast(origin, direction, out RaycastHit hit, gunProps.EffectiveRange))
        {
            //Instantiate(bulletImpact, hit.point, Quaternion.FromToRotation(Vector3.up, hit.normal));
            Debug.DrawRay(hit.point, hit.normal * 0.2f, Color.red, 3f);
        }
        
    }

    //forward transform is only negative because the gun faces the wrong way!!!
    //TO DO better weight + drag calculations
    private Vector3 CalculateEndPointOfShotOverXMetres(float gravity, float distance, float muzzleVelocity, float totalDistanceTravelled)
    {
        Transform currentTransform = transform;
        float velocityWithDrag = muzzleVelocity - totalDistanceTravelled * 50; //50 is the bullet slowing down 50m/s
        
        return -currentTransform.forward * velocityWithDrag - currentTransform.up * (gravity * (distance / velocityWithDrag));
    }
    
    public static void Fire(
        GunSwayAndRecoilBehaviour swayBehaviour, 
        Vector3 currentRotation, 
        bool isAiming, 
        GunProperties gunProps, 
        Transform to,
        Transform from,
        VisualEffect muzzleFlashVFX,
        GameObject muzzleFlashLight,
        AnimationCurve recoilCurve,
        AudioSource audioSource)
    {
        RayCastShot(gunProps, to, from);

        //TODO: This needs some reworking
        //StartCoroutine(RotateWeaponCoRoutine(currentRotation, isAiming, gunProps.Handling));
        muzzleFlashVFX.Play();
        muzzleFlashLight.SetActive(true);

        audioSource.clip = gunProps.FireSound;
        float pitch = Random.Range(0.9f, 1.1f);
        audioSource.pitch = pitch;
        audioSource.Play();

        float recoil = gunProps.Recoil;
        if (isAiming)
        {
            recoil *= 0.5f;
        }
        
        float xSway = Random.Range(-0.01f, 0.01f); //Random bit of shake in the x direction so that it doesn't just go straight up
        swayBehaviour.AddRecoil(new Vector3(xSway, recoil, 0));
    }
    
    //Amount of rotation + recovery time influenced by weapon handling (and eventually skill)
    private IEnumerator RotateWeaponCoRoutine(Vector3 currentRotation, bool isAiming, float weaponHandling, AnimationCurve recoilCurve)
    {
        float weaponSwayModifier = 1f;
        if (isAiming)
        {
            weaponSwayModifier = 0.5f;
        }

        float xRange = 10 * weaponHandling * weaponSwayModifier;
        float yRange = 10 * weaponHandling * weaponSwayModifier;
        float zRange = 10 * weaponHandling * weaponSwayModifier; 
        
        float xRot = Random.Range(0, xRange);
        float yRot = Random.Range(-yRange, yRange);
        float zRot = Random.Range(-zRange, zRange);
        
        float lerpTime = 0.1f * weaponHandling;
        
        //Vector3 oldRot = _targetWeaponRot.eulerAngles;
        //Vector3 oldRot = _targetWeaponRot.eulerAngles;
        Vector3 recoilJitter = new Vector3(xRot, yRot, zRot);
        
        yield return LerpToRotation(lerpTime * 2, currentRotation, currentRotation + recoilJitter, transform, recoilCurve);
    }

    private static IEnumerator LerpToRotation(float lerpTime, Vector3 oldRot, Vector3 newRot, Transform weaponTransform, AnimationCurve curve)
    {
        float timeElapsed = 0;
        while (timeElapsed < lerpTime)
        {
            
            float t = timeElapsed / lerpTime;
            
            weaponTransform.localEulerAngles = Vector3.Lerp(oldRot, newRot, curve.Evaluate(t));
            
            timeElapsed += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }

        weaponTransform.localEulerAngles = oldRot;
    }
}
