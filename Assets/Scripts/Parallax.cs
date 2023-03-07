using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Parallax : MonoBehaviour
{
    private Vector2 length, startpos;
    private Camera cam;
    [SerializeField] private Vector2 parallax;
    void Start(){
        cam = GameObject.FindWithTag("MainCamera").GetComponent<Camera>();
        startpos = transform.position;
        length = new Vector2(GetComponent<SpriteRenderer>().bounds.size.x, GetComponent<SpriteRenderer>().bounds.size.y);
    }

    void Update(){
        Vector2 distance = new Vector2(cam.transform.position.x * parallax.x, cam.transform.position.y * parallax.y);
        transform.position = new Vector3(startpos.x + distance.x, startpos.y + distance.y, transform.position.z);
    }
}
