using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Unity.Entities;
using Tank;
using Improbable;
using Improbable.Gdk.Core;

[UpdateInGroup(typeof(FixedUpdateSystemGroup))]
public class TankMovementSystem : ComponentSystem
{
    private EntityQuery eq;
    //private Vector3 origin;

    protected override void OnCreate()
    {
        base.OnCreate();

        //origin = World.GetExistingSystem<WorkerSystem>().Origin;
        eq = GetEntityQuery(ComponentType.ReadOnly<Transform>(),
            ComponentType.ReadWrite<Rigidbody>(),
            ComponentType.ReadOnly<FireCannonball.ComponentAuthority>());

        eq.SetFilter(FireCannonball.ComponentAuthority.Authoritative);
    }
    protected override void OnUpdate()
    {
        Entities.With(eq).ForEach((Entity e, Transform t, Rigidbody rb) =>
        {
            float movementInput = Input.GetAxis("Vertical");
            float rotationInput = Input.GetAxis("Horizontal");

            Vector3 movement = t.forward * movementInput * 12f * Time.deltaTime;
            rb.MovePosition(rb.position + movement);

            float turn = rotationInput * 180f * Time.deltaTime;
            Quaternion turnRotation = Quaternion.Euler(0, turn, 0);
            rb.MoveRotation(rb.rotation * turnRotation);
        });
    }
}
