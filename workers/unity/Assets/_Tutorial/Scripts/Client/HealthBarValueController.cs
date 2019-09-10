﻿using System;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Subscriptions;
using Improbable.Worker.CInterop;
using Tank;
using UnityEngine;
using UnityEngine.UI;

public class HealthBarValueController : MonoBehaviour
{
    [Require] private EntityId entityId;
    [Require] private HealthReader healthReader;
    [Require] private WorldCommandSender worldCommandSender;
    
    [SerializeField] Image foregroundImage;

    private void OnEnable()
    {
        healthReader.OnUpdate += OnHealthUpdate;
        float fillAmount = (float)healthReader.Data.Health / 100;
        foregroundImage.fillAmount = fillAmount;
    }
    
    private void OnHealthUpdate(Health.Update update)
    {
        Debug.Log($"Update value = " + update.Health.Value);
        if (!update.Health.HasValue)
        {
            return;
        }

        if (update.Health.Value <= 0)
        {
            Debug.Log($"HIT");
            Destroy(gameObject);
            worldCommandSender.SendDeleteEntityCommand(new WorldCommands.DeleteEntity.Request(entityId));
        }

        float fillAmount = (float)update.Health.Value / 100;
        foregroundImage.fillAmount = fillAmount;
    }
}
