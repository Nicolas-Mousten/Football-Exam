using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Player : MonoBehaviour
{
    public GameObject cam;
    
    [Header("References")]
    public Transform trans;
    public Transform modelTrans;
    public CharacterController characterController;

    //movement
    [Header("Movement")]
    [Tooltip("Units moved per second at maximum speed.")]
    public float movespeed = 24;
    [Tooltip("Time, in seconds, to reach maximum speed")]
    public float timeToMaxSpeed = 0.26f;
    private float VelocityGainPerSecond { get {return movespeed / timeToMaxSpeed; } }
    [Tooltip("Time, in seconds, to go from maximum speed to stationary.")]
    public float timeToLoseMaxSpeed = 0.2F;
    private float VelocityLossPerSecond { get { return movespeed / timeToLoseMaxSpeed; } }
    
    [Tooltip("Multiplier for momentum when attempting to move in a  direction opposite the current traveling direction (e.g. trying to move right when alreadt moving left).")]
    public float reverseMomentumMultiplier = 2.2f;
    private Vector3 movementVelocity = Vector3.zero;
    // Start is called before the first frame update

    [Tooltip("Field Max Size x/y")]
    public int fieldX = 0;
    public int fieldZ = 0;
    public float respawnWaitTime = 2f;
    private bool dead = false;
    private Vector3 spawnpoint;
    private Quaternion spawnRotation;
    




    void Start()
    {
        spawnpoint = trans.position;
        spawnRotation = modelTrans.rotation;
    }

    // Update is called once per frame
    void Update()
    {
        Movement();
        PlayerLocation();
        if(Input.GetKeyDown(KeyCode.T)){
            outOfBounds();
        }

        //Esc
        if(Input.GetKeyDown(KeyCode.Escape)){
            SceneManager.LoadScene("Main Menu");
        }
    }
    private void PlayerLocation(){
        //Player Location
        float PlayerLocationX = transform.position.x;
        float PlayerLocationZ = transform.position.z;
        //Debug.Log("["+PlayerLocationX +", " + PlayerLocationZ +"]");
        //is to far to the right or left
        if(PlayerLocationX>=fieldX || PlayerLocationX <= -fieldX){
            //Debug.Log("out of field");
            outOfBounds();
        }
        if(PlayerLocationZ>=fieldZ || PlayerLocationZ <= -fieldZ){
            //Debug.Log("out of field");
            outOfBounds();
        }
        
    }
    public void instantRespawn(){
        if(!dead){
            dead = true;
            Invoke("Respawn", 0);
            movementVelocity = Vector3.zero;
            enabled = false;
            characterController.enabled = false;
            modelTrans.gameObject.SetActive(false);
        }
    }
    private void outOfBounds(){
        if(!dead){
            dead = true;
            Invoke("Respawn", respawnWaitTime);
            movementVelocity = Vector3.zero;
            enabled = false;
            characterController.enabled = false;
            modelTrans.gameObject.SetActive(false);
        }
    }
    private void Respawn(){
       dead = false;
       trans.position = spawnpoint;
       enabled = true;
       characterController.enabled = true;
       modelTrans.gameObject.SetActive(true);
       modelTrans.rotation = spawnRotation;
    }
    private void Movement(){
        //If W or the up Key is held:
        if (Input.GetKey(KeyCode.W) || Input.GetKey(KeyCode.UpArrow)){
            if(movementVelocity.z >= 0){ //If we're already moving forward
                //Increase Z velocity by VelocityGainPerSEcond, but don't go higher than 'movespeed':
                movementVelocity.z = Mathf.Min(movespeed, movementVelocity.z + VelocityGainPerSecond * Time.deltaTime);
            }
            else //Else if we're moving back
            {
            //Increase Z velocity by VelocityGainPerSecond, using the reverseMomentumMultiplier, but don't raise higher than 0;
            movementVelocity.z = Mathf.Max(0,movementVelocity.z + VelocityGainPerSecond * reverseMomentumMultiplier * Time.deltaTime);
            }
        }
        
        //If S or the down arrow key is held:
        else if(Input.GetKey(KeyCode.S) || Input.GetKey(KeyCode.DownArrow)){
            if(movementVelocity.z > 0 ) //If we're already moving forward
            {
                movementVelocity.z = Mathf.Max(0,movementVelocity.z - VelocityGainPerSecond * reverseMomentumMultiplier * Time.deltaTime);
            }
            else //If we're moving back or not moving at all
            {
                movementVelocity.z = Mathf.Max(-movespeed, movementVelocity.z - VelocityGainPerSecond * Time.deltaTime);
            }
        }
        else{ //If neither forward nor back are being held
            //We must bring the Z Velocity back to 0 over time.
            if(movementVelocity.z > 0)//if we are moving up,
                //Decrease Z Velocity by VelocityLossPerSecond, but don't got any lower than 0
                movementVelocity.z = Mathf.Max(0, movementVelocity.z - VelocityLossPerSecond * Time.deltaTime);
            else //if we're moving down
                //Increase Z Velocity (back towards 0) by VelocityLossPerSecond, but don't go any higher than 0.
                movementVelocity.z = Mathf.Min(0, movementVelocity.z + VelocityLossPerSecond * Time.deltaTime);
        }
        //------------------------------Left And Right--------------------------------------------------------------
        if (Input.GetKey(KeyCode.D) || Input.GetKey(KeyCode.RightArrow)){
            if(movementVelocity.x >= 0){ 
                movementVelocity.x = Mathf.Min(movespeed, movementVelocity.x + VelocityGainPerSecond * Time.deltaTime);
            }
            else
            {
            movementVelocity.x = Mathf.Min(0,movementVelocity.x + VelocityGainPerSecond * reverseMomentumMultiplier * Time.deltaTime);
            }
        }
        
        else if(Input.GetKey(KeyCode.A) || Input.GetKey(KeyCode.LeftArrow)){
            if(movementVelocity.x > 0 )
            {
                movementVelocity.x = Mathf.Max(0,movementVelocity.x - VelocityGainPerSecond * reverseMomentumMultiplier * Time.deltaTime);
            }
            else
            {
                movementVelocity.x = Mathf.Max(-movespeed,movementVelocity.x - VelocityGainPerSecond * Time.deltaTime);
            }
        }
        else{
            if(movementVelocity.x > 0)
                movementVelocity.x = Mathf.Max(0, movementVelocity.x - VelocityLossPerSecond * Time.deltaTime);
            else 
                movementVelocity.x = Mathf.Min(0, movementVelocity.x + VelocityLossPerSecond * Time.deltaTime);
        }
        //If the player is moving in either direction(Left/Right or Up/Down):
        if(movementVelocity.x !=0 || movementVelocity.z !=0)
        {
            characterController.Move(movementVelocity * Time.deltaTime);
            modelTrans.rotation = Quaternion.Slerp(modelTrans.rotation, Quaternion.LookRotation(movementVelocity),0.18F);
        }
    }
}
