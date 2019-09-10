using System;
using System.Collections;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

public class CannonballController : MonoBehaviour
{
    [Require] private EntityId entityId;
    [Require] private WorldCommandSender worldCommandSender;

    [SerializeField] private float blastRadius = 10;
    
    private void OnCollisionEnter(Collision other)
    {
        WorldCommands.DeleteEntity.Request request = new WorldCommands.DeleteEntity.Request(entityId);
        worldCommandSender.SendDeleteEntityCommand(request);
        Destroy(gameObject);
    }
}
