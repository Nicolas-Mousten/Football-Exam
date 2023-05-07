using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Ball : MonoBehaviour
{
    public GameObject cam;
    public Transform trans;
    [Tooltip("Field Max Size x/y")]
    public int BallfieldX = 0;
    public int BallfieldZ = 0;
    public float respawnWaitTime = 2f;
    private bool dead = false;
    private Vector3 spawnpoint;
    private Vector3 BallmovementVelocity = Vector3.zero;

    // Start is called before the first frame update
    void Start()
    {
        spawnpoint = trans.position;
    }

    // Update is called once per frame
    void Update()
    {
        BallLocation();
    }
    
    private void BallLocation(){
        //Player Location
        float BallLocationX = trans.position.x;
        float BallLocationZ = trans.position.z;
        //Debug.Log("["+BallLocationX +", " + BallLocationZ +"]");
        //is to far to the right or left
        if(BallLocationX>=BallfieldX || BallLocationX <= -BallfieldX){
            //Debug.Log("out of field");
            outOfBounds();
        }
        if(BallLocationZ>=BallfieldZ || BallLocationZ <= -BallfieldZ){
            //Debug.Log("out of field");
            outOfBounds();
        }
        
    }
    public void instantRespawn(){
        if(!dead){
            dead = true;
            Invoke("Respawn", 0);
            BallmovementVelocity = Vector3.zero;
            enabled = false;
        }
    }
    private void outOfBounds(){
        if(!dead){
            dead = true;
            Invoke("Respawn", respawnWaitTime);
            BallmovementVelocity = Vector3.zero;
            //Debug.Log("Ball out of bounds");
            enabled = false;
            //respawn player:
            var playerGobj = GameObject.Find("Player 1");
            var playerScript = playerGobj.GetComponent<Player>();
            if(playerScript == null){
                Debug.Log("Can't find player script");
            }else{
                playerScript.instantRespawn();
            }
            
        }
    }
    private void Respawn(){
        //Debug.Log("Ball respawned");
        dead = false;
        trans.position = spawnpoint;
        enabled = true;
    }
}
