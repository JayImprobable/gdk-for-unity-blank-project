using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.Subscriptions;
using Improbable.Worker.CInterop;
using Tank;
using UnityEngine;

public class FireClient : MonoBehaviour
{
    //#17 - Entity ID will be used to send the Command
    [Require] private EntityId entityId;
    //#17 - HealthCommandSender is used to send the Command
    [Require] private HealthCommandSender healthCommandSender;

    [SerializeField] private GameObject firingPoint;
    [SerializeField] private LayerMask hitMask;

    private float fireDistance;

    private void Update()
    {

        if (Input.GetMouseButtonDown(0)) //Machine Gun
        {
            fireDistance = GameConstants.MaxFireDistance;
            if (Physics.Raycast(firingPoint.transform.position, firingPoint.transform.forward, out var hit, fireDistance, hitMask))
            {
                //#17 - Creating the HitValidator instance that will be sent as payload of the Command
                HitValidator request = new HitValidator(hit.collider.gameObject.GetComponent<LinkedEntityComponent>().EntityId.Id);
                //#17 - Sending the Command
                healthCommandSender.SendValidateHitCommand(entityId, request, OnCommandResponse);
            }
        }
    }

    private void OnCommandResponse(Health.ValidateHit.ReceivedResponse response)
    {
        if (response.StatusCode != StatusCode.Success)
        {
            Debug.LogWarning($"Hit validation request response = {response.Message}");
        }
    }
}
