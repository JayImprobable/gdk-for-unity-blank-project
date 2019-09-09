using System.Collections;
using System.Collections.Generic;
using Improbable.Gdk.Subscriptions;
using Tank;
using Unity.Mathematics;
using UnityEngine;

public class TurretRotationUpdateReceiver : MonoBehaviour
{
    [Require] private TurretRotationReader turretRotationReader;

    [SerializeField] private Transform turretTransform;
    
    void OnEnable()
    {
        turretRotationReader.OnUpdate += UpdateReceived;
    }

    void UpdateReceived(TurretRotation.Update update)
    {
        Debug.Log($"Rotate?");
        turretTransform.Rotate(0, update.Rotation, 0);
    }
}
