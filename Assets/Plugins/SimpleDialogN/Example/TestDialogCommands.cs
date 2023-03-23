using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using SimpleDialogN;

public class TestDialogCommands : MonoBehaviour
{
    //List of Dialog to test the Dialog System
    [SerializeField] [TextArea] private List<string> testDialogText = new List<string>();

    //List of Selection Text to test the Dialog System with Selection
    [SerializeField] [TextArea] private List<string> testSelectionText = new List<string>();

    /*Dialog Index If you need to send a value back when the dialog is finished
    Example:
    I send a value of 1 and use DialogCommands.GetDialogController().DialogFinished to return the
    same value confirming that the dialog was finished and perform some function regarding the index
    */
    [SerializeField] private int dialogIndex;
    
    //Function that tests the dialog if it is set
    public void TestDialog(){
        DialogCommands.CallDialog(new List<string>(testDialogText), dialogIndex);
    }

    //Function that tests the dialog with selection if it is set
    public void TestDialogSelection(){
        DialogCommands.CallDialogSelection(new List<string>(testDialogText), new List<string>(testSelectionText));
    }

    //Inputs to finish or call the next text / change the selection
    void Update(){
        if(Input.GetKeyDown(KeyCode.X)){
            DialogCommands.PlayDialog();
            //PressingWriting True if you want to change the writing velocity while pressing button
            DialogCommands.PressingWriting(true);
        }
        if(Input.GetKeyDown(KeyCode.UpArrow)){
            DialogCommands.DialogSelectionChange(false);
        }
        if(Input.GetKeyDown(KeyCode.DownArrow)){
            DialogCommands.DialogSelectionChange(true);
        }

        //Using the Pressing Button Function
        //While pressing the button (Down to Up) the writing delay is another
        //To change the writing delay and writing delay while pressing button check the Dialog Controller by Inspector
        if(Input.GetKeyUp(KeyCode.X)){
            DialogCommands.PressingWriting(false);
        }

        //You can check if the Dialog Mode is active to pause game or do another stuff
        if(DialogCommands.IsDialogActive()){
            //Do stuff
        }
        
    }

    //Assigned functions to return the index for the text that is finished and also for the selection chosen if necessary
    void Start(){
        DialogCommands.GetDialogController().DialogFinished += DebugIndex;
        DialogCommands.GetDialogController().SelectionFinished += DebugSelection;
    }

    void DebugIndex(int id){
        Debug.Log($"Index {id}");
    }
    void DebugSelection(int id, int selection){
        Debug.Log($"Index {id}, Selection {id}");
    }
}
