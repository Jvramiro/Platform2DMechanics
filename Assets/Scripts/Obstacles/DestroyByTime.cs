using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DestroyByTime : MonoBehaviour
{
    [SerializeField] private float timeToDestroy = 5;
    void Start(){
        StartCoroutine(DestroyGameObject());
    }

    IEnumerator DestroyGameObject(){
        yield return new WaitForSeconds(timeToDestroy);
        Destroy(gameObject);
    }
}
