using System.IO;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.TransformSynchronization;
using UnityEditor;
using UnityEngine;
using Snapshot = Improbable.Gdk.Core.Snapshot;

namespace BlankProject.Editor
{
    internal static class SnapshotGenerator
    {
        private static string DefaultSnapshotPath = Path.GetFullPath(
            Path.Combine(
                Application.dataPath,
                "..",
                "..",
                "..",
                "snapshots",
                "default.snapshot"));

        [MenuItem("SpatialOS/Generate snapshot")]
        public static void Generate()
        {
            Debug.Log("Generating snapshot.");
            var snapshot = CreateSnapshot();

            Debug.Log($"Writing snapshot to: {DefaultSnapshotPath}");
            snapshot.WriteToFile(DefaultSnapshotPath);
        }

        private static Snapshot CreateSnapshot()
        {
            var snapshot = new Snapshot();

            AddHealer(snapshot);
            AddPlayerSpawner(snapshot);
            return snapshot;
        }

        private static void AddPlayerSpawner(Snapshot snapshot)
        {
            var serverAttribute = UnityGameLogicConnector.WorkerType;

            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(), serverAttribute);
            template.AddComponent(new Metadata.Snapshot("PlayerCreator"), serverAttribute);
            template.AddComponent(new Persistence.Snapshot(), serverAttribute);
            template.AddComponent(new PlayerCreator.Snapshot(), serverAttribute);

            template.SetReadAccess(
                UnityClientConnector.WorkerType,
                UnityGameLogicConnector.WorkerType,
                MobileClientWorkerConnector.WorkerType,
                UnityHealerConnector.WorkerType);
            template.SetComponentWriteAccess(EntityAcl.ComponentId, serverAttribute);

            snapshot.AddEntity(template);
        }

        private static void AddHealer(Snapshot snapshot)
        {
            var healerAttribute = UnityHealerConnector.WorkerType;
            
            var healer = new Healer.HealValue.Snapshot(GameConstants.HealerValue);
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(new Coordinates(6, 2, 0)), healerAttribute);
            template.AddComponent(new Metadata.Snapshot("Healer"), healerAttribute);
            template.AddComponent(new Persistence.Snapshot(), healerAttribute);
            template.AddComponent(healer, healerAttribute);
            
            template.SetReadAccess(
                UnityClientConnector.WorkerType,
                UnityGameLogicConnector.WorkerType,
                MobileClientWorkerConnector.WorkerType,
                UnityHealerConnector.WorkerType);
            template.SetComponentWriteAccess(EntityAcl.ComponentId, healerAttribute);

            snapshot.AddEntity(template);
        }
    }
}
