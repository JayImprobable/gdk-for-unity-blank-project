using Improbable.Gdk.Subscriptions;
////#20 - Class containing the HealthReader Component
//using Tank;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarValueControllerNonAuthoritative : MonoBehaviour
{
//    //#20 - HealthReader used to receive Health updates
//    [Require] private HealthReader healthReader;
    [SerializeField] Image foregroundImage;

    private float fillAmount = 1f;

    private void OnEnable()
    {
//        //#20 - registering the callback to be called when Update is received and setting the initial amount
//        healthReader.OnUpdate += OnHealthUpdate;
//        float fillAmount = (float)healthReader.Data.Health / 100;
        foregroundImage.fillAmount = fillAmount;
    }
    
//    //#20 - Callback that updates the health amount
//    private void OnHealthUpdate(Health.Update update)
//    {
//        if (!update.Health.HasValue)
//        {
//            return;
//        }
//        if (update.Health.Value <= 0)
//        {
//            gameObject.SetActive(false);
//        }
//        float fillAmount = (float)update.Health.Value / 100;
//        
//        foregroundImage.fillAmount = fillAmount;
//    }
}