
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
    public SharedVector3 validationVector;

    [Phil.Tooltip("Centre of area to be patrolled")]
    public SharedVector3 circleCentre;
    [Phil.Tooltip("Radius of patrol area")]
    public SharedFloat circleAreaRadius;
    [Phil.Tooltip("Margin of error to find NavMesh point")]
    public SharedFloat navErrorDistance;
    [Phil.Tooltip("What parts of the NavMesh can be considered a valid patrol location")]
    public SharedInt navMask;

    public SharedVector3 returnedPoint;
    private Vector3 returnedPos;
    private Vector3 samplePosition;

    public override Phil.TaskStatus OnUpdate()
    {
        if (ValidatePointToNavmesh(circleCentre.Value, circleAreaRadius.Value, navErrorDistance.Value, navMask.Value)) 
        {
            returnedPoint.Value = returnedPos;
            return Phil.TaskStatus.Success; //Returns success when a valid point found
        }
        else return Phil.TaskStatus.Running; //Returns running when a valid point not found
        
        
    }

    public Vector3 PointOnCircumference(Vector3 circleCentre, float circleRadius, float radians)
    {
        float x = (Mathf.Cos(radians) * circleRadius) + circleCentre.x;
        float y = (Mathf.Sin(radians) * circleRadius) + circleCentre.z;

        Vector3 resultPoint = new Vector3(x, circleCentre.y, y);
        Debug.Log("GETCIRCUMPOINT: " + "Sampled Point: " + resultPoint);
        return resultPoint;
    }

    bool ValidatePointToNavmesh(Vector3 circleCentre, float circleRadius, float pointError, int areaMask) //Takes given point and translates it to the nearest allowed navmesh location, returns true if valid point found
    {
        Debug.Log("GETCIRCUMPOINT: " + "validate attempt");
        Vector3 samplePoint = PointOnCircumference(circleCentre, circleRadius, Random.Range(0f, 360f).ToRadians());


        for (int i = 0; i < 30; i++)
        {
            if (NavMesh.SamplePosition(samplePoint, out NavMeshHit hit, pointError, areaMask))
            {
                NavMeshPath pathTest = new NavMeshPath();
                Debug.Log("GETCIRCUMPOINT: " + "begin testingPath");
                NavMesh.CalculatePath(hit.position, validationVector.Value, areaMask, pathTest);
                samplePosition = hit.position;
                if (pathTest.status == NavMeshPathStatus.PathComplete)
                {
                    if (Emerald.GetPathLength(pathTest) <= circleAreaRadius.Value)
                    {
                        Debug.Log("GETCIRCUMPOINT: " + "SUCCESS");
                        returnedPos = hit.position;
                        return true;
                    }
                }
                else samplePoint = PointOnCircumference(circleCentre, circleRadius, Random.Range(0f, 360f).ToRadians());
            }
        }
        Debug.Log("GETCIRCUMPOINT: " + "FAILURE");
        returnedPos = Vector3.zero;
        return false;
    }

    public override void OnDrawGizmos()
    {
#if UNITY_EDITOR
        Color colourOne = Color.red;
        Color colourTwo = Color.cyan;

        Gizmos.color = colourOne;
        Gizmos.DrawSphere(samplePosition, .1f);

        Gizmos.color = colourTwo;
        Gizmos.DrawSphere(validationVector.Value, .1f);

        Color colour = Color.gray;
        colour.a = 0.01f;
        UnityEditor.Handles.color = colour;
        UnityEditor.Handles.DrawSolidDisc(circleCentre.Value, Vector3.up, circleAreaRadius.Value);
#endif
    }
}
