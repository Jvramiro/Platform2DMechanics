using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogCanvasDDOL : MonoBehaviour
{
    private static DialogCanvasDDOL _singleton;

    public static DialogCanvasDDOL Singleton{
        get => _singleton;
        private set{
            if(_singleton == null){
                _singleton = value;
                DontDestroyOnLoad(Singleton);
            }
            else if(_singleton != value){
                Destroy(value);
            }
        }
    }
    private void Awake(){
        Singleton = this;
    }
}
