using BlankProject;
using Improbable.Gdk.Core;
using Improbable.Gdk.Subscriptions;
using UnityEngine;

[WorkerType(UnityClientConnector.WorkerType)]
public class HealthBarUI : MonoBehaviour
{
    [Require] private EntityId entityId;
    [SerializeField] private Transform canvasTransform;

    private Vector3 canvasRootPosition;
    private new GameObject camera;

    private void OnEnable()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
    }

    private void Update()
    {
        if (camera != null)
        {
            canvasTransform.rotation = camera.transform.rotation;
        }
        else
        {
            {
                camera = GameObject.FindGameObjectWithTag("MainCamera");
            }
        }
    }
}