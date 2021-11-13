using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FootstepDetector : MonoBehaviour
{
    Transform player;
    CharacterController character;
    public Collider playerCollider;
    Terrain terrain;
    int posX;
    int posZ;
    public float[] textureValues;
    bool isGrounded;
    bool isOnTerrain;
    bool walking;
    float currentSpeed;
    float distanceCoverd;
    RaycastHit hit;
    void Start()
    {
        terrain = Terrain.activeTerrain;
        player = gameObject.transform;
        character = gameObject.GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        isGrounded = PlayerGrounded();
        isOnTerrain = CheckOnTerrain();
        currentSpeed = character.velocity.magnitude;
        walking = CheckIfWalking();

        if (walking)
        {
            distanceCoverd += (currentSpeed * Time.unscaledDeltaTime) * 0.5f;
            if (distanceCoverd > 1 && isGrounded)
            {
                if (isOnTerrain)
                {
                    PlayFootStepTerrain();
                }
                else
                {
                    PlayFootStepTag();
                }
                
                distanceCoverd = 0;
            }
        }

        
    }

    bool CheckIfWalking()
    {
        if (currentSpeed > 0 && isGrounded)
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    bool PlayerGrounded()
    {
        return Physics.Raycast(transform.position, Vector3.down, out hit, playerCollider.bounds.extents.y + 0.5f);
    }

    bool CheckOnTerrain()
    {
        if(hit.collider != null && hit.collider.tag == "Terrain")
        {
            return true;
        }
        else
        {
            return false;
        }
    }

    public void PlayFootStepTag()
    {
        if(hit.collider.tag == "Home_Wood")
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Footstep/Home Wood");
        }
        else if (hit.collider.tag == "Cael_Floor")
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Footstep/Wood_1");
        }
        else if (hit.collider.tag == "Cael_Stair")
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Footstep/Cobblestone");
        }
        else if (hit.collider.tag == "Home_Carpet")
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Footstep/Carpet");
        }
        else if (hit.collider.tag == "Stone")
        {
            FMODUnity.RuntimeManager.PlayOneShot("event:/Footstep/Cobblestone");
        }
    }

    public void PlayFootStepTerrain()
    {
        
        GetTerrainTexture();
        if (textureValues[0] > 0)
        {
            //Debug.Log("Desert Grass - volume:" + textureValues[1]);
            FMODUnity.RuntimeManager.PlayOneShot("event:/Footstep/Grass");
        }
        else if (textureValues[1] > 0)
        {
            //Debug.Log("mud@ - volume:" + textureValues[3]);
            FMODUnity.RuntimeManager.PlayOneShot("event:/Footstep/Mud");
        }
        else if (textureValues[2] > 0)
        {
            //Debug.Log("road_Path- volume:" + textureValues[5]);
            FMODUnity.RuntimeManager.PlayOneShot("event:/Footstep/Cobblestone");
        }
        else if (textureValues[3] > 0)
        {
            //Debug.Log("leaf_Forest - volume:" + textureValues[6]);
            FMODUnity.RuntimeManager.PlayOneShot("event:/Footstep/Grass");
        }
        else if (textureValues[4] > 0)
        {
            //Debug.Log("newlayer - volume:" + textureValues[7]);
            FMODUnity.RuntimeManager.PlayOneShot("event:/Footstep/Gravel");
        }
    }

    public void GetTerrainTexture()
    {
        ConvertPosition(player.position);
        CheckTexture();
    }

    void ConvertPosition(Vector3 playerPos)
    {
        Vector3 terrainPosition = playerPos - terrain.transform.position;

        Vector3 mapPos = new Vector3(terrainPosition.x / terrain.terrainData.size.x, 0, terrainPosition.z / terrain.terrainData.size.z);

        float xCoord = mapPos.x * terrain.terrainData.alphamapWidth;
        float zCoord = mapPos.z * terrain.terrainData.alphamapHeight;

        posX = (int)xCoord;
        posZ = (int)zCoord;
    }

    void CheckTexture()
    {
        float[,,] aMap = terrain.terrainData.GetAlphamaps(posX, posZ, 1, 1);
        textureValues[0] = aMap[0, 0, 0];
        textureValues[1] = aMap[0, 0, 1];
        textureValues[2] = aMap[0, 0, 2];
        textureValues[3] = aMap[0, 0, 3];
        textureValues[4] = aMap[0, 0, 4];
    }
}
