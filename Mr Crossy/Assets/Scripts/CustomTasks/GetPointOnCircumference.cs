
using UnityEngine;
using UnityEngine.AI;
using Gemstone;
using BehaviorDesigner.Runtime;
using Phil = BehaviorDesigner.Runtime.Tasks;


[Phil.TaskDescription("Gets a point on a circumference, and validates point on navmesh")]
[Phil.TaskCategory("Mr. Crossy/Movement")]
[Phil.TaskIcon("Assets/Behavior Designer Movement/Editor/Icons/{SkinColor}PatrolIcon.png")]


public class GetPointOnCircumference : Phil.Action
{
    public SharedBool isSpawned;

    [Phil.Tooltip("Centre of area to be patrolled")]
    public SharedVector3 circleCentre;
    [Phil.Tooltip("Radius of patrol area")]
    public SharedFloat circleRadius;
    [Phil.Tooltip("Margin of error to find NavMesh point")]
    public SharedFloat navErrorDistance;
    [Phil.Tooltip("What parts of the NavMesh can be considered a valid patrol location")]
    public SharedInt navMask;

    public SharedVector3 returnedPoint;

    public override Phil.TaskStatus OnUpdate()
    {
        Vector3 spawnPoint;

        if (!isSpawned.Value)
        {
            if (ValidatePointToNavmesh(PointOnCircumference(circleCentre.Value, circleRadius.Value, Random.Range(0f, 360f)), out spawnPoint, navErrorDistance.Value, navMask.Value)) 
            {
                returnedPoint.Value = spawnPoint;
                return Phil.TaskStatus.Success; //Returns success when a valid point found
            }
            else return Phil.TaskStatus.Running; //Returns running when a valid point not found
        }
        else return Phil.TaskStatus.Failure; //Returns failure if run when Mr Crossy is on the field
    }

    public Vector3 PointOnCircumference(Vector3 circleCentre, float circleRadius, float angle)
    {
        float radians = angle.ToRadians();

        float x = (Mathf.Cos(radians) * circleRadius) + circleCentre.x;
        float y = (Mathf.Cos(radians) * circleRadius) + circleCentre.z;

        Vector3 resultPoint = new Vector3(x,0,y);

        return resultPoint;
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
