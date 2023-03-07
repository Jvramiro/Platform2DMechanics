using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.Text;
using UnityEngine.SceneManagement;

public class ItemsManager : MonoBehaviour
{
    public static ItemsManager Singleton;
    void Awake() {
        if (Singleton != null && Singleton != this){
            Destroy(this.gameObject);
        }
        else{
            Singleton = this;
        }
    }

    [System.Serializable]
    public struct ItemUnit{
        public int id;
        public bool isCollected;
        public GameObject itemObject;
    }

    [SerializeField] private List<ItemUnit> items = new List<ItemUnit>();
    [SerializeField] private Interactions interactions;
    public List<ItemUnit> getItems {get{return items;}}
    void Start(){
        
        string getItems = PlayerPrefs.GetString("GItems-" + SceneManager.GetActiveScene());
        if(!string.IsNullOrEmpty(getItems)){
            List<ItemUnit> getItemsList = DecryptList(getItems);
            items = new List<ItemUnit>(getItemsList);
        }
        UpdateSceneItems();
    }

    public void AddItem(GameObject itemObject){
        for(int i = 0; i < items.Count; i++){
            if(itemObject == items[i].itemObject && !items[i].isCollected){
                ItemUnit itemUnit = items[i];
                itemUnit.isCollected = true;
                items[i] = itemUnit;
                UpdateSceneItems();
                return;
            }
        }
    }
    public void RemoveItem(GameObject itemObject){
        for(int i = 0; i < items.Count; i++){
            //If unique item like a Key
            if(itemObject == items[i].itemObject){
                ItemUnit itemUnit = items[i];
                itemUnit.isCollected = false;
                items[i] = itemUnit;
                UpdateSceneItems();
                return;
            }
        }
    }

    public void ClearCollectedItems(){
        for(int i = 0; i < items.Count; i++){
            ItemUnit itemUnit = items[i];
            itemUnit.isCollected = false;
            items[i] = itemUnit;
        }
    }

    void UpdateSceneItems(){
        for(int i = 0; i < items.Count; i++){
            if(items[i].isCollected){
                GameObject itemObject = items[i].itemObject;
                if(itemObject.activeSelf){
                    itemObject?.SetActive(false);
                }
            }
        }
    }

    void OnDisable() => UpdateList();
    void UpdateList(){
        //Change to function that will be called when save is called
        PlayerPrefs.SetString("GItems-" + SceneManager.GetActiveScene(), EncryptList(items));
        //Debug.Log(PlayerPrefs.GetString("GItems-" + SceneManager.GetActiveScene()));
    }

    private string EncryptList(List<ItemUnit> items)
    {
        StringBuilder builder = new StringBuilder();
        foreach (ItemUnit item in items)
        {
            builder.Append(item.id);
            builder.Append(",");
            builder.Append(item.isCollected);
            builder.Append(";");
        }

        return builder.ToString();
    }

    private List<ItemUnit> DecryptList(string encryptedString)
    {
        List<ItemUnit> decryptedList = new List<ItemUnit>();
        string[] itemsString = encryptedString.Split(';');
        foreach (string item in itemsString)
        {
            if (string.IsNullOrEmpty(item))
                continue;

            string[] itemData = item.Split(',');
            int id = int.Parse(itemData[0]);
            bool isCollected = bool.Parse(itemData[1]);
            GameObject itemObject = null;

            foreach(var current in items){
                if(current.id == id){
                    itemObject = current.itemObject;
                }
            }

            decryptedList.Add(new ItemUnit { id = id, isCollected = isCollected, itemObject = itemObject });
        }
        return decryptedList;
    }

    [EasyButtons.Button] public void ResetPlayerPrefs() => PlayerPrefs.DeleteAll();
}
