using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace WeaponFireBehaviours
{
    public class AutomaticFireBehaviour: BaseFireBehaviour
    {
        public override FireMode FireMode => FireMode.Auto;
        
        public AutomaticFireBehaviour(GunProperties gunProps) : base(gunProps)
        {
        }

        public override bool TriggerDown(
            Gun gun,
            GunSwayAndRecoilBehaviour gunSwayBehaviour, 
            bool isWeaponAiming, 
            Transform target, 
            Transform from, 
            VisualEffect muzzleFlashEffect,
            GameObject muzzleFlashLight, 
            AnimationCurve recoilCurve, 
            AudioSource audioSource,
            int numberOfBullets,
            AudioClip emptyClick)
        {
            bool successfullyFiredBullet = false;
            
            if (RoundsPerMinuteLock)
            {
                RoundsPerMinuteLock = false;
                
                successfullyFiredBullet = FireWeapon.Fire(gunSwayBehaviour,
                    gun.transform.eulerAngles,
                    isWeaponAiming,
                    gun,
                    target,
                    from,
                    muzzleFlashEffect,
                    muzzleFlashLight,
                    recoilCurve,
                    audioSource,
                    numberOfBullets,
                    emptyClick);
                gun.StartCoroutine(WaitForNextRoundToBeReadyToFire());
            }
            return successfullyFiredBullet;
        }

        public override void TriggerUp()
        {
        }

        // private IEnumerator AutoFire(
        //     GunSwayAndRecoilBehaviour gunSwayBehaviour, 
        //     Transform weaponTransForm, 
        //     bool isWeaponAiming, 
        //     Gun gun, 
        //     Transform target, 
        //     Transform from, 
        //     VisualEffect muzzleFlashEffect,
        //     GameObject muzzleFlashLight, 
        //     AnimationCurve recoilCurve, 
        //     AudioSource audioSource)
        // {
        //     while (true)
        //     {
        //         
        //         yield return WaitForNextRoundToBeReadyToFire();
        //     }
        // }
    }
}