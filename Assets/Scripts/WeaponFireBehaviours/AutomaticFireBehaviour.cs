using System.Collections;
using UnityEngine;
using UnityEngine.VFX;

namespace WeaponFireBehaviours
{
    public class AutomaticFireBehaviour: BaseFireBehaviour
    {
        public override FireMode FireMode => FireMode.Auto;
        private IEnumerator _fireCoroutine;

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
            _fireCoroutine = AutoFire(gunSwayBehaviour,
                gun.transform,
                isWeaponAiming,
                gun.Properties,
                target,
                from,
                muzzleFlashEffect,
                muzzleFlashLight,
                recoilCurve,
                audioSource);
            gun.StartCoroutine(_fireCoroutine);
        }

        public override void TriggerUp(Gun gun)
        {
            gun.StopCoroutine(_fireCoroutine);
        }

        private IEnumerator AutoFire(
            GunSwayAndRecoilBehaviour gunSwayBehaviour, 
            Transform weaponTransForm, 
            bool isWeaponAiming, 
            GunProperties properties, 
            Transform target, 
            Transform from, 
            VisualEffect muzzleFlashEffect,
            GameObject muzzleFlashLight, 
            AnimationCurve recoilCurve, 
            AudioSource audioSource)
        {
            while (true)
            {
                FireWeapon.Fire(gunSwayBehaviour,
                    weaponTransForm.eulerAngles,
                    isWeaponAiming,
                    properties,
                    target,
                    from,
                    muzzleFlashEffect,
                    muzzleFlashLight,
                    recoilCurve,
                    audioSource);
                yield return WaitForNextRoundToBeReadyToFire();
            }
        }

        public AutomaticFireBehaviour(GunProperties gunProps) : base(gunProps)
        {
        }
    }
}