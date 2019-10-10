﻿using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Tank;
using UnityEngine;

public class UpdateHealth : MonoBehaviour
{
    [Require] private HealthCommandReceiver healthCommandReceiver;
    [Require] private HealthWriter healthWriter;

    private int machineGunDamage;

    private void OnEnable()
    {
        //#19 - Setting the callback that will handle the command
        healthCommandReceiver.OnUpdateHealthRequestReceived += UpdateHealthCommandReceived;
    }

    private void UpdateHealthCommandReceived(Health.UpdateHealth.ReceivedRequest request)
    {
        //#19 - update the health and check if it's above maximum health
        var updatedHealth = healthWriter.Data.Health + request.Payload.Amount;
        if (updatedHealth > GameConstants.MaxHealth)
        {
            updatedHealth = GameConstants.MaxHealth;
        }

        SendHealthUpdate(updatedHealth);
        
        //#19 - Send the command response
        healthCommandReceiver.SendUpdateHealthResponse(request.RequestId, new Empty());
    }

    //#23 - Update the health when taking damage from a cannonball
    public void TakeCannonballDamage(int value)
    {
        var updatedHealth = healthWriter.Data.Health + value;

        SendHealthUpdate(updatedHealth);
    }

    private void SendHealthUpdate(int value)
    {
        //#19 - Send the health update that will be used to update the Players UI
        Health.Update update = new Health.Update
        {
            Health = value
        };
        healthWriter.SendUpdate(update);
    }
}