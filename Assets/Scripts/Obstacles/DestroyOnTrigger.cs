using UnityEngine;

public class DestroyOnTrigger : MonoBehaviour
{
    void OnTriggerEnter2D(Collider2D col){
        Invoke(nameof(DestroyGameObject),0.1f);
    }

    void DestroyGameObject(){
        Destroy(gameObject);
    }

}
