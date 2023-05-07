using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Camera : MonoBehaviour
{
    public Transform trans;
    public Transform targetObject;
    public float offset;




    // Start is called before the first frame update
    void Start()
    {
        trans = transform;
        
    }

    // Update is called once per frame
    void Update()
    {
        trans.position = new Vector3(targetObject.position.x, targetObject.position.y+offset, targetObject.position.z);//fallow object
        trans.eulerAngles = new Vector3 (90, trans.rotation.y, trans.rotation.z);//lock rotation
    }
}   
