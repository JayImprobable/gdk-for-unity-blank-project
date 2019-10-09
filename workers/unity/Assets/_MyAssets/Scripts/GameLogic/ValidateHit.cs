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
    //#19 - Sends the command to reduce the health in case of a valid hit
    [Require] private HealthCommandSender healthCommandSender;
    [Require] private WorldCommandSender worldCommandSender;
    [Require] private WeaponsReader weaponsReader;
    [Require] private EntityId entityId;

    private EntityId entityIdHit;
    //#19 - Worker origin offset
    private Vector3 workerOrigin;
    //#19 - Damage inflicted
    private int damage;

    private void OnEnable()
    {
        healthCommandReceiver.OnValidateHitRequestReceived += OnValidateHit;
        workerOrigin = GetComponent<LinkedEntityComponent>().Worker.Origin;
        //#19 - Read the machine gun damage value from the Weapon Component
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
                    //Debug.Log($"Hit!!!");
                    //#19 - Prepare and send the UpdateHealthCommand
                    Vector3 hitPosition = v.Value.Coords.ToUnityVector();
                    Vector3 myPosition = transform.position - workerOrigin;
                    var distance = Vector3.Distance(hitPosition, myPosition);
                    if (distance <= GameConstants.MaxFireDistance)
                    {
                        //Prepare the Command payload...
                        HealthModifier request = new HealthModifier(damage);
                        //...and Send the command
                        healthCommandSender.SendUpdateHealthCommand(entityIdHit, request, UpdateHealthCommandCallback);
                    }
                    else
                    {
                        //Player is cheating!
                    }
                }
            }
        }
    }
    
    //#19 - calback for the SendUpdateHealth Command
    private void UpdateHealthCommandCallback(Tank.Health.UpdateHealth.ReceivedResponse response)
    {
        if (response.StatusCode != StatusCode.Success)
        {
            Debug.LogWarning($"Update Health request response = {response.Message}");
        }
    }
}