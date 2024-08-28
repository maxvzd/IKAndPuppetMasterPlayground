using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.VFX;

namespace WeaponFireBehaviours
{
    public class SemiAutoFireBehaviour : BaseFireBehaviour
    {
        
        public override FireMode FireMode => FireMode.SemiAuto;
        
        public SemiAutoFireBehaviour(GunProperties gunProps) : base(gunProps)
        {
        }

        public override void TriggerDown(
            Gun gun,
            GunSwayAndRecoilBehaviour gunSwayBehaviour, 
            bool isWeaponAiming, 
            Transform target, 
            Transform from, 
            VisualEffect muzzleFlashEffect,
            GameObject muzzleFlashLight, 
            AnimationCurve recoilCurve, 
            AudioSource audioSource)
        {
            if (RoundsPerMinuteLock)
            {
                RoundsPerMinuteLock = false;
                
                FireWeapon.Fire(gunSwayBehaviour,
                    gun.transform.eulerAngles,
                    isWeaponAiming,
                    gun.Properties,
                    target,
                    from,
                    muzzleFlashEffect,
                    muzzleFlashLight,
                    recoilCurve,
                    audioSource);
                gun.StartCoroutine(base.WaitForNextRoundToBeReadyToFire());
            }
        }

        public override void TriggerUp(Gun gun)
        {
            //Do nothing
        }
    }
}