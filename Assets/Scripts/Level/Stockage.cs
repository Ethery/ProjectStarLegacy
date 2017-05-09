using System;
using UnityEngine.UI;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class Stockage : Interactable {

    public GameObject display;
    public Transform contentInventory, contentStock;

    [SerializeField]
    private Inventory stock, inventory;
    public Transform prefabItem;
    public Dictionary<string, ObjectDictionaryItem> objectList;
    public string lastSelected = "";

    public Inventory getStock()
    {
        return stock;
    }

    public Inventory getInventory()
    {
        return FindObjectOfType<SavesManager>().main.items;
    }

    public override void Activate(bool a)
    {
        active = a;
        FindObjectOfType<PlayerInputManager>().canMove = !a;
        FindObjectOfType<PlayerInputManager>().canUse = !a;
        display.SetActive(a);
        if (a)
        {
            Time.timeScale = 0f;
            updateDisplay();
        }
        else
        {
            Time.timeScale = 1f;
        }
    }

    public override bool Activate(bool a, string key)
    {
        if (a && key == "Submit")
        {
            stock = FindObjectOfType<SavesManager>().main.stock;
            inventory = FindObjectOfType<SavesManager>().main.items;
            active = a;
            FindObjectOfType<PlayerInputManager>().canMove = false;
            FindObjectOfType<PlayerInputManager>().canUse = false;
            Time.timeScale = 0f;
            updateDisplay();
            display.SetActive(true);
            return true;
        }
        if (!a && key == "Cancel")
        {
            FindObjectOfType<SavesManager>().saveStockInventory(stock, inventory);
            active = a;
            FindObjectOfType<PlayerInputManager>().canMove = true;
            FindObjectOfType<PlayerInputManager>().canUse = true;
            Time.timeScale = 1f;
            EventSystem.current.SetSelectedGameObject(null);
            display.SetActive(false);
            return true;
        }
        return false;
    }

    public override bool nextStep(string key)
    {
        lastSelected = "";

        if (EventSystem.current.currentSelectedGameObject != null)
        {

            if (EventSystem.current.currentSelectedGameObject.transform.IsChildOf(contentInventory) && key == "Submit")
            {
                int item = inventory.FindIndex(search => search.name == EventSystem.current.currentSelectedGameObject.name);
                int target = stock.FindIndex(search => search.name == EventSystem.current.currentSelectedGameObject.name);
                if (inventory[item] != null)
                {
                    inventory[item].owned = inventory[item].owned - 1;
                    if (target >= 0)
                    {
                        stock[target].owned++;
                    }
                    else
                    {
                        stock.Add(new Item(inventory[item].name, 1, inventory[item].tags, inventory[item].stats, inventory[item].description));
                    }

                    lastSelected = "inventory:" + EventSystem.current.currentSelectedGameObject.name;
                    if (inventory[item].owned < 1)
                    {
                        lastSelected = "";
                        inventory.RemoveAt(item);
                    }
                    updateDisplay();
                    return true;
                }
            }
            if (EventSystem.current.currentSelectedGameObject.transform.IsChildOf(contentStock) && key == "Submit")
            {
                int item = stock.FindIndex(search => search.name == EventSystem.current.currentSelectedGameObject.name);
                int target = inventory.FindIndex(search => search.name == EventSystem.current.currentSelectedGameObject.name);
                if (stock[item] != null)
                {
                    stock[item].owned = stock[item].owned - 1;
                    if (target >= 0)
                    {
                        inventory[target].owned++;
                    }
                    else
                    {
                        inventory.Add(new Item(stock[item].name, 1, stock[item].tags, stock[item].stats, stock[item].description));
                    }
                    lastSelected = "stock:" + EventSystem.current.currentSelectedGameObject.name;
                    if (stock[item].owned < 1)
                    {
                        lastSelected = "";
                        stock.RemoveAt(item);
                    }
                    updateDisplay();
                    return true;
                }
            }
        }
        return true;
    }

    public void updateDisplay()
    {
        //On supprime tous les boutons
        for (int i = 0; i < contentInventory.childCount || i < contentStock.childCount; i++)
        {
            if (i < contentStock.childCount)
            {
                Destroy(contentStock.GetChild(i).gameObject);
            }
            if (i < contentInventory.childCount)
            {
                Destroy(contentInventory.GetChild(i).gameObject);
            }
        }
        //On parcours l'inventaire du joueur
        for (int i = 0; i < inventory.Count; i++)
        {
            //Si l'objet de l'inventaire est referenc� dans le dictionnaire
            if (objectList.ContainsKey(inventory[i].name) && inventory[i].owned > 0)
            {
                int owned = inventory[i].owned;
                for (int j = 0; j < Math.Ceiling((float)owned / objectList[inventory[i].name].stackValue); j++)
                {
                    Transform slot = Instantiate(prefabItem, contentInventory, false);
                    slot.name = inventory[i].name;
                    slot.FindChild("Image").GetComponent<Image>().sprite = objectList[inventory[i].name].GetComponent<SpriteRenderer>().sprite;
                    slot.FindChild("Text").GetComponent<Text>().text = inventory[i].name;
                    if (owned > objectList[inventory[i].name].stackValue)
                    {
                        slot.FindChild("Owned").GetComponent<Text>().text = objectList[inventory[i].name].stackValue.ToString();
                    }
                    else
                    {
                        slot.FindChild("Owned").GetComponent<Text>().text = inventory[i].owned.ToString();
                    }
                    owned -= objectList[inventory[i].name].stackValue;
                }
            }
        }
        // On parcours tout le tableau qui contient le stock
        for (int i = 0; i < stock.Count; i++)
        {
            //Si l'objet de l'inventaire est referenc� dans le dictionnaire
            if (objectList.ContainsKey(stock[i].name) && stock[i].owned > 0)
            {
                int owned = stock[i].owned;
                for (int j = 0; j < Math.Ceiling((float)owned / objectList[stock[i].name].stackValue); j++)
                {
                    Transform slot = Instantiate(prefabItem, contentStock, false);
                    slot.name = stock[i].name;
                    slot.FindChild("Image").GetComponent<Image>().sprite = objectList[stock[i].name].GetComponent<SpriteRenderer>().sprite;
                    slot.FindChild("Text").GetComponent<Text>().text = stock[i].name;
                    if (owned > objectList[stock[i].name].stackValue)
                    {
                        slot.FindChild("Owned").GetComponent<Text>().text = objectList[stock[i].name].stackValue.ToString();
                    }
                    else
                    {
                        slot.FindChild("Owned").GetComponent<Text>().text = stock[i].owned.ToString();
                    }
                    owned -= objectList[stock[i].name].stackValue;
                }
            }
        }
    }

    // Use this for initialization
    void Start () {
        stock = FindObjectOfType<SavesManager>().main.stock;

        inventory = FindObjectOfType<SavesManager>().main.items;
        objectList = FindObjectOfType<ObjectDictionary>().list;
        active = display.activeSelf;
        lastSelected = "";
    }

    public void load(Inventory stock)
    {
        this.stock = stock;
    }

	//Update is called once per frame
	void Update ()
    {
        //On resetup le bouton qui doit etre selectionné par defaut avant la prochaine action.
        if (isActive() && EventSystem.current.currentSelectedGameObject == null && (contentStock.childCount > 0 || contentInventory.childCount > 0))
        {
            //Debug.Log(lastSelected + ";" + contentStock.childCount + ";" + contentInventory.childCount);
            if (lastSelected == "" && contentStock.childCount > 0)
            {
                EventSystem.current.SetSelectedGameObject(contentStock.GetChild(0).gameObject);
            }
            else if (lastSelected == "" && contentInventory.childCount > 0)
            {
                EventSystem.current.SetSelectedGameObject(contentInventory.GetChild(0).gameObject);
            }
            else if (lastSelected != "")
            {
                if (lastSelected.Contains("inventory"))
                {
                    EventSystem.current.SetSelectedGameObject(contentInventory.FindChild(lastSelected.Substring(10)).gameObject);
                }
                else if (lastSelected.Contains("stock"))
                {
                    EventSystem.current.SetSelectedGameObject(contentStock.FindChild(lastSelected.Substring(6)).gameObject);
                }
            }
        }
    }

    private void OnTriggerStay2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ranged = true;
        }
    }

    private void OnTriggerExit2D(Collider2D collision)
    {
        if (collision.tag == "Player")
        {
            ranged = false;
        }
    }
}
