using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.Audio;

public class LevelSelectUI : MonoBehaviour
{
    public AudioClip startSound;
    private int currentScene = 0;
    private GameObject levelViewCamera;
    private AsyncOperation currentLoadOperation;
    // Start is called before the first frame update
    void Start()
    {
        DontDestroyOnLoad(gameObject);
    }

    // Update is called once per frame
    void Update()
    {
        if(currentLoadOperation != null && currentLoadOperation.isDone){
            currentLoadOperation = null;
            levelViewCamera = GameObject.Find("Level View Camera");
            if(levelViewCamera == null){
                Debug.LogError("No Level view camera was found in the scene!");
            }
        }
    }
    void OnGUI()
    {
        GUILayout.Label("Field Selector");
        if(currentScene != 0){
            GUILayout.Label("Currently viewing Level " + currentScene);

            if(GUILayout.Button("PLAY")){
                PlayCurrentLevel();
            }
        }
        else
            GUILayout.Label("Select a level to preview it.");
        for (int i = 1; i < SceneManager.sceneCountInBuildSettings; i++)
        {
            if(GUILayout.Button("Level " + i)){
                if(currentLoadOperation == null){
                    currentLoadOperation = SceneManager.LoadSceneAsync(i);

                    currentScene = i;
                }
            }
        }
    
    }
    private void PlayCurrentLevel(){
        levelViewCamera.SetActive(false);
        var playerGobj = GameObject.Find("Player 1");
        var ballGobj = GameObject.Find("Ball");
        if(playerGobj == null || ballGobj == null){
            Debug.LogError("Couldn't find a player or ball in the level!");
        }else{
            var playerScript = playerGobj.GetComponent<Player>();
            var ballScript = ballGobj.GetComponent<Ball>();
            playerScript.enabled = true;
            //playerScript.cam.SetActive(true);
            ballScript.cam.SetActive(true);
            Destroy(this.gameObject);
            //play start sound:
            AudioSource.PlayClipAtPoint(startSound, ballGobj.transform.position);
        }
        
    }
}
