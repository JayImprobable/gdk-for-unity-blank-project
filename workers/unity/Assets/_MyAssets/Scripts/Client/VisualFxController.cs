using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Tank;
using UnityEngine;

public class VisualFxController : MonoBehaviour
{
    [SerializeField] private GameObject machineGunFx;
    [SerializeField] private GameObject damageFx;
    [SerializeField] private GameObject healFx;
    //#21 - Setting the WeaponsFxReader instace
    [Require] private WeaponsFxReader weaponsFxReader;

    private void OnEnable()
    {
        //#21 - Setting the callback for the ShowEffect Event
        weaponsFxReader.OnShowEffectEvent += ShowEffect;
    }

    private void ShowEffect(Effect effect)
    {
        //#21 - Switch based on the payload of the Event
        switch (effect.EffectType)
        {
            case EffectEnum.MACHINE_GUN:
                machineGunFx.GetComponent<ParticleSystem>().Play();
                break;
            
            case EffectEnum.DAMAGE:
                damageFx.GetComponent<ParticleSystem>().Play();
                break;
            
            case EffectEnum.HEAL:
                healFx.GetComponent<ParticleSystem>().Play();
                break;
        }
        
    }
}