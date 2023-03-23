using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Tilemaps;
using SingletonExtensions;

public class PlayerCollider : MonoBehaviour
{
    [SerializeField] private Interactions interactions;
    [SerializeField] private PlayerMovement playerMovement;
    [SerializeField] private Transform playerPosition;
    [SerializeField] private Vector2 knockbackForce = new Vector2(1200,300);
    [SerializeField] private float intangibleTime = 1f;
    void OnEnable() => SingletonExtension.GetInteractions().Knockback += Knockback;
    void OnDisable() => Interactions.Singleton.Knockback -= Knockback;
    private bool isIntangible = false;
    void OnTriggerEnter2D(Collider2D col){
        if(col.CompareTag("Hit") && !isIntangible){

            Vector3 colPoint = new Vector3();
            int direction = 0;
            if (col.GetComponent<Tilemap>() != null){
                direction = playerPosition.gameObject.GetComponent<PlayerMovement>().rightSide ? -1 : 1;
            }
            else{
                colPoint = col.bounds.ClosestPoint(transform.position);
                direction = playerPosition.position.x > colPoint.x ? 1 : playerPosition.position.x < colPoint.x ? -1 : 0;
            }

            interactions.UpdateHealth(-1);
            interactions.Knockback(direction);
        }

        if(col.CompareTag("HitKill") && !isIntangible){
            interactions.Death();
        }

        if(col.CompareTag("Item") && ItemsManager.Singleton != null){
            ItemsManager.Singleton.AddItem(col.transform.root.gameObject);
        }

        
    }

    void Knockback(int direction){
        StartCoroutine(Intangible());

        var rgb = playerMovement.getRgb;
        bool rightSide = playerMovement.rightSide;

        rgb.velocity = Vector2.zero;
        Vector2 knockBackForce = new Vector2(direction * knockbackForce.x, knockbackForce.y);
        rgb.AddForce(knockBackForce);
    }

    IEnumerator Intangible(){
        isIntangible = true;
        GetComponent<Collider2D>().enabled = false;
        yield return new WaitForSeconds(intangibleTime);
        GetComponent<Collider2D>().enabled = true;
        isIntangible = false;
    }

}
