using System;
using Improbable.Gdk.Core;
////#6 - Adding the systems to the worker
//using Improbable.Gdk.GameObjectCreation;
using Improbable.Gdk.PlayerLifecycle;
////#10 - Adding the Transform Synchronization systems to the worker
//using Improbable.Gdk.TransformSynchronization;
using Improbable.Worker.CInterop;
using UnityEngine;

namespace BlankProject
{
    public class UnityClientConnector : WorkerConnector
    {
        public const string WorkerType = "UnityClient";
        
//        //#8 - GameObject prefab to be instantiated and variable to hold the instantiated level
//        [SerializeField] private GameObject level;
        private GameObject levelInstance;

        private async void Start()
        {
            var connParams = CreateConnectionParameters(WorkerType);
            connParams.Network.ConnectionType = NetworkConnectionType.Kcp;

            var builder = new SpatialOSConnectionHandlerBuilder()
                .SetConnectionParameters(connParams);

            if (!Application.isEditor)
            {
                var initializer = new CommandLineConnectionFlowInitializer();
                switch (initializer.GetConnectionService())
                {
                    case ConnectionService.Receptionist:
                        builder.SetConnectionFlow(new ReceptionistFlow(CreateNewWorkerId(WorkerType), initializer));
                        break;
                    case ConnectionService.Locator:
                        builder.SetConnectionFlow(new LocatorFlow(initializer));
                        break;
                    default:
                        throw new ArgumentOutOfRangeException();
                }
            }
            else
            {
                builder.SetConnectionFlow(new ReceptionistFlow(CreateNewWorkerId(WorkerType)));
            }

            await Connect(builder, new ForwardingDispatcher()).ConfigureAwait(false);
        }

        protected override void HandleWorkerConnectionEstablished()
        {
//            //#4 - Adding the Player Lifecycle system to the worker
//            PlayerLifecycleHelper.AddClientSystems(Worker.World);
            
            //#6 - Adding the systems to the worker
            //GameObjectCreationHelper.EnableStandardGameObjectCreation(Worker.World);
            
//            //#12 - Setting the custom GameObject creator
//            var fallbackCreator = new GameObjectCreatorFromMetadata(Worker.WorkerType, Worker.Origin, Worker.LogDispatcher);
//            var customCreator = new CustomGameObjectCreator(fallbackCreator, Worker.World, Worker.WorkerType, Worker.Origin);
//            GameObjectCreationHelper.EnableStandardGameObjectCreation(Worker.World, customCreator);
            
//            //#10 - Adding the Transform Synchronization systems to the worker
//            TransformSynchronizationHelper.AddClientSystems(Worker.World);

//            //#8 - Instantiating the level
//            if (level == null)
//            {
//                return;
//            }
//            levelInstance = Instantiate(level, transform.position, transform.rotation);
        }
        
//        //#8 - This method destroys the level instance once the worker is destroyed
//        public override void Dispose()
//        {
//            if (levelInstance != null)
//            {
//                Destroy(levelInstance);
//            }
//            base.Dispose();
//        }
    }
}
