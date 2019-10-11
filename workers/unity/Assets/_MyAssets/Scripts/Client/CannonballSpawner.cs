////#23 - Class containing the FireCannonballReader
//using Tank;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

public class CannonballSpawner : MonoBehaviour
{
//    //#23 - FireCannonaballReader is used to receive the event
//    [Require] private FireCannonballReader fireCannonball;

    [SerializeField] private GameObject cannonFiringPoint;
    [SerializeField] private GameObject cannonballGameObject;
    
    // Start is called before the first frame update
    void Start()
    {
//        //#23 - sets the callback to be used when the Fire event is received
//        fireCannonball.OnFireEvent += FireEventReceived;
    }

    private void FireEventReceived(Empty e)
    {
        GameObject go = GameObject.Instantiate(cannonballGameObject, cannonFiringPoint.transform.position,
            cannonFiringPoint.transform.rotation);
        go.GetComponent<Rigidbody>().AddForce(go.GetComponent<Transform>().forward * GameConstants.CannonForce);
    }
}