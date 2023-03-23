using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class DialogController : MonoBehaviour
{
    [SerializeField] private DialogText dialogText;
    [SerializeField] private SelectionText selectionText;
    [SerializeField] private GameObject dialogObject, selectionObject;
    [SerializeField] private Color unselectedColor = new Color(1,1,1,0.5f), selectedColor = new Color(1,1,1,1);
    [SerializeField] private GameObject prefab;
    [HideInInspector] public List<string> dialogList = new List<string>();
    [HideInInspector] public bool dialogActive {get{ return dialogObject.activeSelf; }}
    public float writeDelay = 0.05f;
    public float pressingDelay = 0.01f;
    private float writeDelayTotal = 0.05f;
    [HideInInspector] public int dialogIndex = 0;
    [HideInInspector] public List<string> selectionList = new List<string>();

    private bool isWriting = false;
    private bool antiSpam = false;

    public Action<int> DialogFinished;
    public Action<int, int> SelectionFinished;

    void Awake(){
        DialogFinished += FinishDialog;
        SelectionFinished += FinishSeletion;
    }

    public void PlayDialog(int dialogIndex = 0){
        if(antiSpam){return;}
        if(dialogIndex != 0){ this.dialogIndex = dialogIndex; }

        if(isWriting){ return; }
        if(dialogList.Count <= 0){ CheckFinishDialog(); return; }
        
        if(!dialogObject.activeSelf){ dialogObject.SetActive(true); }

        string dialogLine = dialogList[0];
        dialogList.RemoveAt(0);
        StartCoroutine(WriteText(dialogLine));
        StartCoroutine(TimeAntiSpam());
    }

    void CheckFinishDialog(){
        if(dialogList.Count <= 0 && dialogObject.activeSelf && selectionList.Count <= 0){
            DialogFinished(dialogIndex);  
        }
        else if(dialogList.Count <= 0 && dialogObject.activeSelf && selectionList.Count > 0 && !selectionObject.activeSelf){
            EnableSelectionList();
        }
        else if(dialogList.Count <= 0 && dialogObject.activeSelf && selectionObject.activeSelf){
            DialogFinished(dialogIndex);
            SelectionFinished(dialogIndex, selectionText.selected);
        }

    }

    public void FinishDialog(int dialogIndex = 0){
        dialogObject.SetActive(false);

        selectionList = new List<string>();
        selectionText.texts = new string[4];
        dialogText.dialog = "";
        dialogText.get_TMP_text.text = "";
        foreach(var current in selectionText.get_selections){ current.text = ""; }
        dialogIndex = 0;
        if(selectionObject.activeSelf){
            selectionObject.SetActive(false);
        }
    }

    void EnableSelectionList(){
        FillSelectionList();
        selectionObject.SetActive(true);
        selectionText.ChangeSelected(0, unselectedColor, selectedColor);
    }

    void FillSelectionList(){
        for(int i = 0; i < selectionList.Count; i++){
            selectionText.texts[i] = selectionList[i];
        }
    }

    public void ChangeSelection(bool down){
        int selected = selectionText.selected;
        selected = down ? selected + 1 : selected - 1;
        selected = selected < 0 ? 0 : selected > (selectionList.Count - 1) ? (selectionList.Count - 1) : selected;

        selectionText.ChangeSelected(selected, unselectedColor, selectedColor);
    }

    public void PressingWriting(bool isPressing){
        writeDelayTotal = isPressing ? pressingDelay : writeDelay;
    }

    void FinishSeletion(int index, int selection){}

    IEnumerator WriteText(string dialogLine){
        isWriting = true;

        dialogText.dialog = "";

        foreach(char ch in dialogLine){
            dialogText.dialog += ch;
            yield return new WaitForSeconds(writeDelayTotal);
        }

        isWriting = false;
    }

    public void InstatiateDialogCanvas(){
        DialogCanvasGet dialogCanvas = (DialogCanvasGet) GameObject.FindObjectOfType (typeof(DialogCanvasGet));
        if(dialogCanvas != null){
            Debug.LogError("There is Already a Dialog Canvas in the scene");
            return;
        }

        try{
            GameObject canvas = Instantiate(prefab, Vector3.zero, Quaternion.identity);
            canvas.name = "DialogCanvas";
            dialogObject = canvas.transform.GetChild(1).gameObject;
            selectionObject = canvas.transform.GetChild(0).gameObject;

            dialogText = canvas.transform.GetChild(1).GetChild(1).GetComponent<DialogText>();
            selectionText = canvas.transform.GetChild(0).GetComponent<SelectionText>();

        }catch{
            Debug.LogError("An error occurred while setting the Dialog Canvas, check the integrity of the files");
        }

    }

    public void SetDontDestroyOnLoad(){
        DialogCanvasGet dialogCanvas = (DialogCanvasGet) GameObject.FindObjectOfType (typeof(DialogCanvasGet));
        if(dialogCanvas == null){
            Debug.LogError("There is no Dialog Canvas in the scene");
            return;
        }
        gameObject.AddComponent<DialogCanvasDDOL>();
        dialogCanvas.gameObject.AddComponent<DialogCanvasDDOL>();
    }

    IEnumerator TimeAntiSpam(){
        antiSpam = true;
        yield return new WaitForSeconds(0.1f);
        antiSpam = false;
    }
    
}
