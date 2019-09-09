using System;
using System.Collections;
using System.Collections.Generic;
using BlankProject;
using Improbable.Gdk.Subscriptions;
using Tank;
using UnityEngine;
using UnityEngine.UI;

[WorkerType(UnityClientConnector.WorkerType)]
public class HealthBarUI : MonoBehaviour
{
    [Require] private HealthReader healthReader;
    [SerializeField] Image foregroundImage;
    [SerializeField] private Transform canvasTransform;

    private Vector3 canvasRootPosition;
    private GameObject camera;

    private void OnEnable()
    {
        camera = GameObject.FindGameObjectWithTag("MainCamera");
        healthReader.OnUpdate += OnHealthUpdate;
        Debug.Log($"OnEnable");
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
    private void OnHealthUpdate(Health.Update update)
    {
        
        if (!update.Health.HasValue)
        {
            return;
        }
        float fillAmount = (float)update.Health.Value / 100;
        foregroundImage.fillAmount = fillAmount;
    }
}