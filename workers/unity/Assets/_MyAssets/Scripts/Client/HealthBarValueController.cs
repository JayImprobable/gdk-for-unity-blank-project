using Improbable.Gdk.Subscriptions;
using Tank;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarValueController : MonoBehaviour
{
    //#20 - used to register the Update callback
    [Require] private HealthReader healthReader;
    //#21 - used to send visual effects events
    [Require] private WeaponsFxWriter weaponsFxWriter;

    [SerializeField] Image foregroundImage;

    private void OnEnable()
    {
        //#20 - Registering the callback
        healthReader.OnUpdate += OnHealthUpdate;
        float fillAmount = (float)healthReader.Data.Health / 100;
        foregroundImage.fillAmount = fillAmount;
    }
    
    private void OnHealthUpdate(Health.Update update)
    {
        if (!update.Health.HasValue)
        {
            return;
        }
        if (update.Health.Value <= 0)
        {
            gameObject.SetActive(false);
            return;
        }
        float fillAmount = (float)update.Health.Value / 100;
        //#21 - If the health update was a damage, I send an event to show the damage effect
        if (fillAmount < foregroundImage.fillAmount)
        {
            weaponsFxWriter.SendShowEffectEvent(new Effect(EffectEnum.DAMAGE));
        }
        foregroundImage.fillAmount = fillAmount;
        
    }
}