using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class AIFunctions
{
    public static class Default
    {
        /// <returns>Returns true if target was reached before movement, false otherwise</returns>
        public static bool MoveToTarget(AI ai, ICharacter owner, Vector3 target, Vector3 movement, Space space, bool _3dMovement = false, bool _3dRotation = false)
        {
            bool reached = false;
            if (Vector3.Distance(owner.transform.position, target) > ai.targetReachDistance)
                owner.movement.MoveTo(owner.transform, target, movement, space, _3dMovement, _3dRotation);
            else reached = true;
            return reached;
        }
        public static bool RotateToTarget(AI ai, ICharacter owner, Vector3 target, float rotationSpeed, float targetingAngle, bool direction = false, bool includeY = false)
        {
            bool reached = false;
            if (ai.transform.AngleToTarget(target, includeY, direction) > ai.targetingAngle)
                owner.movement.RotateToTarget(owner.transform ,target, rotationSpeed, targetingAngle, direction, includeY);
            else reached = true;
            return reached;
        }

        public static Attack RandomAttack(ICharacter owner) => RandomAttack(owner.attacks);
        public static Attack RandomAttack(IList<Attack> attacks) => attacks[UnityEngine.Random.Range(0, attacks.Count)];
        
        public static bool CheckAttack(Attack attack, ICharacter attacker, ICharacter target)
        {
            if (!attack.useDetectors && !attack.useAreas) return true;
            else
            {
                int length1;
                if (!attack.useAreas) length1 = attack.detectorsIndexes.Length;
                else length1 = attack.areas.Length;
                bool ret = false;
                for (int i = 0; i < length1; i++)
                {
                    if (attack.useDetectors)
                    {
                        CollisionTracker tracker;
                        if (attack.detectorsIndexes[i].x >= 0) tracker = (CollisionTracker)attack.linkedData[attack.detectorsIndexes[i].x].linked[attack.detectorsIndexes[i].y];
                        else tracker = (CollisionTracker)attack.sequenceData[attack.detectorsIndexes[i].y].objectValue;

                        if (tracker.FindIndex((c) => c.gameObject == target.gameObject) >= 0) return true;
                    }
                    if (attack.useAreas)
                    {
                        Attack.Area area = attack.areas[i];

                        if (area.type == Attack.Area.Type.circle || area.type == Attack.Area.Type.cylinder)
                        {
                            Vector2 attacker2 = new Vector2(attacker.transform.position.x + area.offset.x, attacker.transform.position.z + area.offset.z);
                            Vector2 target2 = new Vector2(target.transform.position.x, target.transform.position.z);
                            ret = Vector2.Distance(attacker2, target2) <= area.radius;
                            if (area.type == Attack.Area.Type.cylinder && ret == true)
                            {
                                float h = target.transform.position.y - (attacker.transform.position.y + area.offset.y);
                                ret = h <= area.height && h >= 0;
                            }
                        }
                        else if (area.type == Attack.Area.Type.sphere) ret = Vector3.Distance(attacker.transform.position + area.offset, target.transform.position) <= area.radius;
                        else if (area.type == Attack.Area.Type.box)
                        {
                            Vector3 position = attacker.transform.InverseTransformPoint(target.transform.position);
                            ret =
                                (
                                    position.x <= area.halfDimensions.x + area.offset.x &&
                                    position.x >= -area.halfDimensions.x + area.offset.x &&

                                    position.y <= area.halfDimensions.y + area.offset.y &&
                                    position.y >= -area.halfDimensions.y + area.offset.y &&

                                    position.z <= area.halfDimensions.z + area.offset.z &&
                                    position.z >= -area.halfDimensions.z + area.offset.z
                                );
                        }
                   
                        if (area.angle > 0) ret &= attacker.transform.AngleToTarget(target.transform.position, true) <= area.angle;
                        if (ret == true) return ret;
                    }
                }
                return false;
            }
        }

        public static AI.StateActions[] FillStatesActionOnly(params (int, Action<AIController, ICharacter>)[] actions)
        {
            AI.StateActions[] states = new AI.StateActions[actions.Length];
            FillStatesActionOnly(ref states, actions);
            return states;
        }
        public static void FillStatesActionOnly(ref AI.StateActions[] states, params (int, Action<AIController, ICharacter>)[] actions)
        {
            for (int i = 0; i < actions.Length; i++)           
                states[actions[i].Item1] = new AI.StateActions
                    (
                        stateEnter: (_, _) => { },
                        stateAction: (ctl, owner) => actions[i].Item2(ctl, owner),
                        stateExit: (_, _) => { }
                    );
            
        }
    }
    public static partial class Custom { }
}
