using UnityEngine;
using UnityEngine.VFX;

namespace WeaponFireBehaviours
{
    public interface IWeaponFireBehaviour
    {
        FireMode FireMode { get; }
    
        bool TriggerDown(
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
            AudioClip emptyClick);
        
        void TriggerUp();
    }
}