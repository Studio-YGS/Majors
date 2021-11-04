using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DoorLock : MonoBehaviour
{
    DoorInteraction[] doors;

    public void LockDoors()
    {
        doors = FindObjectsOfType<DoorInteraction>();
        foreach(DoorInteraction door in doors)
        {
            if (!door.isSafeHouse)
            {
                door.locked = true;
            }
        }
    }

    public void UnLockDoors()
    {
        doors = FindObjectsOfType<DoorInteraction>();
        foreach (DoorInteraction door in doors)
        {
            door.locked = false;
        }
    }
}
