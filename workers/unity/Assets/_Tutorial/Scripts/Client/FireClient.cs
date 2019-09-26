using System.Collections;
using System.Collections.Generic;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.Core.Commands;
using Improbable.Gdk.QueryBasedInterest;
using Improbable.Gdk.Subscriptions;
using Improbable.Worker.CInterop;
using Tank;
using UnityEngine;

public class FireClient : MonoBehaviour
{
    [Require] private EntityId entityId;
    [Require] private HealthCommandSender healthCommandSender;
    [Require] private WorldCommandSender worldCommandSender;
    [Require] private WeaponsFxWriter weaponsFxWriter;
    [Require] private FireCannonballWriter fireCannonball;

    [SerializeField] private GameObject firingPoint;
    [SerializeField] private GameObject cannonFiringPoint;
    [SerializeField] private LayerMask hitMask;
    [SerializeField] private float cannonFireDelay = 1f;
    [SerializeField] private GameObject cannonballGameObject;

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
                HitValidator request = new HitValidator(hit.collider.gameObject.GetComponent<LinkedEntityComponent>().EntityId.Id);
                healthCommandSender.SendValidateHitCommand(entityId, request, OnCommandSent);
            }
            weaponsFxWriter.SendMachineGunEffectEvent(new Empty());
        }

        if (Input.GetMouseButtonDown(1) && fireCannon)
        {
            
            GameObject go = GameObject.Instantiate(cannonballGameObject, cannonFiringPoint.transform.position,
                cannonFiringPoint.transform.rotation);
            go.GetComponent<Rigidbody>().AddForce(gameObject.GetComponent<Transform>().forward * GameConstants.CannonForce);
            fireCannonball.SendFireEvent(new Empty());
            fireCannon = false;
            StartCoroutine(nameof(SetFireCannon));
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
