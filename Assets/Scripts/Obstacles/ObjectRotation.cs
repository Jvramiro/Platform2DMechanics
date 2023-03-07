using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ObjectRotation : MonoBehaviour
{
    [SerializeField] private bool isPendulum;
    [SerializeField] private float speed, limitPendulum;
    private float currentRotation, direction = 1f;
    void FixedUpdate(){
        if(!isPendulum){
            transform.Rotate(new Vector3(0,0,speed) * Time.fixedDeltaTime, Space.Self);
        }
        else{
            currentRotation += speed * direction * Time.fixedDeltaTime;
            if(currentRotation >= limitPendulum || currentRotation <= -limitPendulum){
                direction *= -1f;
            }
            transform.rotation = Quaternion.Euler(0, 0, currentRotation);
        }
    }
}
