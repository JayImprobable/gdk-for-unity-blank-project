using Improbable.Gdk.Subscriptions;
////#14 - class that contains the TurretRotationReader Component
//using Tank;
using UnityEngine;

public class TurretRotationUpdateReceiver : MonoBehaviour
{
//    //#14 - TurretRotationReader used to receive Updates
//    [Require] private TurretRotationReader turretRotationReader;

    [SerializeField] private Transform turretTransform;
    
    void OnEnable()
    {
//        //#14 - Setting the Update callback
//        turretRotationReader.OnUpdate += UpdateReceived;
    }

//    //#14 - Update Callback
//    void UpdateReceived(TurretRotation.Update update)
//    {
//        turretTransform.Rotate(0, update.Rotation, 0);
//    }
}