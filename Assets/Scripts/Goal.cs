using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Goal : MonoBehaviour
{
    // Start is called before the first frame update
    public AudioClip goalSound;
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    void OnTriggerEnter(Collider other){
        if(other.gameObject.layer == 8){
            AudioSource.PlayClipAtPoint(goalSound,transform.position);
            Debug.Log("GOAL!!!");
            Invoke("LoadScene", 5);
            
        }
    }
    void LoadScene(){
        SceneManager.LoadScene("Main Menu");
    }
}
