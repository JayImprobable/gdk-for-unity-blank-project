using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Tank;
using UnityEngine;

public class DetectCollisionGameLogic : MonoBehaviour
{
    [Require] private EntityId entityId;

    [SerializeField] private float blastRadius = 10;
    [SerializeField] private LayerMask layerMask;

    private void OnCollisionEnter(Collision other)
    {
        Collider[] hitColliders = Physics.OverlapSphere(transform.position, blastRadius, layerMask);
        if (hitColliders.Length > 0)
        {
            foreach (var v in hitColliders)
            {
                if (v.gameObject.CompareTag("Tank"))
                {
                    v.gameObject.GetComponent<UpdateHealth>().TakeCannonballDamage(GameConstants.CannonDamage);
                }
            }
        }
        Destroy(gameObject);
    }

    private void HealthCommandResponseReceived(Tank.Health.UpdateHealth.ReceivedResponse response)
    {
    }
}
