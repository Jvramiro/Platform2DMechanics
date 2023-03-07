using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Interactions : MonoBehaviour
{
    public static Interactions Singleton;
    void Awake() {
        if (Singleton != null && Singleton != this){
            Destroy(this.gameObject);
        }
        else{
            Singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public delegate void Action();
    public delegate void BoolAction(bool isTrue);
    public delegate void IntAction(int quantity);
    public Action Jump, SaveProgress, Death;
    public IntAction UpdateHealth, Knockback, GetBoolean, GetItem;
}
