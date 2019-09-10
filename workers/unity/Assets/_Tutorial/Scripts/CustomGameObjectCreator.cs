using System;
using System.Collections;
using System.Collections.Generic;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.Subscriptions;
using Unity.Entities;
using UnityEngine;
using Object = UnityEngine.Object;

public class CustomGameObjectCreator : IEntityGameObjectCreator
{
    private readonly IEntityGameObjectCreator fallbackCreator;
    private readonly World world;
    private readonly string workerType;
    private readonly Vector3 workerOrigin;
    private readonly ILogDispatcher logger;

    public CustomGameObjectCreator(IEntityGameObjectCreator fallbackCreator, World world, string workerType, Vector3 workerOrigin, ILogDispatcher logger)
    {
        this.fallbackCreator = fallbackCreator;
        this.world = world;
        this.workerType = workerType;
        this.workerOrigin = workerOrigin;
        this.logger = logger;
    }

    public void OnEntityCreated(SpatialOSEntity entity, EntityGameObjectLinker linker)
    {
        if (!entity.HasComponent<Metadata.Component>())
        {
            return;
        }

        switch (entity.GetComponent<Metadata.Component>().EntityType)
        {
            case "Player":
                CreatePlayer(entity, linker);
                break;
            
            case "Cannonball":
                CreateCannonball(entity, linker);
                break;
            
            default:
                fallbackCreator.OnEntityCreated(entity, linker);
                break;
        }
    }
    
    public void OnEntityRemoved(EntityId entityId)
    {
        fallbackCreator.OnEntityRemoved(entityId);
    }

    private void CreatePlayer(SpatialOSEntity entity, EntityGameObjectLinker linker)
    {
        if (!entity.TryGetComponent<Metadata.Component>(out var metadata) ||
            !entity.TryGetComponent<Position.Component>(out var spatialOSPosition))
        {
            return;
        }

        bool hasAuthority = PlayerLifecycleHelper.IsOwningWorker(entity.SpatialOSEntityId, world);
        string pathToPrefab;
        if (hasAuthority)
        {
            pathToPrefab = $"Prefabs/{workerType}/Authoritative/Player";
        }
        else
        {
            pathToPrefab = $"Prefabs/{workerType}/Non-Authoritative/Player";
        }

        var prefab = Resources.Load<GameObject>(pathToPrefab);
        var position = spatialOSPosition.Coords.ToUnityVector() + workerOrigin;
        
        var playerGameObject = Object.Instantiate(prefab, position, Quaternion.identity);
        playerGameObject.name = $"{metadata.EntityType}(SpatialOS: {entity.SpatialOSEntityId}, Worker: {workerType})";

        Type[] componentsToAdd =
        {
            typeof(Transform),
            typeof(Rigidbody),
            typeof(MeshRenderer)
        };
        linker.LinkGameObjectToSpatialOSEntity(entity.SpatialOSEntityId, (GameObject) playerGameObject,
            componentsToAdd);
    }

    private void CreateCannonball(SpatialOSEntity entity, EntityGameObjectLinker linker)
    {
        if (!entity.TryGetComponent<Metadata.Component>(out var metadata) ||
            !entity.TryGetComponent<Position.Component>(out var spatialOSPosition))
        {
            return;
        }

        string pathToPrefab = $"Prefabs/{workerType}/Cannonball";
        var prefab = Resources.Load<GameObject>(pathToPrefab);
        var position = spatialOSPosition.Coords.ToUnityVector() + workerOrigin;
        var rotation = GameObject.Find("CannonFiringPoint").GetComponent<Transform>().rotation;

        var gameObject = UnityEngine.Object.Instantiate(prefab, position, rotation);
        gameObject.name = $"{metadata.EntityType}(SpatialOS: {entity.SpatialOSEntityId}, Worker: {workerType})";
        Type[] componentsToAdd =
        {
            typeof(Transform),
            typeof(Rigidbody),
            typeof(MeshRenderer)
        };
        linker.LinkGameObjectToSpatialOSEntity(entity.SpatialOSEntityId, (GameObject) gameObject,
            componentsToAdd);
        gameObject.GetComponent<Rigidbody>().AddForce(gameObject.GetComponent<Transform>().forward * 750);
    }
}
