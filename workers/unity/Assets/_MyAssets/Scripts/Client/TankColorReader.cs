using Improbable.Gdk.Subscriptions;
////#15 - Class that contains the TankColorReader
//using Tank;
using UnityEngine;

public class TankColorReader : MonoBehaviour
{
//    //#15 - TankColorReader used to receive Updates
//    [Require] private Tank.TankColorReader tankColorReader;
    
    [SerializeField] private GameObject chassisGameObject;
    [SerializeField] private GameObject turretGameObject;

    private void OnEnable()
    {
//        //#15 - Registering the update callback and setting the color
//        tankColorReader.OnUpdate += ColorUpdated;
//        SetTankColors(tankColorReader.Data.Chassis, tankColorReader.Data.Turret);
    }

//    //#15 - Color update callback
//    void ColorUpdated(Tank.TankColor.Update update)
//    {
//        SetTankColors(update.Chassis, update.Turret);
//    }

//    //#15 - Function that sets the tank color
//    private void SetTankColors(ChassisColor chassis, TurretColor turret)
//    {
//        chassisGameObject.GetComponent<MeshRenderer>().material.color = new UnityEngine.Color(chassis.Red, chassis.Green, chassis.Blue);
//        turretGameObject.GetComponent<MeshRenderer>().material.color = new UnityEngine.Color(turret.Red, turret.Green, turret.Blue);
//    }
}