using System;
using System.Collections;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Subscriptions;
using Tank;
using UnityEngine;

public class CannonballController : MonoBehaviour
{
    [Require] private EntityId entityId;
    [Require] private WorldCommandSender worldCommandSender;
    [Require] private HealthCommandSender healthCommandSender;

    [SerializeField] private float blastRadius = 10;
    [SerializeField] private LayerMask layerMask;
    
    private void OnCollisionEnter(Collision other)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, blastRadius, layerMask);
        if (hitColliders.Length > 0)
        {
            foreach (var v in hitColliders)
            {
                if (v.gameObject.CompareTag("Tank"))
                {
                    Debug.Log($"I am {gameObject.name}");
                    HealthModifier request = new HealthModifier(GameConstants.CannonDamage);
                    healthCommandSender.SendUpdateHealthCommand(v.gameObject.GetComponent<LinkedEntityComponent>().EntityId, request,HealthCommandResponseReceived);
                }
            }
        }
        
        Destroy(gameObject);
        if (worldCommandSender != null)
        {
            WorldCommands.DeleteEntity.Request request = new WorldCommands.DeleteEntity.Request(entityId);
            worldCommandSender.SendDeleteEntityCommand(request);
        }
    }

    private void HealthCommandResponseReceived(Tank.Health.UpdateHealth.ReceivedResponse response)
    {
    }
}
