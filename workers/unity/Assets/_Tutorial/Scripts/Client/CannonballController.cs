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
    [SerializeField] private LayerMask layerMask;
    
    private void OnCollisionEnter(Collision other)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, blastRadius, layerMask);
        if (hitColliders.Length > 0)
        {
            foreach (var v in hitColliders)
            {
            }
        }
        
        Destroy(gameObject);
        if (worldCommandSender != null)
        {
            WorldCommands.DeleteEntity.Request request = new WorldCommands.DeleteEntity.Request(entityId);
            worldCommandSender.SendDeleteEntityCommand(request);
        }
    }
}
