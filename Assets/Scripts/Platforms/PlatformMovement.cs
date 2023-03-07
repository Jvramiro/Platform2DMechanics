using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using EasyButtons;

public class PlatformMovement : MonoBehaviour
{
    [SerializeField] private List<Vector2> points = new List<Vector2>();
    [SerializeField] private float velocity = 10;
    private int currentPoint = 0;
    [SerializeField] private bool ShowPoints = false;
    [SerializeField] private Vector2 startPosition;
    [SerializeField] private bool startPositionSet = false;
    [SerializeField] private float delayTime = 1;
    private bool movementDelay = false;

    void Start(){
        Vector2 startPositionToInsert = startPositionSet ? startPosition : transform.position;
        points.Insert(0, startPositionToInsert);
        if(startPositionSet){ ReturnStartPosition(); }
    }

    void Update(){
        if (Vector2.Distance(transform.position, points[currentPoint]) < 0.1f)
        {
            currentPoint++;

            if (currentPoint >= points.Count)
            {
                currentPoint = 0;
            }

            if(delayTime > 0){
                StartCoroutine(CallDelay());
            }
            
        }
        
    }

    void FixedUpdate(){
        if(!movementDelay){
            transform.position = Vector2.MoveTowards(transform.position, points[currentPoint], Time.fixedDeltaTime * velocity);
        }
    }

    IEnumerator CallDelay(){
        movementDelay = true;
        yield return new WaitForSeconds(delayTime);
        movementDelay = false;
    }

    [Button]
    public void AddPositionToList(){
        Vector2 currentPosition = transform.position;
        points.Add(currentPosition);
    }
    [Button]
    public void SetStartPosition(){
        startPosition = transform.position;
        startPositionSet = true;
    }
    [Button]
    public void ResetStartPosition(){
        startPosition = Vector2.zero;
        startPositionSet = false;
    }
    [Button]
    public void ReturnStartPosition(){
        if(startPositionSet){
            transform.position = startPosition;
        }
    }
    [Button]
    public void ClearList(){
        points.Clear();
    }

    void OnDrawGizmos(){
        if(!ShowPoints){return;}
        Gizmos.color = Color.green;
        foreach(var current in points){
            Gizmos.DrawSphere(current, 0.3f);
        }
    }

}
