using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempLoadSave : MonoBehaviour
{
    
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.K))
        {
            SaveSystem.SavePlayer(gameObject.GetComponent<Player_Controller>());
        }
        if (Input.GetKeyDown(KeyCode.L))
        {
            PlayerData data = SaveSystem.LoadPlayer();
            Vector3 position;
            position.x = data.position[0];
            position.y = data.position[1];
            position.z = data.position[2];
            GetComponent<Player_Controller>().enabled = false;
            GetComponent<CharacterController>().enabled = false;

            transform.position = position;
            GetComponent<Player_Controller>().enabled = true;
            GetComponent<CharacterController>().enabled = true;
        }
    }
}
