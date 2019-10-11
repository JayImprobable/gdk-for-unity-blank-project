using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Subscriptions;
using Improbable.Worker.CInterop;
using Improbable.Worker.CInterop.Query;
////#18 - Class that contains the HealthCommandReceived, HealthCommandSender and WeaponsReader Components
//using Tank;
using UnityEngine;
using EntityQuery = Improbable.Worker.CInterop.Query.EntityQuery;

public class ValidateHit : MonoBehaviour
{
//    //#18 - HealthCommandReceiver is used to receive and handle requests
//    [Require] private HealthCommandReceiver healthCommandReceiver;
//    //#19 - Sends the command to reduce the health in case of a valid hit
//    [Require] private HealthCommandSender healthCommandSender;
    [Require] private WorldCommandSender worldCommandSender;
//    //#18 - Weapons Reader reads the damage dealt by the machine gun from the Component
//    [Require] private WeaponsReader weaponsReader;
    [Require] private EntityId entityId;
    //#24 - WorkerFlagReader will allow a callback to be called when a flag is changed
    [Require] private WorkerFlagReader workerFlagReader;

    private EntityId entityIdHit;
    //#19 - Worker origin offset
    private Vector3 workerOrigin;
    //#19 - Damage inflicted
    private int damage;
    //#30 - Class that send logs to the Console
    private ILogDispatcher logger;

    private void OnEnable()
    {
//        //#18 - Setting the callback that will handle ValidateHit requests
//        healthCommandReceiver.OnValidateHitRequestReceived += OnValidateHit;
        workerOrigin = GetComponent<LinkedEntityComponent>().Worker.Origin;
//        //#19 - Read the machine gun damage value from the Weapon Component
//        damage = weaponsReader.Data.MachineGunDamage;
        //#24 - setting the callback to be called when a flag is changed
        workerFlagReader.OnWorkerFlagChange += UpdateDamageValue;
        //#30 - Setting the variable to the LogDispatcher contained within the Worker
        logger = GetComponent<LinkedEntityComponent>().Worker.LogDispatcher;
    }

//    //#18 - ValidateHit callback
//    private void OnValidateHit(Health.ValidateHit.ReceivedRequest request)
//    {
//        entityIdHit = new EntityId(request.Payload.EntityIdHit);
//        worldCommandSender.SendEntityQueryCommand(
//            new WorldCommands.EntityQuery.Request(
//                new EntityQuery
//                {
//                    Constraint = new EntityIdConstraint(request.Payload.EntityIdHit),
//                    ResultType = new SnapshotResultType()
//                }), QueryResultReceived);
//        healthCommandReceiver.SendValidateHitResponse(request.RequestId, new Empty());
//    }

//    //#18 - callback for the Entity Query
//    void QueryResultReceived(WorldCommands.EntityQuery.ReceivedResponse response)
//    {
//        if (response.StatusCode == StatusCode.Success)
//        {
//            if (response.Result.Count > 0)
//            {
//                var v = response.Result[entityIdHit].GetComponentSnapshot<Position.Snapshot>();
//                if (v != null)
//                {
//                    //Debug.Log($"Hit!!!");
////                    //#19 - Prepare and send the UpdateHealthCommand
////                    Vector3 hitPosition = v.Value.Coords.ToUnityVector();
////                    Vector3 myPosition = transform.position - workerOrigin;
////                    var distance = Vector3.Distance(hitPosition, myPosition);
////                    if (distance <= GameConstants.MaxFireDistance)
////                    {
////                        //Prepare the Command payload...
////                        HealthModifier request = new HealthModifier(damage);
////                        //...and Send the command
////                        healthCommandSender.SendUpdateHealthCommand(entityIdHit, request, UpdateHealthCommandCallback);
////                    }
//                    else
//                    {
//                        //Player is cheating!
//                        //#30 - Send the Log to the console (and to the Unity Editor if playing within the Editor)
//                        logger.HandleLog(LogType.Warning, new LogEvent($"Potential cheat: {entityId.Id} - Reason: Hitting from too far " +
//                                                                       $"(max distance {GameConstants.MaxFireDistance} - current distance {distance})"));
//                    }
//                }
//            }
//        }
//    }
    
//    //#19 - calback for the SendUpdateHealth Command
//    private void UpdateHealthCommandCallback(Tank.Health.UpdateHealth.ReceivedResponse response)
//    {
//        if (response.StatusCode != StatusCode.Success)
//        {
//            Debug.LogWarning($"Update Health request response = {response.Message}");
//        }
//    }
    
    //#24 - Callback when a flag is changed. It receives a key value pair containing the flag name and the new value
    private void UpdateDamageValue(string value, string key)
    {
        damage = int.Parse(key) * -1;
    }
}