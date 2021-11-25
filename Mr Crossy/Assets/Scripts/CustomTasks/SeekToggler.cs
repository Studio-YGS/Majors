using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;
using Phil = BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;

[Phil.TaskDescription("Seek the target specified using the Unity NavMesh. Can switch seek target based on boolean")]
[Phil.TaskCategory("Movement")]
[Phil.TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}SeekIcon.png")]
public class SeekToggler : NavMeshMovement
{
    public bool gameObjectIsTrue;
    [Phil.Tooltip("The seek toggler.")]
    public SharedBool toggleBool;
    [Phil.Tooltip("The GameObject that the agent is seeking")]
    public SharedGameObject target;
    [Phil.Tooltip("If target is null then use the target position")]
    public SharedVector3 targetPosition;

    public override void OnStart()
    {
        base.OnStart();

        SetDestination(Target());
    }

    // Seek the destination. Return success once the agent has reached the destination.
    // Return running if the agent hasn't reached the destination yet
    public override Phil.TaskStatus OnUpdate()
    {
        if (HasArrived())
        {
            return Phil.TaskStatus.Success;
        }

        SetDestination(Target());

        return Phil.TaskStatus.Running;
    }

    // Return targetPosition if target is null
    private Vector3 Target()
    {
        if (target.Value != null)
        {
            return target.Value.transform.position;
        }
        else if (gameObjectIsTrue)
        {
            return (toggleBool.Value) ? target.Value.transform.position : targetPosition.Value;
        }
        else return (toggleBool.Value) ? targetPosition.Value : target.Value.transform.position;
    }

    public override void OnReset()
    {
        base.OnReset();
        target = null;
        targetPosition = Vector3.zero;
    }
}
