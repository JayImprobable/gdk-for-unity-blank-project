using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Subscriptions;
using Improbable.Worker.CInterop;
using Improbable.Worker.CInterop.Query;
using Tank;
using UnityEngine;
using EntityQuery = Improbable.Worker.CInterop.Query.EntityQuery;

public class ValidateHit : MonoBehaviour
{
    [Require] private HealthCommandReceiver healthCommandReceiver;
    [Require] private HealthCommandSender healthCommandSender;
    [Require] private WorldCommandSender worldCommandSender;
    [Require] private WeaponsReader weaponsReader;
    [Require] private EntityId entityId;

    private int damage;
    private EntityId entityIdHit;
    private Vector3 workerOrigin;
    private ILogDispatcher logger;

    private void OnEnable()
    {
        healthCommandReceiver.OnValidateHitRequestReceived += OnValidateHit;
        workerOrigin = GetComponent<LinkedEntityComponent>().Worker.Origin;
        logger = GetComponent<LinkedEntityComponent>().Worker.LogDispatcher;
        damage = weaponsReader.Data.MachineGunDamage;
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
                if (v != null)
                {
                    Debug.Log($"Hit!!!");
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
}