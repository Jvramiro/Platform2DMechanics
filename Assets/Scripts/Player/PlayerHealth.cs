using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using SingletonExtensions;

public class PlayerHealth : MonoBehaviour
{
    [SerializeField] private Interactions interactions;
    [SerializeField] private int mainHealth = 3;
    public int health;

    void OnEnable(){
        SingletonExtension.GetInteractions().UpdateHealth += UpdateHealth;
        SingletonExtension.GetInteractions().Death += DestroyPlayer;
    }
    void OnDisable(){
        Interactions.Singleton.UpdateHealth -= UpdateHealth;
        Interactions.Singleton.Death -= DestroyPlayer;
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
