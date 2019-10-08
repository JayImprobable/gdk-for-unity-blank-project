using BlankProject;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Tank;
using UnityEngine;

[WorkerType(UnityClientConnector.WorkerType)]
public class TankColorWriter : MonoBehaviour
{
    //Adding the TankColor component writer so we can send the update once the color is set
    [Require] private Tank.TankColorWriter tankColorWriter;
    
    private UnityEngine.Color chassisColor;
    private UnityEngine.Color turretColor;
    [SerializeField] private GameObject chassis;
    [SerializeField] private GameObject turret;

    private void OnEnable()
    {
        chassisColor = new UnityEngine.Color((float)Random.Range(0, 255) / 255, (float)Random.Range(0, 255) / 255, (float)Random.Range(0, 255) / 255);
        chassis.GetComponent<MeshRenderer>().material.color = chassisColor;
        
        turretColor = new UnityEngine.Color((float)Random.Range(0, 255) / 255, (float)Random.Range(0, 255) / 255, (float)Random.Range(0, 255) / 255);
        turret.GetComponent<MeshRenderer>().material.color = turretColor;

        //Setting the component update
        var update = new Tank.TankColor.Update
        {
            Chassis = new ChassisColor(chassisColor.r, chassisColor.g, chassisColor.b),
            Turret = new TurretColor(turretColor.r, turretColor.g, turretColor.b)
        };
        //Sending the update
        tankColorWriter.SendUpdate(update);
    }
}