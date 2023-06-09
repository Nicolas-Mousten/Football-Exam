using System.Collections;
using System;
using System.Collections.Generic;
using UnityEngine;

public class Patroller : MonoBehaviour
{
    private const float rotationSlerpAmount = 0.68f;
    [Header("References")]
    public Transform trans;
    public Transform modelHolder;

    [Header("Stats")]
    public float movespeed = 30;

    private int currentPointIndex;
    private Transform currentPoint;
    private Transform[] patrolPoints;
    // Start is called before the first frame update
    void Start()
    {
        List<Transform> points = GetUnsortedPatrolPoints();
        if(points.Count > 0){
            patrolPoints = new Transform[points.Count];
            for (int i = 0; i < points.Count; i++)
            {
                Transform point = points[i];
                int closingParenthesisIndex = point.gameObject.name.IndexOf(')');
                string indexSubstring = point.gameObject.name.Substring(14, closingParenthesisIndex - 14);

                int index = Convert.ToInt32(indexSubstring);
                patrolPoints[index] = point;

                point.SetParent(null);
                point.gameObject.hideFlags = HideFlags.HideInHierarchy;
            }
            SetCurrentPatrolPoint(0);
        }
    }
    private List<Transform> GetUnsortedPatrolPoints(){
        Transform[] children = gameObject.GetComponentsInChildren<Transform>();
        List<Transform> points = new List<Transform>();
        for (int i = 0; i < children.Length; i++)
        {
            if(children[i].gameObject.name.StartsWith("Patrol Point (")){
                points.Add(children[i]);
            }
            
        }
        return points;
    }
    // Update is called once per frame
    void Update()
    {
        if(currentPoint != null){
            trans.position = Vector3.MoveTowards(trans.position, currentPoint.position, movespeed * Time.deltaTime);
            if(trans.position == currentPoint.position){
                if(currentPointIndex >= patrolPoints.Length - 1){
                    SetCurrentPatrolPoint(0);
                }
                else{
                    SetCurrentPatrolPoint(currentPointIndex + 1);
                }
            }
            else{
                Quaternion lookRotation = Quaternion.LookRotation((currentPoint.position - trans.position).normalized);
                modelHolder.rotation = Quaternion.Slerp(trans.rotation, lookRotation, rotationSlerpAmount);
            }
        }
    }
    private void SetCurrentPatrolPoint(int index){
        currentPointIndex = index;
        currentPoint = patrolPoints[index];
    }
}
