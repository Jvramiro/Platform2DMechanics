using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PauseSystem : MonoBehaviour
{
    public static PauseSystem Singleton;
    void Awake() {
        if (Singleton != null && Singleton != this){
            Destroy(this.gameObject);
        }
        else{
            Singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }
    }

    private bool isPaused = false;
    public bool CheckIsPause{get{return isPaused;}}

    public void PauseGame(){
        isPaused = true;
        PauseUpdates();
    }

    public void UnpauseGame(){
        isPaused = false;
        PauseUpdates();
    }
    public void SwitchPauseGame(){
        isPaused = !isPaused;
        PauseUpdates();
    }

    void Update(){
        if(Input.GetKeyDown(KeyCode.P)){
            SwitchPauseGame();
        }
    }

    void PauseUpdates(){
        UpdateTimeScale();
        UpdateCanvasObject();
    }

    void UpdateTimeScale(){
        Time.timeScale = isPaused ? 0f : 1f;
    }

    void UpdateCanvasObject(){
        GameObject pauseCanvas = GameObject.FindGameObjectWithTag("PauseCanvas");
        if(pauseCanvas != null){
            pauseCanvas.transform.GetChild(0).gameObject.SetActive(isPaused);
        }
    }

}
