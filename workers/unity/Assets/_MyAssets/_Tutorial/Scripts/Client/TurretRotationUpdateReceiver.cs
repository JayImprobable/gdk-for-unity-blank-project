using Improbable.Gdk.Subscriptions;
using Tank;
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
        turretTransform.Rotate(0, update.Rotation, 0);
    }
}