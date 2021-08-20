using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime.Tasks;
using BehaviorDesigner.Runtime;

[TaskCategory("Unity/Transform")]
[TaskDescription("Stores the position of the Transform. Returns Success.")]
public class GetPositionFromGameObjectList : Action
{

    [BehaviorDesigner.Runtime.Tasks.Tooltip("The GameObjectList that the task operates on.")]
    public SharedGameObjectList targetGameObjectList;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("Picks a random object from list to get position.")]
    public bool getRandomGameObject;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("Index to specify GameObject to use. Is ignored if getting random.")]
    public int getGameObjectByIndex;
    [BehaviorDesigner.Runtime.Tasks.Tooltip("Can the target GameObject be empty?")]
    [RequiredField]
    public SharedVector3 storeValue;

    private Transform targetTransform;
    private GameObject prevGameObject;

    public override void OnStart()
    {
        int i;
        List<GameObject> list = targetGameObjectList.Value;

        if (getRandomGameObject) i = Random.Range(0, list.Count);
        else i = getGameObjectByIndex;

        GameObject currentGameObject = GetDefaultGameObject(list[i]);
        if (currentGameObject != prevGameObject)
        {
            targetTransform = currentGameObject.GetComponent<Transform>();
            prevGameObject = currentGameObject;
        }
    }

    public override TaskStatus OnUpdate()
    {
        if (targetTransform == null)
        {
            Debug.LogWarning("Transform is null");
            return TaskStatus.Failure;
        }

        storeValue.Value = targetTransform.position;

        return TaskStatus.Success;
    }

    public override void OnReset()
    {
        targetGameObjectList = null;
        storeValue = Vector3.zero;
    }
}
