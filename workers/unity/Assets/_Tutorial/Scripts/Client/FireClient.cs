using System;
using System.Collections;
using System.Collections.Generic;
using BlankProject.Scripts.Config;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Subscriptions;
using Improbable.Worker.CInterop;
using Tank;
using UnityEngine;

public class FireClient : MonoBehaviour
{
    [Require] private EntityId entityId;
    [Require] private WeaponsReader weaponsReader;
    [Require] private HealthCommandSender healthCommandSender;
    [Require] private WorldCommandSender worldCommandSender;

    [SerializeField] private GameObject firingPoint;
    [SerializeField] private GameObject cannonFiringPoint;
    [SerializeField] private LayerMask hitMask;

    private LinkedEntityComponent linkedEntityComponent;

    private void OnEnable()
    {
        linkedEntityComponent = gameObject.GetComponent<LinkedEntityComponent>();
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) //Machine Gun
        {
            if (Physics.Raycast(firingPoint.transform.position, firingPoint.transform.forward, out var hit, GameConstants.MaxFireDistance, hitMask))
            {
                HitValidator request = new HitValidator(hit.collider.gameObject.GetComponent<LinkedEntityComponent>().EntityId.Id, weaponsReader.Data.MachineGunDamage);
                healthCommandSender.SendValidateHitCommand(entityId, request, OnCommandSent);
            }
        }

        if (Input.GetMouseButtonDown(1))
        {
            var entityTemplate = EntityTemplates.CreateCannonballEntityTemplate(linkedEntityComponent.Worker.WorkerId, cannonFiringPoint.transform.position);
            WorldCommands.CreateEntity.Request request = new WorldCommands.CreateEntity.Request(entityTemplate);
            worldCommandSender.SendCreateEntityCommand(request, CannonballEntityCreated);
        }
    }

    private void OnCommandSent(Health.ValidateHit.ReceivedResponse response)
    {
        if (response.StatusCode != StatusCode.Success)
        {
            Debug.LogWarning($"Hit validation request response = {response.Message}");
        }
    }

    private void CannonballEntityCreated(WorldCommands.CreateEntity.ReceivedResponse response)
    {
    }
}
