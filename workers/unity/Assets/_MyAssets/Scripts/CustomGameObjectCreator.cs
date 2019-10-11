//using System;
//using Improbable;
//using Improbable.Gdk.Core;
//using Improbable.Gdk.GameObjectCreation;
//using Improbable.Gdk.PlayerLifecycle;
//using Improbable.Gdk.Subscriptions;
//using Unity.Entities;
//using UnityEngine;
//using Object = UnityEngine.Object;
//
//public class CustomGameObjectCreator : IEntityGameObjectCreator
//{
//    //fallbackCreator is the creator to be used in case the GameObject to be created is not handled by the custom creator
//    //In our case the fallbackCreator will be the GameObjectCreatorFromMetadata class
//    private readonly IEntityGameObjectCreator fallbackCreator;
//    
//    //Your game world
//    private readonly World world;
//    
//    //The worker type that will use this custom creator
//    private readonly string workerType;
//    
//    //Origin position of the worker, used mostly for the Editor offset of worker connectors
//    private readonly Vector3 workerOrigin;
//    
//    //List of Unity Components to be added as entity components
//    private readonly Type[] componentsToAdd =
//    {
//        typeof(Transform),
//        typeof(Rigidbody),
//        typeof(MeshRenderer)
//    };
//
//    //Constructor, in it we just set the class variables
//    public CustomGameObjectCreator(IEntityGameObjectCreator fallbackCreator, World world, string workerType, Vector3 workerOrigin)
//    {
//        this.fallbackCreator = fallbackCreator;
//        this.world = world;
//        this.workerType = workerType;
//        this.workerOrigin = workerOrigin;
//    }
//
//    //This is the callback that is called once the entity is created
//    public void OnEntityCreated(SpatialOSEntity entity, EntityGameObjectLinker linker)
//    {
//        //If the entity have no Metadata component I return, because there's no GameObject to be created
//        if (!entity.HasComponent<Metadata.Component>())
//        {
//            return;
//        }
//
//        //I call the appropriate function depending on the Entity type, otherwise I use the fallbackCreator's
//        //OnEntityCreated function
//        switch (entity.GetComponent<Metadata.Component>().EntityType)
//        {
//            case "Player":
//                CreatePlayer(entity, linker);
//                break;
//            
//            case "Healer":
//                CreateHealer(entity, linker);
//                break;
//
//            default:
//                fallbackCreator.OnEntityCreated(entity, linker);
//                break;
//        }
//    }
//
//    //Creates the Healer GameObjects
//    private void CreateHealer(SpatialOSEntity entity, EntityGameObjectLinker linker)
//    {
//        //In order to create the GameObject *and* link it to an Entity, we need these two components: Metadata and
//        //Position. If they're not present, we stop the function
//        if (!entity.TryGetComponent<Metadata.Component>(out var metadata) ||
//            !entity.TryGetComponent<Position.Component>(out var spatialOSPosition))
//        {
//            return;
//        }
//
//        //The Metadata Component contains the name of the prefab to be loaded
//        var prefabName = metadata.EntityType;
//        //The Position Component contains the Entity position. We're also adding the workerOrigin value to it to
//        //compensate for the Editor offset
//        var position = spatialOSPosition.Coords.ToUnityVector() + workerOrigin;
//
//        //Path where the Prefab is stored
//        var pathToPrefab = $"Prefabs/{workerType}/Healer";
//        
//        //Loading the prefab
//        var prefab = Resources.Load<GameObject>(pathToPrefab);
//
//        //Since my 3D model is rotated on the prefab, I need to use this rotation when instantiating the GameObject
//        var rotation = prefab.GetComponent<Transform>().rotation;
//
//        //If the prefab is not present, I don't instantiate it
//        if (prefab == null)
//        {
//            return;
//        }
//
//        //Finally instantiating the GameObject...
//        var gameObject = Object.Instantiate(prefab, position, rotation);
//        //... And renaming it using the Entity ID and worker to which this GameObject is associated
//        gameObject.name = $"{prefab.name}(SpatialOS: {entity.SpatialOSEntityId}, Worker: {workerType})";
//
//        //Lastly, I link this GameObject to its Entity via the Entity ID
//        linker.LinkGameObjectToSpatialOSEntity(entity.SpatialOSEntityId, gameObject);
//    }
//    
//    //Creates the Player GameObject
//    private void CreatePlayer(SpatialOSEntity entity, EntityGameObjectLinker linker)
//    {
//        //In order to create the GameObject *and* link it to an Entity, we need these two components: Metadata and
//        //Position. If they're not present, we stop the function
//        if (!entity.TryGetComponent<Metadata.Component>(out var metadata) ||
//            !entity.TryGetComponent<Position.Component>(out var spatialOSPosition))
//        {
//            return;
//        }
//
//        //Here I check if I "own" the player I am instantiating. In the EntityTemplates class, when I add the 
//        //PlayerLifecycleHelper, one of the parameters is the workerId that is creating that entity. So when player A
//        //is instantiating Player B's GameObject, this will return false
//        bool hasAuthority = PlayerLifecycleHelper.IsOwningWorker(entity.SpatialOSEntityId, world);
//        string pathToPrefab;
//        if (hasAuthority)
//        {
//            //If I have authority over this GameObject (i.e.: It is my player's GameObject), I load the Player GameObject
//            //under the Authoritative folder...
//            pathToPrefab = $"Prefabs/{workerType}/Authoritative/Player";
//        }
//        else
//        {
//            //...If I don't, I load the GameObject from the Non-Authoritative folder
//            pathToPrefab = $"Prefabs/{workerType}/Non-Authoritative/Player";
//        }
//
//        //Loading the prefab
//        var prefab = Resources.Load<GameObject>(pathToPrefab);
//        //The Position Component contains the Entity position. We're also adding the workerOrigin value to it to
//        //compensate for the Editor offset
//        var position = spatialOSPosition.Coords.ToUnityVector() + workerOrigin;
//        
//        //Finally instantiating the GameObject...
//        var playerGameObject = Object.Instantiate(prefab, position, Quaternion.identity);
//        //... And renaming it using the Entity ID and worker to which this GameObject is associated
//        playerGameObject.name = $"{metadata.EntityType}(SpatialOS: {entity.SpatialOSEntityId}, Worker: {workerType})";
//
//        //Lastly, I link this GameObject to its Entity via the Entity ID
//        linker.LinkGameObjectToSpatialOSEntity(entity.SpatialOSEntityId, (GameObject) playerGameObject,
//            componentsToAdd);
//    }
//    
//    
//
//    public void OnEntityRemoved(EntityId entityId)
//    {
//        fallbackCreator.OnEntityRemoved(entityId);
//    }
//
//}
