using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hazard : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other){
        if(other.gameObject.layer == 8){
            Debug.Log("Parry");
            var playerGobj = GameObject.Find("Player 1");
            var playerScript = playerGobj.GetComponent<Player>();
            var ballGobj = GameObject.Find("Ball");
            var ballScript = ballGobj.GetComponent<Ball>();
            ballScript.instantRespawn();
            playerScript.instantRespawn();
        }
    }
}
