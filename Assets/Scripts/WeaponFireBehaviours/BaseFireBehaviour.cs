using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace WeaponFireBehaviours
{
    public abstract class BaseFireBehaviour : IWeaponFireBehaviour
    {
        private readonly float _weaponLockWaitTime;
        protected bool RoundsPerMinuteLock;
        public virtual FireMode FireMode => FireMode.SemiAuto;
        

        protected BaseFireBehaviour(GunProperties gunProps)
        {
            RoundsPerMinuteLock = true;
            float roundsPerMinute = gunProps.RoundsPerMinute;
            float roundsPerSecond = roundsPerMinute / 60f;
            _weaponLockWaitTime = 1 / roundsPerSecond;
        }


        //Leave for concrete class to override

        public virtual void TriggerDown(
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
        }


        //Leave for concrete class to override
        public virtual void TriggerUp(Gun gun)
        {
        }
        
        protected IEnumerator WaitForNextRoundToBeReadyToFire()
        {
            yield return new WaitForSeconds(_weaponLockWaitTime);
            RoundsPerMinuteLock = true;
        }
        
        protected IEnumerator WaitForNoTime()
        {
            yield return new WaitForEndOfFrame();
            RoundsPerMinuteLock = true;
        }
    }
}