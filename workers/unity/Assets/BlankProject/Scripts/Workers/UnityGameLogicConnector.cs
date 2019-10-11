using BlankProject.Scripts.Config;
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
    public class UnityGameLogicConnector : WorkerConnector
    {
        public const string WorkerType = "UnityGameLogic";
        
//        //#8 - GameObject prefab to be instantiated and variable to hold the instantiated level
//        [SerializeField] private GameObject level;
        private GameObject levelInstance;

        private async void Start()
        {
//            //#4 - Setting the player entity template
//            PlayerLifecycleConfig.CreatePlayerEntityTemplate = EntityTemplates.CreatePlayerEntityTemplate;

            IConnectionFlow flow;
            ConnectionParameters connectionParameters;

            if (Application.isEditor)
            {
                flow = new ReceptionistFlow(CreateNewWorkerId(WorkerType));
                connectionParameters = CreateConnectionParameters(WorkerType);
            }
            else
            {
                flow = new ReceptionistFlow(CreateNewWorkerId(WorkerType),
                    new CommandLineConnectionFlowInitializer());
                connectionParameters = CreateConnectionParameters(WorkerType,
                    new CommandLineConnectionParameterInitializer());
            }

            var builder = new SpatialOSConnectionHandlerBuilder()
                .SetConnectionFlow(flow)
                .SetConnectionParameters(connectionParameters);

            await Connect(builder, new ForwardingDispatcher()).ConfigureAwait(false);
        }

        protected override void HandleWorkerConnectionEstablished()
        {
            Worker.World.GetOrCreateSystem<MetricSendSystem>();
            
//            //#4 - Adding the Player Lifecycle system to the worker
//            PlayerLifecycleHelper.AddServerSystems(Worker.World);
            
//            //#6 - Adding the systems to the worker
//            GameObjectCreationHelper.EnableStandardGameObjectCreation(Worker.World);
            
//            //#10 - Adding the Transform Synchronization systems to the worker
//            TransformSynchronizationHelper.AddServerSystems(Worker.World);
            
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
