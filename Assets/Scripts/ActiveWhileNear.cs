using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ActiveWhileNear : MonoBehaviour
{
    [SerializeField] private float minDistance = 15;
    [SerializeField] private Behaviour component;
    private Transform player;
    private bool isNear = false;
    //[SerializeField]
    private float playerDistance;

    void Start(){
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }
    void Update(){
        playerDistance = Vector2.Distance(transform.position,player.position);
        component.enabled = playerDistance < minDistance;
    }
}
