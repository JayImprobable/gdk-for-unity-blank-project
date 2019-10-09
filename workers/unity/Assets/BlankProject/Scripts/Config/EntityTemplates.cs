using System.Collections.Generic;
using UnityEngine;
using Improbable;
using Improbable.Gdk.Core;
using Improbable.Gdk.PlayerLifecycle;
//#10 - Adding the Transform Synchronization systems to the worker
using Improbable.Gdk.TransformSynchronization;
//#13 - Adding the TurretRotation component to the Player Template
using Tank;

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
            Vector3 position = spawnPoints[Random.Range(0, spawnPoints.Count)];
            
            //#13 - Adding the TurretRotation component to the Player Template
            var turretRotationComponent = new TurretRotation.Snapshot();
            
            //#15 - Adding the TankColor component to the Player Template
            var colorComponent = new TankColor.Snapshot();
            
            //#16 - Adding the Health and Weapons component
            var healthComponent = new Health.Snapshot(GameConstants.MaxHealth);
            var weaponsComponent = new Weapons.Snapshot(GameConstants.MachineGunDamage);
            
            //#21 - Adding the weapons effect components
            var weaponsFxComponent = new WeaponsFx.Snapshot();

            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(position.ToCoordinates()), clientAttribute);
            template.AddComponent(new Metadata.Snapshot("Player"), serverAttribute);
            
            //#13 - Adding the TurretRotation component to the Player Template
            template.AddComponent(turretRotationComponent, clientAttribute);
            
            //#15 - Adding the TankColor component to the Player Template
            template.AddComponent(colorComponent, clientAttribute);
            
            //#16 - Adding the Health and Weapons component
            template.AddComponent(healthComponent, serverAttribute);
            template.AddComponent(weaponsComponent, serverAttribute);
            
            //#21 - Adding the weapons effect components
            template.AddComponent(weaponsFxComponent, clientAttribute);

            PlayerLifecycleHelper.AddPlayerLifecycleComponents(template, workerId, serverAttribute);
            //#10 - Adding the Transform Synchronization systems to the worker
            TransformSynchronizationHelper.AddTransformSynchronizationComponents(template, clientAttribute, position);

            template.SetReadAccess(UnityClientConnector.WorkerType, MobileClientWorkerConnector.WorkerType, serverAttribute);
            template.SetComponentWriteAccess(EntityAcl.ComponentId, serverAttribute);

            return template;
        }
    }
}
