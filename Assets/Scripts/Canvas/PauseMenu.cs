using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class PauseMenu : MonoBehaviour
{
    [SerializeField] private List<string> menus = new List<string>();
    [SerializeField] private TMP_Text menuText;
    [SerializeField] private bool toDownTest;
    [SerializeField] private int selectedMenu;

    void Update(){
        if(Input.GetKeyDown(KeyCode.UpArrow) && PauseSystem.Singleton.CheckIsPause){
            ChangeMenu(false);
        }
        if(Input.GetKeyDown(KeyCode.DownArrow) && PauseSystem.Singleton.CheckIsPause){
            ChangeMenu(true);
        }
        if(Input.GetKeyDown(KeyCode.Return)){
            CallMenuOption();
        }
    }

    public void ChangeMenu(bool toDown){
        selectedMenu = toDown && menus.Count > selectedMenu + 1 ? selectedMenu + 1 :
                        !toDown && selectedMenu == 0 ? menus.Count - 1 :
                        !toDown && selectedMenu > 0 ? selectedMenu - 1 : 0;
        UpdateMenu();
    }

    [EasyButtons.Button]
    public void TestChangeMenu(){
        selectedMenu = toDownTest && menus.Count > selectedMenu + 1 ? selectedMenu + 1 :
                        !toDownTest && selectedMenu == 0 ? menus.Count - 1 :
                        !toDownTest && selectedMenu > 0 ? selectedMenu - 1 : 0;
        UpdateMenu();
    }

    [EasyButtons.Button]
    public void UpdateMenu(){
        if(menus.Count == 0){return;}

        menuText.text = "";

        for(int i = 0; i < menus.Count; i++){
            string toAdd = selectedMenu != i ? menus[i] : $"<i>> {menus[i]} <</i>";
            toAdd += "\n";
            menuText.text += toAdd;
        }

    }

    public void CallMenuOption(){
        switch(selectedMenu){
            case 0 : Resume();
            break;
            case 1 : Options();
            break;
            case 2 : ExitToMenu();
            break;
            case 3 : ExitGame();
            break;
        }
    }

    void Resume(){
        PauseSystem.Singleton.UnpauseGame();
    }

    void Options(){

    }

    void ExitToMenu(){

    }

    void ExitGame(){

    }
}
