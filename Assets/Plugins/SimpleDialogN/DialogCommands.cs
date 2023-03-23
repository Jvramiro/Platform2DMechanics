using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace SimpleDialogN{
    public class DialogCommands : MonoBehaviour
    {
        public static int SelectionLimit = 4;

        public static void CallDialog(List<string> dialogList, int dialogIndex = 0){
            DialogController dialogController = GetDialogController();
            if(dialogController == null){return;}

            if(!CheckCanCall(dialogController)){return;}

            dialogController.dialogList = new List<string>(dialogList);
            dialogController.selectionList = new List<string>();
            dialogController.PlayDialog(dialogIndex);
        }

        public static void PlayDialog(){
            DialogController dialogController = GetDialogController();
            if(dialogController == null){return;}

            dialogController.PlayDialog();
        }

        public static void CallDialogSelection(List<string> dialogList, List<string> selectionList, int dialogIndex = 0){
            DialogController dialogController = GetDialogController();
            if(dialogController == null){return;}

            if(selectionList.Count > SelectionLimit){
                Debug.LogError($"The maximum selections supported in this package are {SelectionLimit}");
                return;
            }

            dialogController.dialogList = new List<string>(dialogList);
            dialogController.selectionList = selectionList;
            dialogController.PlayDialog(dialogIndex);

        }

        public static void DialogSelectionChange(bool inputDown){
            DialogController dialogController = GetDialogController();
            if(dialogController == null){return;}

            if(dialogController.selectionList.Count <= 0){
                Debug.LogError($"There is no Selection Display to change selection");
                return;
            }
            
            dialogController.ChangeSelection(inputDown);
        }

        private static bool CheckCanCall(DialogController dialogController){
            if(dialogController.dialogActive){
                //Debug.LogWarning("Wait for the current dialog to end to start another one");
            }
            return !dialogController.dialogActive;
        }

        public static bool IsDialogActive(){
            DialogController dialogController = GetDialogController();
            if(dialogController == null){return false;}

            return dialogController.dialogActive;
        }

        public static void PressingWriting(bool isPressing){
            DialogController dialogController = GetDialogController();
            if(dialogController == null){return;}
            
            dialogController.PressingWriting(isPressing);
        }

        public static DialogController GetDialogController(){
            DialogController dialogController = (DialogController) GameObject.FindObjectOfType (typeof(DialogController));
            if(dialogController == null){
                Debug.LogError("Dialog Controller does not exists");
            }
            return dialogController;
        }
    }
}
