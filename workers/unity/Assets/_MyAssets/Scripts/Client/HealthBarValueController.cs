using Improbable.Gdk.Subscriptions;
////#20 - Class containing the Health Component
//using Tank;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarValueController : MonoBehaviour
{
//    //#20 - used to register the Update callback
//    [Require] private HealthReader healthReader;
//    //#21 - used to send visual effects events
//    [Require] private WeaponsFxWriter weaponsFxWriter;

    [SerializeField] Image foregroundImage;
    private float fillAmount = 1f;

    private void OnEnable()
    {
//        //#20 - Registering the callback and setting the initial value of the health bar
//        healthReader.OnUpdate += OnHealthUpdate;
//        float fillAmount = (float)healthReader.Data.Health / 100;
        foregroundImage.fillAmount = fillAmount;
    }
    
    //#20 - Health update callback
//    private void OnHealthUpdate(Health.Update update)
//    {
//        if (!update.Health.HasValue)
//        {
//            return;
//        }
////        //#25 - When a player health reaches zero I disable the player's gameObject. If I destroy it
////        //the Player Lifecycle module stops sending the heartbeat and will stop receiving updates
////        if (update.Health.Value <= 0)
////        {
////            gameObject.SetActive(false);
////            return;
////        }
//        fillAmount = (float)update.Health.Value / 100;
//        
////        //#28 - If the health update was healing, I send an event to show the heal effect
////        if (fillAmount > foregroundImage.fillAmount)
////        {
////            weaponsFxWriter.SendShowEffectEvent(new Effect(EffectEnum.HEAL));
////        }
//        
////        //#21 - If the health update was a damage, I send an event to show the damage effect
////        if (fillAmount < foregroundImage.fillAmount)
////        {
////            weaponsFxWriter.SendShowEffectEvent(new Effect(EffectEnum.DAMAGE));
////        }
//          //#20 - Update the UI element
////        foregroundImage.fillAmount = fillAmount;
//        
//    }
}