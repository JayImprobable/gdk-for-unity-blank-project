using System;
using System.Collections;
using System.Collections.Generic;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Subscriptions;
using Improbable.Worker.CInterop;
using Improbable.Worker.CInterop.Query;
using Tank;
using Unity.Entities;
using UnityEngine;
using EntityQuery = Improbable.Worker.CInterop.Query.EntityQuery;

public class ValidateHit : MonoBehaviour
{
    [Require] private HealthCommandReceiver healthCommandReceiver;
    [Require] private HealthCommandSender healthCommandSender;
    [Require] private WorldCommandSender worldCommandSender;
    [Require] private WeaponsReader weaponsReader;
    [Require] private WorkerFlagReader workerFlagReader;

    private int damage;
    private EntityId entityIdHit;
    private Vector3 workerOrigin;

    private void OnEnable()
    {
        healthCommandReceiver.OnValidateHitRequestReceived += OnValidateHit;
        workerFlagReader.OnWorkerFlagChange += UpdateDamageValue;
        workerOrigin = gameObject.GetComponent<LinkedEntityComponent>().Worker.Origin;
        damage = weaponsReader.Data.MachineGunDamage * -1;
    }

    private void OnValidateHit(Health.ValidateHit.ReceivedRequest request)
    {
        entityIdHit = new EntityId(request.Payload.EntityIdHit);
        worldCommandSender.SendEntityQueryCommand(
            new WorldCommands.EntityQuery.Request(
                new EntityQuery
                {
                    Constraint = new EntityIdConstraint(request.Payload.EntityIdHit),
                    ResultType = new SnapshotResultType()
                }), QueryResultReceived);
        healthCommandReceiver.SendValidateHitResponse(request.RequestId, new Empty());
    }

    void QueryResultReceived(WorldCommands.EntityQuery.ReceivedResponse response)
    {
        if (response.StatusCode == StatusCode.Success)
        {
            if (response.Result.Count > 0)
            {
                var v = response.Result[entityIdHit].GetComponentSnapshot<Position.Snapshot>();
                Vector3 hitPosition = v.Value.Coords.ToUnityVector();
                Vector3 myPosition = transform.position - workerOrigin;
                if (Vector3.Distance(hitPosition, myPosition) <= GameConstants.MaxFireDistance)
                {
                    HealthModifier request = new HealthModifier(damage);
                    healthCommandSender.SendUpdateHealthCommand(entityIdHit, request, UpdateHealthCommandCallback);
                }
                else
                {
                    //Cheat
                }
            }
        }
    }

    private void UpdateHealthCommandCallback(Tank.Health.UpdateHealth.ReceivedResponse response)
    {
        if (response.StatusCode != StatusCode.Success)
        {
            Debug.LogWarning($"Update Health request response = {response.Message}");
        }
    }
    
    private void UpdateDamageValue(string value, string key)
    {
        damage = int.Parse(key) * -1;
    }
}