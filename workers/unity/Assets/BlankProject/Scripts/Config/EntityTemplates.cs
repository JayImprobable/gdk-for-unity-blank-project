using System.Collections.Generic;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
using Improbable.Gdk.TransformSynchronization;
using Tank;
using UnityEngine;
using UnityEngine.Animations;

namespace BlankProject.Scripts.Config
{
    public static class EntityTemplates
    {
        public static EntityTemplate CreatePlayerEntityTemplate(string workerId, byte[] serializedArguments)
        {
            IList<Vector3> spawnPoints = new List<Vector3>();
            spawnPoints.Add(new Vector3(-35, 0, 20));
            spawnPoints.Add(new Vector3(-37, 0, -35));
            spawnPoints.Add(new Vector3(39, 0, 33));
            spawnPoints.Add(new Vector3(23, 0, -35));
            var clientAttribute = EntityTemplate.GetWorkerAccessAttribute(workerId);
            var serverAttribute = UnityGameLogicConnector.WorkerType;
            Vector3 position = spawnPoints[Random.Range(0, 3)];

            var turretRotationComponent = new TurretRotation.Snapshot();
            var colorComponent = new TankColor.Snapshot();
            var healthComponent = new Health.Snapshot(GameConstants.MaxHealth);
            var weaponsComponent = new Weapons.Snapshot(GameConstants.MachineGunDamage, GameConstants.CannonDamage);
            var weaponsFxComponent = new WeaponsFx.Snapshot();
            var fireCannon = new FireCannonball.Snapshot();

            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(position.ToCoordinates()), clientAttribute);
            template.AddComponent(new Metadata.Snapshot("Player"), serverAttribute);
            template.AddComponent(turretRotationComponent, clientAttribute);
            template.AddComponent(colorComponent, clientAttribute);
            template.AddComponent(healthComponent, serverAttribute);
            template.AddComponent(weaponsComponent, serverAttribute);
            template.AddComponent(weaponsFxComponent, clientAttribute);
            template.AddComponent(fireCannon, clientAttribute);

            PlayerLifecycleHelper.AddPlayerLifecycleComponents(template, workerId, serverAttribute);
            TransformSynchronizationHelper.AddTransformSynchronizationComponents(template, clientAttribute, position);

            template.SetReadAccess(UnityClientConnector.WorkerType, MobileClientWorkerConnector.WorkerType, serverAttribute, UnityTurretConnector.WorkerType);
            template.SetComponentWriteAccess(EntityAcl.ComponentId, serverAttribute);

            return template;
        }

        public static EntityTemplate CreateCannonballEntityTemplate(string workerId, Vector3 position, Vector3 rotation)
        {
            var clientAttribute = EntityTemplate.GetWorkerAccessAttribute(workerId);
            var serverAttribute = UnityGameLogicConnector.WorkerType;
            
            var startRotation = new Rotation.Snapshot(new CannonballRotation(rotation.x, rotation.y, rotation.z));

            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(position.ToCoordinates()), serverAttribute);
            template.AddComponent(new Metadata.Snapshot("Cannonball"), serverAttribute);
            template.AddComponent(startRotation, serverAttribute);
            
            TransformSynchronizationHelper.AddTransformSynchronizationComponents(template, serverAttribute, position);
            template.SetReadAccess(UnityClientConnector.WorkerType, MobileClientWorkerConnector.WorkerType, serverAttribute);
>>>>>>> cannonball
            template.SetComponentWriteAccess(EntityAcl.ComponentId, serverAttribute);

            return template;
        }
    }
}
