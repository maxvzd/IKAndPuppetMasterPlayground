using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Random = UnityEngine.Random;

public class FireWeapon : MonoBehaviour
{
    //[SerializeField] private Transform fireAtTarget;
    //[SerializeField] private float weaponRecoil;
    
    [SerializeField] private AnimationCurve recoilCurve;

    //private GunSwayAndRecoilBehaviour _fireAtTargetLerper;

    private void Start()
    {
        //_fireAtTargetLerper = fireAtTarget.GetComponent<GunSwayAndRecoilBehaviour>();
    }

    public void Fire(float recoil, GunSwayAndRecoilBehaviour swayBehaviour, Vector3 desiredRotation, bool isAiming, float weaponHandling)
    {
        StartCoroutine(RotateWeaponCoRoutine(desiredRotation, isAiming, weaponHandling));

        if (isAiming)
        {
            recoil *= 0.5f;
        }
        
        float xSway = Random.Range(-0.01f, 0.01f); //Random bit of shake in the x direction so that it doesn't just go straight up
        swayBehaviour.AddRecoil(new Vector3(xSway, recoil, 0));
    }
    
    //Amount of rotation + recovery time influenced by weapon handling (and eventually skill)
    private IEnumerator RotateWeaponCoRoutine(Vector3 desiredRotation, bool isAiming, float weaponHandling)
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
        
        float lerpTime = 0.2f * weaponHandling;
        
        //Vector3 oldRot = _targetWeaponRot.eulerAngles;
        //Vector3 oldRot = _targetWeaponRot.eulerAngles;
        Vector3 recoilJitter = new Vector3(xRot, yRot, zRot);
        
        yield return LerpToRotation(lerpTime * 2, desiredRotation, desiredRotation + recoilJitter, transform, recoilCurve);
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
    }
}
