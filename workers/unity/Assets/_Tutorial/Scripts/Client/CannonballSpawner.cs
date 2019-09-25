using Tank;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

public class CannonballSpawner : MonoBehaviour
{
    [Require] private FireCannonballReader fireCannonball;

    [SerializeField] private GameObject cannonFiringPoint;
    [SerializeField] private GameObject cannonballGameObject;
    
    // Start is called before the first frame update
    void Start()
    {
        fireCannonball.OnFireEvent += FireEventReceived;
    }

    private void FireEventReceived(Empty e)
    {
        GameObject go = GameObject.Instantiate(cannonballGameObject, cannonFiringPoint.transform.position,
            cannonFiringPoint.transform.rotation);
        go.GetComponent<Rigidbody>().AddForce(gameObject.GetComponent<Transform>().forward * GameConstants.CannonForce);
    }
}
