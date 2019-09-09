using System;
using System.Collections;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Tank;
using UnityEngine;

public class ValidateHit : MonoBehaviour
{
    [Require] private HealthCommandReceiver healthCommandReceiver;
    [Require] private HealthCommandSender healthCommandSender;

    private void OnEnable()
    {
        healthCommandReceiver.OnValidateHitRequestReceived += OnValidateHit;
    }

    private void OnValidateHit(Health.ValidateHit.ReceivedRequest request)
    {
        
        //TO DO: Validate Hit
        HealthModifier healthModifier = new HealthModifier(request.Payload.Damage);
        //healthCommandSender.SendUpdateHealthCommand(request.Payload.EntityIdHit, healthModifier, null);
    }
}
