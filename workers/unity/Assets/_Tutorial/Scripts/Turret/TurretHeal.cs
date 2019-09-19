using System;
using System.Collections;
using System.Collections.Generic;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Tank;
using Turret;
using UnityEngine;

public class TurretHeal : MonoBehaviour
{
    [Require] private EntityId entityId;
    [Require] private HealthCommandSender healthCommandSender;
    [Require] private TurretHealReader turretHealReader;

    private Collider[] colliders;
    private float radius;
    private int turretHeal;

    [SerializeField] private LayerMask mask;
    [SerializeField] private float fireRate = 1;

    private void OnEnable()
    {
        radius = GetComponent<SphereCollider>().radius;
        turretHeal = turretHealReader.Data.TurretHeal;
        StartCoroutine(nameof(DealDamage));
    }

    public IEnumerator DealDamage()
    {
        yield return new WaitForSeconds(fireRate);
        colliders = Physics.OverlapSphere(transform.position, radius, mask);
        if (colliders.Length > 0)
        {
            foreach (Collider c in colliders)
            {
                var entityIdHit = c.gameObject.GetComponent<LinkedEntityComponent>().EntityId;
                HealthModifier request = new HealthModifier(turretHeal);
                healthCommandSender.SendUpdateHealthCommand(entityIdHit, request, OnUpdateResponseReceived);
            }
        }
        StartCoroutine(nameof(DealDamage));
    }

    void OnUpdateResponseReceived(Health.UpdateHealth.ReceivedResponse receivedResponse)
    {
    }
}
