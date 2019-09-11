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
    [SerializeField] private float cannonFireDelay = 1f;

    private LinkedEntityComponent linkedEntityComponent;
    private bool fireCannon;

    private void OnEnable()
    {
        linkedEntityComponent = gameObject.GetComponent<LinkedEntityComponent>();
        fireCannon = true;
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

        if (Input.GetMouseButtonDown(1) && fireCannon)
        {
            var entityTemplate = EntityTemplates.CreateCannonballEntityTemplate(linkedEntityComponent.Worker.WorkerId, cannonFiringPoint.transform.position, cannonFiringPoint.transform.eulerAngles);
            WorldCommands.CreateEntity.Request request = new WorldCommands.CreateEntity.Request(entityTemplate);
            worldCommandSender.SendCreateEntityCommand(request, CannonballEntityCreated);
            fireCannon = false;
            StartCoroutine("SetFireCannon");
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

    IEnumerator SetFireCannon()
    {
        yield return new WaitForSeconds(cannonFireDelay);
        fireCannon = true;
    }
}
