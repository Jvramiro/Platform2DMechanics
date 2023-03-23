using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(fileName = "Dialog", menuName = "ScriptableObjects/Dialog", order = 1)]
public class SO_Dialog : ScriptableObject
{
    [System.Serializable]
    public class DialogClass{
        public int id;
        [TextArea]
        public List<string> dialog;
        [TextArea]
        public List<string> dialogPT;
    }
    [SerializeField]
    public List<DialogClass> mainDialogs = new List<DialogClass>();
}
