using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PauseSystem : MonoBehaviour
{
    private InputActionsControl input;

    public static PauseSystem Singleton;

    void Awake() {
        if (Singleton != null && Singleton != this){
            Destroy(this.gameObject);
        }
        else{
            Singleton = this;
            DontDestroyOnLoad(this.gameObject);
        }

        input = new InputActionsControl();
    }

    private bool isPaused = false;
    public bool CheckIsPause{get{return isPaused;}}

    void OnEnable(){
        input.PlayerMap.Pause.started += PauseButton;
        input.PlayerMap.Pause.Enable();
    }

    #region Input Section

    void PauseButton(InputAction.CallbackContext context){
        SwitchPauseGame();
    }

    #endregion

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
