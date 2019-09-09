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

public class PlayerGameObjectCreator : IEntityGameObjectCreator
{
    private readonly IEntityGameObjectCreator fallbackCreator;
    private readonly World world;
    private readonly string workerType;

    public PlayerGameObjectCreator(IEntityGameObjectCreator fallbackCreator, World world, string workerType)
    {
        this.fallbackCreator = fallbackCreator;
        this.world = world;
        this.workerType = workerType;
    }

    public void OnEntityCreated(SpatialOSEntity entity, EntityGameObjectLinker linker)
    {
        if (!entity.HasComponent<Metadata.Component>())
        {
            return;
        }

        if (entity.GetComponent<Metadata.Component>().EntityType != "Player")
        {
            fallbackCreator.OnEntityCreated(entity, linker);
            return;
        }

        var metadata = entity.GetComponent<Metadata.Component>();
        bool isPlayer = metadata.EntityType == "Player";

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

        var prefab = Resources.Load(pathToPrefab);
        var playerGameObject = UnityEngine.Object.Instantiate(prefab);
        playerGameObject.name = $"{metadata.EntityType}(SpatialOS: {entity.SpatialOSEntityId}, Worker: {workerType}";

        Type[] componentsToAdd =
        {
            typeof(Transform),
            typeof(Rigidbody),
            typeof(MeshRenderer)
        };
        linker.LinkGameObjectToSpatialOSEntity(entity.SpatialOSEntityId, (GameObject) playerGameObject,
            componentsToAdd);
    }
    
    public void OnEntityRemoved(EntityId entityId)
    {
        fallbackCreator.OnEntityRemoved(entityId);
    }
}
