using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Tank;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarValueControllerNonAuthoritative : MonoBehaviour
{
    [Require] private HealthReader healthReader;

    [SerializeField] Image foregroundImage;

    private void OnEnable()
    {
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
        foregroundImage.fillAmount = fillAmount;
    }
}