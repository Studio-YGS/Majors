
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

    Vector3 destination;

    public override Phil.TaskStatus OnUpdate()
    {
        if (navMeshAgent.destination == null)
        {
            if (ValidatePointToNavmesh(GetPointInCircle(patrolAreaRadius.Value, patrolAreaCentre.Value), out destination, navErrorDistance.Value, navMask.Value)) navMeshAgent.destination = destination;
            else return Phil.TaskStatus.Failure;
        }
        else if (HasArrived())
        {
            if (ValidatePointToNavmesh(GetPointInCircle(patrolAreaRadius.Value, patrolAreaCentre.Value), out destination, navErrorDistance.Value, navMask.Value)) navMeshAgent.destination = destination;
            else return Phil.TaskStatus.Failure;
        }

        return Phil.TaskStatus.Running;
    }

    Vector3 GetPointInCircle(float radius, Vector3 centre) //Returns a random point in circle with given radius and centre.
    {
        Vector2 pointInCircle;
        pointInCircle = Random.insideUnitCircle * radius;

        Vector3 areaWithCentreOffset = new Vector3(pointInCircle.x + centre.x, 0, pointInCircle.y + centre.z);

        return areaWithCentreOffset;
    }

    bool ValidatePointToNavmesh(Vector3 samplePoint, out Vector3 resultingPoint, float pointError, int areaMask) //Takes given point and translates it to the nearest allowed navmesh location
    {
        NavMeshHit hit;

        if (NavMesh.SamplePosition(samplePoint, out hit, pointError, areaMask))
        {
            resultingPoint = hit.position;
            return true;
        }
        else { resultingPoint = Vector3.zero; return false; }
    }
}
