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

            AddTurret(snapshot);
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
                UnityTurretConnector.WorkerType);
            template.SetComponentWriteAccess(EntityAcl.ComponentId, serverAttribute);

            snapshot.AddEntity(template);
        }

        private static void AddTurret(Snapshot snapshot)
        {
            var turretAttribute = UnityTurretConnector.WorkerType;
            
            var turretDamage = new Turret.TurretDamage.Snapshot(GameConstants.turretDamage);
            
            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(new Coordinates(6, 0.5, 0)), turretAttribute);
            template.AddComponent(new Metadata.Snapshot("Turret"), turretAttribute);
            template.AddComponent(new Persistence.Snapshot(), turretAttribute);
            template.AddComponent(turretDamage, turretAttribute);
            
            template.SetReadAccess(
                UnityClientConnector.WorkerType,
                UnityGameLogicConnector.WorkerType,
                MobileClientWorkerConnector.WorkerType,
                UnityTurretConnector.WorkerType);
            template.SetComponentWriteAccess(EntityAcl.ComponentId, turretAttribute);

            snapshot.AddEntity(template);
        }
    }
}
