
using UnityEngine;
using UnityEngine.AI;
using BehaviorDesigner.Runtime;
using Phil = BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime.Tasks.Movement;



[Phil.TaskDescription("Patrol within the specified area using the Unity NavMesh.")]
[Phil.TaskCategory("Mr. Crossy/Movement")]
[Phil.TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}PatrolIcon.png")]

public class PatrolWithinArea : NavMeshMovement
{
    [Phil.Tooltip("Centre of area to be patrolled")]
    public SharedVector3 patrolAreaCentre;
    [Phil.Tooltip("Radius of patrol area")]
    public SharedFloat patrolAreaRadius;
    [Phil.Tooltip("Margin of error to find NavMesh point")]
    public SharedFloat navErrorDistance;
    [Phil.Tooltip("What parts of the NavMesh can be considered a valid patrol location")]
    public SharedInt navMask;

    Vector3 destination = Vector3.zero;

    public override void OnStart()
    {
        base.OnStart();

        if (ValidatePointToNavmesh(GetPointInCircle(patrolAreaRadius.Value, patrolAreaCentre.Value), navErrorDistance.Value, navMeshAgent.areaMask)) SetDestination(destination);
    }

    public override Phil.TaskStatus OnUpdate()
    {
        if (destination == Vector3.zero)
        {
            if (ValidatePointToNavmesh(GetPointInCircle(patrolAreaRadius.Value, patrolAreaCentre.Value), navErrorDistance.Value, navMeshAgent.areaMask))
            {
                SetDestination(destination);
            }

        }
        else if (HasArrived())
        {
            Debug.Log("Boope");
            if (ValidatePointToNavmesh(GetPointInCircle(patrolAreaRadius.Value, patrolAreaCentre.Value), navErrorDistance.Value, navMeshAgent.areaMask))
            {
                SetDestination(destination);
            }
        }
        return Phil.TaskStatus.Running;
    }

    Vector3 GetPointInCircle(float radius, Vector3 centre) //Returns a random point in circle with given radius and centre.
    {
        Vector2 pointInCircle;
        pointInCircle = Random.insideUnitCircle * radius;

        Vector3 areaWithCentreOffset = new Vector3(pointInCircle.x + centre.x, centre.y, pointInCircle.y + centre.z);
        return areaWithCentreOffset;
    }

    bool ValidatePointToNavmesh(Vector3 samplePoint, float pointError, int areaMask) //Takes given point and translates it to the nearest allowed navmesh location, returns true if valid point found
    {
        
        for (int i = 0; i < 30; i++)
        {
            if (NavMesh.SamplePosition(samplePoint, out NavMeshHit hit, pointError, areaMask))
            {
                destination = hit.position;
                return true;
            }
        }

        destination = Vector3.zero; 
        return false;
    }

    public override void OnDrawGizmos()
    {
        Color colour = Color.green;
        colour.a = 0.01f;
        UnityEditor.Handles.color = colour;
        UnityEditor.Handles.DrawSolidDisc(patrolAreaCentre.Value, Vector3.up, patrolAreaRadius.Value);
    }
}
