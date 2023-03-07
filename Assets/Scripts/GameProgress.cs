using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameProgress : MonoBehaviour
{
    public static GameProgress Singleton;
    void Awake() {
        if (Singleton != null && Singleton != this){
            Destroy(this.gameObject);
        }
        else{
            Singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    public bool DoubleJump;
    public bool WallJump;
    public bool Glide;
    public int heartSlices;
    
}
