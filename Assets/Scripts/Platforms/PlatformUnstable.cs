using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlatformUnstable : MonoBehaviour
{
    [SerializeField] private float velocity = 1f, unstableTimer = 2.0f, resetTimer = 3.0f, fallDistance = 3, unstableMovement = 0.05f;
    private Vector3 startPosition, fallingPosition;
    private Quaternion startRotation;
    private bool isUnstable, afterUnstable;
    [SerializeField] private Collider2D col;
    [SerializeField] private SpriteRenderer sprite;

    void Start(){
        startPosition = transform.position;
        startRotation = transform.rotation;

        FillNullVariables();

        fallingPosition = new Vector3(transform.position.x, transform.position.y - fallDistance, transform.position.z);
        isUnstable = false;
        afterUnstable = false;
    }
    void Update(){

        if(isUnstable){
            Shaking();
        }
        else if(afterUnstable){
            StartCoroutine(TimerToReset());
            afterUnstable = false;

        }

    }

    void Shaking(){
        float x = Mathf.Sin(Time.time * 10f) * unstableMovement;
        float y = Mathf.Sin(Time.time * 12f) * unstableMovement;
            
        transform.position = startPosition + new Vector3(x, y, 0);
        transform.rotation = new Quaternion(
            startRotation.x + x * 0.2f,
            startRotation.y + y * 0.2f,
            0,
            startRotation.w
        );
    }

    void UpdatePlatformSettings(bool _isUnstable, bool _afterUnstable){
        isUnstable = _isUnstable;
        afterUnstable = _afterUnstable;
        
        col.enabled = !_afterUnstable;
        sprite.enabled = !_afterUnstable;
        //Debug.Log($"Update Platform Settings - isUnstable {_isUnstable} - afterUnstable {_afterUnstable}");
    }

    void ResetPlatform(){
        GetComponent<GetPlayerToChild>()?.CheckChildToTransformPosition();
        transform.position = startPosition;
        col.enabled = true;
        sprite.enabled = true;
    }

    IEnumerator TimerToFall(){
        if(isUnstable){ yield break; }
        if(!CheckNotNullVariables()){ NullVariablesError(); yield break; }

        UpdatePlatformSettings(true, false);
        yield return new WaitForSeconds(unstableTimer);
        UpdatePlatformSettings(false, true);
    }

    IEnumerator TimerToReset(){
        yield return new WaitForSeconds(resetTimer);
        ResetPlatform();
    }


    [EasyButtons.Button]
    public void Break(){
        StartCoroutine(TimerToFall());
    }

    void FillNullVariables(){
        if(col == null){ 
            if(GetComponent<Collider2D>() != null) {
                col = GetComponent<Collider2D>();
            } 
        }
        if(sprite == null){
            if(GetComponent<SpriteRenderer>() != null) {
                sprite = GetComponent<SpriteRenderer>();
            } 
        }
    }

    bool CheckNotNullVariables(){
        return col != null && sprite != null;
    }

    void NullVariablesError(){
        Debug.LogError("Collider2D or SpriteRenderer are Missing in Object or Serializable Variables");
    }

}
