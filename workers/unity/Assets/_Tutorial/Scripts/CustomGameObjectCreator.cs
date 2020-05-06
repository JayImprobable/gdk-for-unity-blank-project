using System;
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
    
    private readonly Type[] componentsToAdd =
    {
        typeof(Transform),
        typeof(Rigidbody),
        typeof(MeshRenderer)
    };
    
    public void PopulateEntityTypeExpectations(EntityTypeExpectations entityTypeExpectations)
    {
        entityTypeExpectations.RegisterEntityType("Player", new[]
        {
            typeof(Position.Component)
        });
        
        entityTypeExpectations.RegisterEntityType("Healer", new[]
        {
            typeof(Position.Component)
        });
        
        fallbackCreator.PopulateEntityTypeExpectations(entityTypeExpectations);
    }

    public CustomGameObjectCreator(IEntityGameObjectCreator fallbackCreator, World world, string workerType, Vector3 workerOrigin, ILogDispatcher logger)
    {
        this.fallbackCreator = fallbackCreator;
        this.world = world;
        this.workerType = workerType;
        this.workerOrigin = workerOrigin;
        this.logger = logger;
    }

    public void OnEntityCreated(string entityType, SpatialOSEntity entity, EntityGameObjectLinker linker)
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
            
            case "Healer":
                CreateHealer(entity, linker);
                break;

            default:
                fallbackCreator.OnEntityCreated(entity.GetComponent<Metadata.Component>().EntityType, entity, linker);
                break;
        }
    }

    private void CreateHealer(SpatialOSEntity entity, EntityGameObjectLinker linker)
    {
        if (!entity.TryGetComponent<Metadata.Component>(out var metadata) ||
            !entity.TryGetComponent<Position.Component>(out var spatialOSPosition))
        {
            return;
        }

        var prefabName = metadata.EntityType;
        var position = spatialOSPosition.Coords.ToUnityVector() + workerOrigin;

        var pathToPrefab = $"Prefabs/{workerType}/Healer";
        
        var prefab = Resources.Load<GameObject>(pathToPrefab);

        var rotation = prefab.GetComponent<Transform>().rotation;

        if (prefab == null)
        {
            return;
        }

        var gameObject = Object.Instantiate(prefab, position, rotation);
        gameObject.name = $"{prefab.name}(SpatialOS: {entity.SpatialOSEntityId}, Worker: {workerType})";

        linker.LinkGameObjectToSpatialOSEntity(entity.SpatialOSEntityId, gameObject);
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

        linker.LinkGameObjectToSpatialOSEntity(entity.SpatialOSEntityId, (GameObject) playerGameObject,
            componentsToAdd);
    }
    
    

    public void OnEntityRemoved(EntityId entityId)
    {
        fallbackCreator.OnEntityRemoved(entityId);
    }

}
