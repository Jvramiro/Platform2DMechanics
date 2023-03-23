using UnityEngine;
using TMPro;

public class SelectionText : MonoBehaviour
{
    [TextArea] public string[] texts = new string[4];
    [SerializeField] private TMP_Text[] selections = new TMP_Text[4];
    [HideInInspector] public TMP_Text[] get_selections {get{return selections;}}
    public int selected;
    void Update(){
        for(int i = 0; i < 4; i++){
            if(selections[i].text != texts[i]){
                selections[i].text = texts[i];
            }
            if(string.IsNullOrEmpty(texts[i]) && selections[i].gameObject.activeSelf){
                selections[i].gameObject.SetActive(false);
            }
            else if(!string.IsNullOrEmpty(texts[i]) && !selections[i].gameObject.activeSelf){
                selections[i].gameObject.SetActive(true);
            }
        }
    }

    public void ChangeSelected(int index, Color unselected, Color selected){
        this.selected = index;
        for(int i = 0; i < 4; i++){
            if(i != index){
                selections[i].color = unselected;
            }
            else{
                selections[i].color = selected;
            }
        }
    }
}
