using System;
using Improbable.Gdk.Core;
using Improbable.Gdk.GameObjectCreation;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.TransformSynchronization;
using Improbable.Worker.CInterop;
using UnityEngine;

namespace BlankProject
{
    public class UnityClientConnector : WorkerConnector
    {
        public const string WorkerType = "UnityClient";

        [SerializeField] private GameObject level;

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
            Worker.World.GetOrCreateSystem<TankMovementSystem>();
            PlayerLifecycleHelper.AddClientSystems(Worker.World);
            
            var fallbackCreator = new GameObjectCreatorFromMetadata(Worker.WorkerType, Worker.Origin, Worker.LogDispatcher);
            var customCreator = new CustomGameObjectCreator(fallbackCreator, Worker.World, Worker.WorkerType, Worker.Origin, Worker.LogDispatcher);
            
            GameObjectCreationHelper.EnableStandardGameObjectCreation(Worker.World, customCreator);
            TransformSynchronizationHelper.AddClientSystems(Worker.World);

            if (level == null)
            {
                return;
            }
            levelInstance = Instantiate(level, transform.position, transform.rotation);
        }

        public override void  Dispose()
        {
            if (levelInstance != null)
            {
                Destroy(levelInstance);
            }
            base.Dispose();
        }
    }
}
