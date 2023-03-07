using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotSpawner : MonoBehaviour
{
    [SerializeField] private float delay = 10;
    [SerializeField] private GameObject shot;
    [SerializeField] private float velocityX, velocityY;
    void OnEnable(){
        StopCoroutine(SummonObject());
        StartCoroutine(SummonObject());
    }

    void OnDisable(){
        StopCoroutine(SummonObject());
    }

    IEnumerator SummonObject(){
        yield return new WaitForSeconds(delay);
        if(!this.enabled){ yield break; }

        GameObject currentShot = Instantiate(shot, transform.position, Quaternion.identity);
        ShotMovement shotMovement = currentShot.GetComponent<ShotMovement>();
        shotMovement.velocityX = velocityX;
        shotMovement.velocityY = velocityY;

        StopCoroutine(SummonObject());
        StartCoroutine(SummonObject());
    }
}
