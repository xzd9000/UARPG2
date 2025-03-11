using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AttackSequenceFunctions
{
    public static class Default
    {
        public static void CommitAttack(AttackAction action, ICharacter character, Attack.Data[] data, Links[] linkedData)
        {
            if (action.activateProjectiles)
            {
                for (int i = 0; i < action.projInfo.Length; i++) ActivateProjectile(data, linkedData, action.projInfo[i]);
            }
            if (action.damageTarget) character.target.Damage(character.damage);
        }

        public static void ActivateProjectile(Attack.Data[] data, Links[] linkedData, ProjectileDataInfo projInfo)
        {
            Vector3 position = new Vector3(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);
            Quaternion rotation = new Quaternion(float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity, float.NegativeInfinity);

            HittingObject proj = GetDataItem<HittingObject>(projInfo.projFromLinks, projInfo.projDataIndex, data, linkedData);
            if (projInfo.usePosition) position = GetDataItem<Vector3>(projInfo.positionFromLinks, projInfo.positionDataIndex, data, linkedData);
            if (projInfo.useRotation)
            {
                if (projInfo.rotationFromLinks) rotation = (Quaternion)linkedData[projInfo.rotationDataIndex.x].linked[projInfo.rotationDataIndex.y];
                else rotation = Quaternion.Euler(data[projInfo.rotationDataIndex.x].vector3Value);
            }

            proj.Activate(position, rotation);
        }

        public static void DeactivateProjectiles(ProjectileDataInfo[] projs, Attack.Data[] data, Links[] linkedData)
        {
            for (int i = 0; i < projs.Length; i++) DeactivateProjectiles(projs[i], data, linkedData);
        }
        public static void DeactivateProjectiles(ProjectileDataInfo projInfo, Attack.Data[] data, Links[] linkedData)
        {
            HittingObject proj = GetDataItem<HittingObject>(projInfo.projFromLinks, projInfo.projDataIndex, data, linkedData);
            proj.Deactivate();
        }

        public static T GetDataItem<T>(bool fromLinks, Vector2Int index, Attack.Data[] data, Links[] linkedData)
        {
            return fromLinks ?
                (T)linkedData[index.x].linked[index.y] :
                (T)data[index.x].value;
        }
    }
    public static partial class Custom { }
}
