using UnityEngine;
using UnityEngine.VFX;

namespace WeaponFireBehaviours
{
    public class SemiAutoFireBehaviour : BaseFireBehaviour
    {
        private bool _hasFiredWithTriggerDown;
        
        public override FireMode FireMode => FireMode.SemiAuto;
        
        public SemiAutoFireBehaviour(GunProperties gunProps) : base(gunProps)
        {
            _hasFiredWithTriggerDown = false;
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
            
            if (RoundsPerMinuteLock && !_hasFiredWithTriggerDown)
            {
                RoundsPerMinuteLock = false;
                _hasFiredWithTriggerDown = true;
                
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
            _hasFiredWithTriggerDown = false;
        }
    }
}