using System.Collections;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using Improbable.Worker.CInterop;
using Tank;
using UnityEngine;

public class FireClient : MonoBehaviour
{
//    //#17 - Entity ID will be used to send the Command
//    [Require] private EntityId entityId;
//    //#17 - HealthCommandSender is used to send the Command
//    [Require] private HealthCommandSender healthCommandSender;
//    //#21 - Adding a WeaponsFxWriter in order to send the effect Event
//    [Require] private WeaponsFxWriter weaponsFxWriter;
//    //#23 - Adding a FireCannonballWriter in order to send the fire cannonball Event
//    [Require] private FireCannonballWriter fireCannonball;

    [SerializeField] private GameObject firingPoint;
    [SerializeField] private LayerMask hitMask;
//    //#23 - Cannonball prefab
//    [SerializeField] private GameObject cannonballGameObject;
//    //#23 - Point where the cannonball will be spawned
//    [SerializeField] private GameObject cannonFiringPoint;
//    //#23 - Time between cannon shots
//    [SerializeField] private float cannonFireDelay = 1f;
//    //#30 - Cheating flag and UI text
//    [SerializeField] private bool cheat;
//    [SerializeField] private GameObject cheatModeText;

    private float fireDistance;
    
//    //#23 - Variable that controls if I can fire the cannon 
//    private bool fireCannon;

    private void OnEnable()
    {
//        //#23 - Setting the initial fireCannon value
//        fireCannon = true;
    }
    
    private void Update()
    {
//        //#30 - cheat mode input
//        if (Input.GetKeyDown(KeyCode.C))
//        {
//            cheat = !cheat;
//            cheatModeText.SetActive(!cheatModeText.activeSelf);
//        }
        
        if (Input.GetMouseButtonDown(0)) //Machine Gun
        {
            fireDistance = GameConstants.MaxFireDistance;
            //#30 - changing the firing range depending if cheat is enabled or disabled
//            if (!cheat)
//            {
//                fireDistance = GameConstants.MaxFireDistance;
//            }
//            else
//            {
//                fireDistance = 200f;
//            }
            if (Physics.Raycast(firingPoint.transform.position, firingPoint.transform.forward, out var hit, fireDistance, hitMask))
            {
//                //#17 - Creating the HitValidator instance that will be sent as payload of the Command
//                HitValidator request = new HitValidator(hit.collider.gameObject.GetComponent<LinkedEntityComponent>().EntityId.Id);
//                //#17 - Sending the Command
//                healthCommandSender.SendValidateHitCommand(entityId, request, OnCommandResponse);
            }
//            //#21 - Sending the event, passing the MACHINE_GUN enum as parameter
//            weaponsFxWriter.SendShowEffectEvent(new Effect(EffectEnum.MACHINE_GUN));
        }
        
//        //#23 - Cannonball firing code
//        if (Input.GetMouseButtonDown(1) && fireCannon)
//        {
//            
//            GameObject go = GameObject.Instantiate(cannonballGameObject, cannonFiringPoint.transform.position,
//                cannonFiringPoint.transform.rotation);
//            go.GetComponent<Rigidbody>().AddForce(go.GetComponent<Transform>().forward * GameConstants.CannonForce);
//            //#23 - After spawning the cannonball and applying a force, I send the event so the cannonball is created
//            //by other workers
//            fireCannonball.SendFireEvent(new Empty());
//            fireCannon = false;
//            StartCoroutine(nameof(SetFireCannon));
//        }
    }

//    //#17 - command response callback
//    private void OnCommandResponse(Health.ValidateHit.ReceivedResponse response)
//    {
//        if (response.StatusCode != StatusCode.Success)
//        {
//            Debug.LogWarning($"Hit validation request response = {response.Message}");
//        }
//    }
    
//    //#23 - Corountine to control when you can and cannot fire the cannon
//    IEnumerator SetFireCannon()
//    {
//        yield return new WaitForSeconds(cannonFireDelay);
//        fireCannon = true;
//    }
}
