using UnityEngine;
using TMPro;

public class DialogText : MonoBehaviour
{
    [TextArea] public string dialog;
    [SerializeField] private TMP_Text _TMP_Text;
    [HideInInspector] public TMP_Text get_TMP_text {get{return _TMP_Text;}}
    void Update(){
        if(dialog != _TMP_Text.text){
            _TMP_Text.text = dialog;
        }
    }
}
