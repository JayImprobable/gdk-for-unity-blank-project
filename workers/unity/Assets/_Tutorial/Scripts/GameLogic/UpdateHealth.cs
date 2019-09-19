using System;
using System.Collections;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Subscriptions;
using Tank;
using UnityEngine;

public class UpdateHealth : MonoBehaviour
{
    [Require] private EntityId entityId;
    [Require] private HealthCommandReceiver healthCommandReceiver;
    [Require] private HealthWriter healthWriter;
    [Require] private WorldCommandSender worldCommandSender;

    private int machineGunDamage;

    private void OnEnable()
    {
        healthCommandReceiver.OnUpdateHealthRequestReceived += UpdateHealthCommandReceived;
    }

    private void UpdateHealthCommandReceived(Health.UpdateHealth.ReceivedRequest request)
    {
        var updatedHealth = healthWriter.Data.Health + request.Payload.Amount;
        if (updatedHealth > GameConstants.MaxHealth)
        {
            updatedHealth = GameConstants.MaxHealth;
        }

        Health.Update update = new Health.Update
        {
            Health = updatedHealth
        };
        healthWriter.SendUpdate(update);
        healthCommandReceiver.SendUpdateHealthResponse(request.RequestId, new Empty());
    }

    void OnDeleteEntityResponse(WorldCommands.DeleteEntity.ReceivedResponse response)
    {
        Debug.LogWarning($"Entity {entityId} deleted");
    }
}
