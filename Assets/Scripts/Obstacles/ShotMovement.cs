using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShotMovement : MonoBehaviour
{
    public float velocityX, velocityY;

    void FixedUpdate(){
        transform.Translate(new Vector3(velocityX,velocityY,0) * Time.fixedDeltaTime);
    }

}
