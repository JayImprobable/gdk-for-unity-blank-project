using System;
using System.Collections;
using System.Collections.Generic;
using Improbable.Gdk.Subscriptions;
using Tank;
using UnityEngine;

public class DealDamage : MonoBehaviour
{
    [Require] private HealthCommandReceiver healthCommandReceiver;
    [Require] private HealthWriter healthWriter;

    private void OnEnable()
    {
        healthCommandReceiver.OnUpdateHealthRequestReceived += HealthCommandReceived;
    }

    private void HealthCommandReceived(Health.UpdateHealth.ReceivedRequest request)
    {
    }
}
