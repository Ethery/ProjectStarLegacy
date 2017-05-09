using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class StartingInventory : MonoBehaviour {

    [SerializeField]
    public List<ItemShort> invList;

    // Use this for initialization
    void Start () {
        
        if (!FindObjectOfType<SavesManager>().main.levels[SceneManager.GetActiveScene().buildIndex].finished)
        {
            foreach (ItemShort item in invList)
            {
                FindObjectOfType<InventoryManager>().addItem(item);
            }
            FindObjectOfType<SavesManager>().saveInventory(FindObjectOfType<InventoryManager>().getInventory());
        }
    } 
}