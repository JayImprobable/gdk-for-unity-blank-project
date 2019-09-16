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
            var clientAttribute = EntityTemplate.GetWorkerAccessAttribute(workerId);
            var serverAttribute = UnityGameLogicConnector.WorkerType;
            Vector3 position = new Vector3(0, 0, 0);

            var turretRotationComponent = new TurretRotation.Snapshot();
            var colorComponent = new TankColor.Snapshot();
            var healthComponent = new Health.Snapshot(GameConstants.MaxHealth);
            var weaponsComponent = new Weapons.Snapshot(GameConstants.MachineGunDamage, GameConstants.CannonDamage);

            var template = new EntityTemplate();
            template.AddComponent(new Position.Snapshot(position.ToCoordinates()), clientAttribute);
            template.AddComponent(new Metadata.Snapshot("Player"), serverAttribute);
            template.AddComponent(turretRotationComponent, clientAttribute);
            template.AddComponent(colorComponent, clientAttribute);
            template.AddComponent(healthComponent, serverAttribute);
            template.AddComponent(weaponsComponent, serverAttribute);

            PlayerLifecycleHelper.AddPlayerLifecycleComponents(template, workerId, serverAttribute);
            TransformSynchronizationHelper.AddTransformSynchronizationComponents(template, clientAttribute, position);

            template.SetReadAccess(UnityClientConnector.WorkerType, MobileClientWorkerConnector.WorkerType, serverAttribute);
            template.SetComponentWriteAccess(EntityAcl.ComponentId, serverAttribute);

            return template;
        }
    }
}
