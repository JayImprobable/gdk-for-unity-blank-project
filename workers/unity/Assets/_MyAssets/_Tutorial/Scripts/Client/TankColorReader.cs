using Improbable.Gdk.Subscriptions;
using Tank;
using UnityEngine;

public class TankColorReader : MonoBehaviour
{
    [Require] private Tank.TankColorReader tankColorReader;
    
    [SerializeField] private GameObject chassisGameObject;
    [SerializeField] private GameObject turretGameObject;

    private void OnEnable()
    {
        tankColorReader.OnUpdate += ColorUpdated;
        SetTankColors(tankColorReader.Data.Chassis, tankColorReader.Data.Turret);
    }

    void ColorUpdated(Tank.TankColor.Update update)
    {
        SetTankColors(update.Chassis, update.Turret);
    }

    private void SetTankColors(ChassisColor chassis, TurretColor turret)
    {
        chassisGameObject.GetComponent<MeshRenderer>().material.color = new UnityEngine.Color(chassis.Red, chassis.Green, chassis.Blue);
        turretGameObject.GetComponent<MeshRenderer>().material.color = new UnityEngine.Color(turret.Red, turret.Green, turret.Blue);
    }
}