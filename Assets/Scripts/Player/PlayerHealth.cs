using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Interactions interactions;
    [SerializeField] private int mainHealth = 3;
    public int health;

    void OnEnable(){
        interactions.UpdateHealth += UpdateHealth;
        interactions.Death += DestroyPlayer;
    }
    void OnDisable(){
        interactions.UpdateHealth -= UpdateHealth;
        interactions.Death -= DestroyPlayer;
    }

    void UpdateHealth(int quantity){
        health += quantity;
        health = health < 0 ? 0 : health;
    }

    void Start(){
        int getHeartSlices = GameProgress.Singleton.heartSlices;
        int plusHealth = (int)Mathf.Ceil(getHeartSlices/2);
        health += mainHealth + plusHealth;
    }

    void DestroyPlayer(){
        SceneManager.LoadScene(SceneManager.GetActiveScene().name);
    }


}
