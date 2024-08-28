using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace WeaponFireBehaviours
{
    public interface IWeaponFireBehaviour
    {
        FireMode FireMode { get; }
    
        void TriggerDown(
            Gun gun,
            GunSwayAndRecoilBehaviour gunSwayBehaviour,
            bool isWeaponAiming,
            Transform target, 
            Transform from, 
            VisualEffect muzzleFlashEffect,
            GameObject muzzleFlashLight, 
            AnimationCurve recoilCurve, 
            AudioSource audioSource);
        
        void TriggerUp(Gun gun);
    }
}