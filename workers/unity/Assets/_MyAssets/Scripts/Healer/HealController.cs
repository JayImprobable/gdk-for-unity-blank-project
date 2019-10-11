//using System;
//using System.Collections;
//using System.Collections.Generic;
//using Improbable.Gdk.Core;
//using Improbable.Gdk.Subscriptions;
//using Tank;
//using Healer;
//using UnityEngine;
//
//public class HealController : MonoBehaviour
//{
//    [Require] private EntityId entityId;
//    [Require] private HealthCommandSender healthCommandSender;
//    [Require] private HealValueReader healValueReader;
//    [Require] private ILogDispatcher logger;
//
//    private Collider[] colliders;
//    private float radius;
//    private int turretHeal;
//
//    [SerializeField] private LayerMask mask;
//    [SerializeField] private float healRate = 1;
//
//    private void OnEnable()
//    {
//        radius = GetComponent<SphereCollider>().radius;
//        turretHeal = healValueReader.Data.Value;
//        StartCoroutine(nameof(Heal));
//        LinkedEntityComponent lec = gameObject.GetComponent<LinkedEntityComponent>();
//    }
//
//    //#28 - In this coroutine I regularly check if there's a player within range, if there is, I send an
//    //UpdateHealth Command
//    public IEnumerator Heal()
//    {
//        yield return new WaitForSeconds(healRate);
//        colliders = Physics.OverlapSphere(transform.position, radius, mask);
//        if (colliders.Length > 0)
//        {
//            foreach (Collider c in colliders)
//            {
//                var entityIdHit = c.gameObject.GetComponent<LinkedEntityComponent>().EntityId;
//                HealthModifier request = new HealthModifier(turretHeal);
//                healthCommandSender.SendUpdateHealthCommand(entityIdHit, request, OnUpdateResponseReceived);
//            }
//        }
//        StartCoroutine(nameof(Heal));
//    }
//
//    void OnUpdateResponseReceived(Health.UpdateHealth.ReceivedResponse receivedResponse)
//    {
//    }
//}
