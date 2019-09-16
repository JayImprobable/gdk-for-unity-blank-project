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
    [Require] private HealthCommandSender healthCommandSender;
    [Require] private WorldCommandSender worldCommandSender;

    [SerializeField] private GameObject firingPoint;
    [SerializeField] private LayerMask hitMask;

    private void Update()
    {
        if (Input.GetMouseButtonDown(0)) //Machine Gun
        {
            if (Physics.Raycast(firingPoint.transform.position, firingPoint.transform.forward, out var hit, GameConstants.MaxFireDistance, hitMask))
            {
                HitValidator request = new HitValidator(hit.collider.gameObject.GetComponent<LinkedEntityComponent>().EntityId.Id);
                healthCommandSender.SendValidateHitCommand(entityId, request, OnCommandSent);
            }
        }
    }

    private void OnCommandSent(Health.ValidateHit.ReceivedResponse response)
    {
        if (response.StatusCode != StatusCode.Success)
        {
            Debug.LogWarning($"Hit validation request response = {response.Message}");
        }
    }
}
