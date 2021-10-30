using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using BehaviorDesigner.Runtime;

public static class TreeMalarkey
{
    public static void EnableTree(BehaviorTree tree)
    {
        tree.EnableBehavior();
        Debug.Log("Enabled tree: " + tree);
    }

    public static void DisableTree(BehaviorTree tree)
    {
        tree.DisableBehavior();
        Debug.Log("Disabled tree: " + tree);
    }

    public static void DisableTree(BehaviorTree tree, bool pauseTree)
    {
        tree.DisableBehavior(pauseTree);
    }

    public static void SendEventToTree(BehaviorTree tree, string eventName)
    {
        tree.SendEvent(eventName);
    }
    public static void SendEventToTree(BehaviorTree tree, string eventName, Object arg1)
    {
        tree.SendEvent(eventName, arg1);
    }
    public static void SendEventToTree(BehaviorTree tree, string eventName, Object arg1, Object arg2)
    {
        tree.SendEvent(eventName, arg1, arg2);
    }
    public static void SendEventToTree(BehaviorTree tree, string eventName, Object arg1, Object arg2, Object arg3)
    {
        tree.SendEvent(eventName, arg1, arg2, arg3);
    }

    public static void RegisterEventOnTree(BehaviorTree tree, string eventName, System.Action action)
    {
        tree.RegisterEvent(eventName, action);
    }

    public static void UnregisterEventOnTree(BehaviorTree tree, string eventName, System.Action action)
    {
        tree.UnregisterEvent(eventName, action);
    }
}
