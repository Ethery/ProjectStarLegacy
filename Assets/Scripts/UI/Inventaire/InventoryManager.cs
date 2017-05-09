using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System.Xml.Serialization;
using System;

public class InventoryManager : Interactable {

	public GameObject display;
	public int selected;
	
	public GameObject slotPrefab;

	public bool onFocus;
    

	public Transform listeButton;
	public Transform description;
	public Transform useButton,deleteButton;
	
	[SerializeField]
	private Inventory _inventory;
	public List<PlayerWeapon> weaponList;
	public Dictionary<string, ObjectDictionaryItem> objectList;

    public Inventory getInventory()
    {
        return _inventory;
    }
    
	public void Start()
	{
		useButton = description.FindChild("UseButton");
		deleteButton = description.FindChild("DeleteButton");

        FindObjectOfType<ObjectDictionary>().initList();
        objectList = FindObjectOfType<ObjectDictionary>().list;

        weaponList = new List<PlayerWeapon>();
        _inventory = FindObjectOfType<SavesManager>().main.items;

        active = display.activeSelf;
        Activate(active);
        int i = 0;
        foreach (Item it in _inventory)
        {
            if (objectList[it.name].GetComponent<PlayerWeapon>() != null)
            {
                _inventory[i].stats.Add(new Stat("shotDamage", objectList[it.name].GetComponent<PlayerWeapon>().shotDamages));
                _inventory[i].stats.Add(new Stat("Color.R", objectList[it.name].GetComponent<PlayerWeapon>().color.r));
                _inventory[i].stats.Add(new Stat("Color.G", objectList[it.name].GetComponent<PlayerWeapon>().color.g));
                _inventory[i].stats.Add(new Stat("Color.B", objectList[it.name].GetComponent<PlayerWeapon>().color.b));
                weaponList.Add(objectList[_inventory[i].name].GetComponent<PlayerWeapon>());
            }
            if (objectList[it.name].GetComponent<Lootable>() != null)
            {
                _inventory[i].stats.Add(new Stat("Heal Amount", objectList[it.name].GetComponent<Lootable>().healAmount));
            }
            i++;
        }
        updateAffichage("After Init");
	}
    
    
	public override void init()
	{
		objectList = FindObjectOfType<ObjectDictionary>().list;
		weaponList = new List<PlayerWeapon>();
		_inventory = new Inventory();
		
		active = display.activeSelf;
        Activate(active);

		foreach (KeyValuePair<string,ObjectDictionaryItem> item in FindObjectOfType<ObjectDictionary>().list)
		{
			if (item.Value.owned > 0)
            {
                List<Stat> l = new List<Stat>();
                if (item.Value.tags.Contains("Weapon"))
				{
					PlayerWeapon w = item.Value.GetComponent<PlayerWeapon>();
					l.Add(new Stat("damage", w.shotDamages));
					l.Add(new Stat("color.R", w.color.r));
					l.Add(new Stat("color.G", w.color.g));
					l.Add(new Stat("color.B", w.color.b));
					_inventory[_inventory.Count - 1].stats = l;
                }
                _inventory.Add(new Item(item.Value.nom, item.Value.owned, item.Value.tags,l,item.Value.description));
            }
		}

		for (int i = 0; i < _inventory.Count; i++)
		{
			if (_inventory[i].tags.Contains("Weapon"))
			{
				weaponList.Add(objectList[_inventory[i].name].GetComponent<PlayerWeapon>());
			}
		}
        
	}

	public void cancelFocus()
	{
		if (listeButton.childCount > 0)
		{
			EventSystem.current.SetSelectedGameObject(listeButton.GetChild(selected).gameObject);
		}
		onFocus = false;
	}

	public void goToFocus(bool toUse)
	{
		if (_inventory.Count != 0)
		{
			Item item = _inventory[int.Parse(listeButton.GetChild(selected).name)];
			description.FindChild("Image").GetComponent<Image>().sprite = objectList[item.name].GetComponent<SpriteRenderer>().sprite;
			description.FindChild("Nom").GetComponentInChildren<Text>().text = item.name;
			description.FindChild("Description").GetComponentInChildren<Text>().text = item.description;
			description.FindChild("Nombre").GetComponentInChildren<Text>().text = item.owned.ToString();
			useButton.GetComponentInChildren<Button>().interactable = true;
			deleteButton.GetComponentInChildren<Button>().interactable = true;
			if (objectList[item.name].GetComponent<Lootable>() == null)
			{
				useButton.GetComponentInChildren<Text>().text = "Equip";
			}
			else
			{
				useButton.GetComponentInChildren<Text>().text = "Use";
			}
			if (toUse)
			{
				EventSystem.current.SetSelectedGameObject(GameObject.Find("UseButton"));
				onFocus = true;
			}
		}
		else
		{
			description.FindChild("Nom").GetComponentInChildren<Text>().text = "";
			description.FindChild("Description").GetComponentInChildren<Text>().text = "";
			description.FindChild("Nombre").GetComponentInChildren<Text>().text = "";
			useButton.GetComponentInChildren<Button>().interactable = false;
			deleteButton.GetComponentInChildren<Button>().interactable = false;
		}
	}

	public void open()
	{
		Time.timeScale = 0f;
		active = true;
		display.SetActive(true);
		GameObject.Find("Player").GetComponent<PlayerInputManager>().canMove = false;
		EventSystem.current.SetSelectedGameObject(null);
		if (listeButton.childCount != 0)
		{
			EventSystem.current.SetSelectedGameObject(listeButton.GetChild(0).gameObject);
		}
		selected =0;
		goToFocus(false);
        updateAffichage("open");
	}

	public void close()
	{
		Time.timeScale = 1f;
		active = false;
		display.SetActive(false);
		GameObject.Find("Player").GetComponent<PlayerInputManager>().canMove = true;
	}

	public void useItem()
	{
		Item item = _inventory[int.Parse(listeButton.GetChild(selected).name)];
		if (objectList[item.name].GetComponent<Lootable>() != null)
		{
			objectList[item.name].GetComponent<Lootable>().use();
			deleteItemOnce();
		}
		else
		{
			FindObjectOfType<PlayerWeaponManager>().changeWeapon(weaponList.IndexOf(objectList[item.name].GetComponent<PlayerWeapon>()));
		}
	}

    public void deleteItemOnce()
    {
        Item item = _inventory[int.Parse(listeButton.GetChild(selected).name)];
        item.owned -= 1;
        if (item.owned <= 0)
        {
            _inventory.Remove(item);
            if (selected > 0)
            {
                selected -= 1;
            }
            EventSystem.current.SetSelectedGameObject(null);
            if (listeButton.childCount != 0)
            {
                EventSystem.current.SetSelectedGameObject(listeButton.GetChild(selected).gameObject);
            }
            else
            {
                EventSystem.current.SetSelectedGameObject(useButton.gameObject);
            }
            updateAffichage("delete once last:" + listeButton.childCount);
        }
        else
        {
            updateAffichage("delete once");
        }
    }

    public void deleteItemAll(string nom)
    {
        Item exists = null;
        foreach (Item itemOnce in _inventory)
        {
            if (itemOnce.name == nom)
            {
                exists = itemOnce;
                break;
            }
        }

        if (exists != null)
        {
            _inventory.Remove(exists);
        }
        if (selected > 0)
        {
            selected -= 1;
        }
        EventSystem.current.SetSelectedGameObject(null);
        if (listeButton.childCount != 0)
        {
            EventSystem.current.SetSelectedGameObject(listeButton.GetChild(selected).gameObject);
        }
        else
        {
            EventSystem.current.SetSelectedGameObject(useButton.gameObject);
        }
        updateAffichage("Item Deleted");
    }

    //Si l'item existe dans l'inventaire on en ajoute le nombre necessaire. Sinon on en crée le nombre necessaire.
    public void addItem(ItemShort item)
	{
		Item exists = null;
		foreach (Item itemOnce in _inventory)
		{
			if (itemOnce.name == item.name)
			{
				exists = itemOnce;
				break;
			}
		}
		
		if (exists != null)
		{
			exists.owned += item.owned;
		}
		else
		{
            List<Stat> stats = new List<Stat>();
            if (objectList.ContainsKey(item.name))
            {
                if (objectList[item.name].GetComponent<PlayerWeapon>() != null)
                {
                    stats.Add(new Stat("shotDamage", objectList[item.name].GetComponent<PlayerWeapon>().shotDamages));
                    stats.Add(new Stat("Color.R", objectList[item.name].GetComponent<PlayerWeapon>().color.r));
                    stats.Add(new Stat("Color.G", objectList[item.name].GetComponent<PlayerWeapon>().color.g));
                    stats.Add(new Stat("Color.B", objectList[item.name].GetComponent<PlayerWeapon>().color.b));
                    weaponList.Add(objectList[item.name].GetComponent<PlayerWeapon>());
                }
                if (objectList[item.name].GetComponent<Lootable>() != null)
                {
                    stats.Add(new Stat("Heal Amount", objectList[item.name].GetComponent<Lootable>().healAmount));
                }

                _inventory.Add(new Item(objectList[item.name].nom, item.owned, objectList[item.name].tags, stats, objectList[item.name].description));
            }
            else
            {
                Debug.LogWarning("Le dictionnaire ne contient pas l'item \"" + item.name + "\"");
            }
        }
		updateAffichage("Add item");
	}

    public void setItemCount(ItemShort item)
    {
        if (item.owned == 0)
        {
            deleteItemAll(item.name);
            return;
        }
        Item exists = null;
        foreach (Item itemOnce in _inventory)
        {
            if (itemOnce.name == item.name)
            {
                exists = itemOnce;
                break;
            }
        }

        if (exists != null)
        {
            exists.owned = item.owned;
        }
        else
        {
            List<Stat> l = new List<Stat>();
            if (objectList[item.name].GetComponent<PlayerWeapon>() != null)
            {
                l.Add(new Stat("shotDamage", objectList[item.name].GetComponent<PlayerWeapon>().shotDamages));
                l.Add(new Stat("Color.R", objectList[item.name].GetComponent<PlayerWeapon>().color.r));
                l.Add(new Stat("Color.G", objectList[item.name].GetComponent<PlayerWeapon>().color.g));
                l.Add(new Stat("Color.B", objectList[item.name].GetComponent<PlayerWeapon>().color.b));
                weaponList.Add(objectList[item.name].GetComponent<PlayerWeapon>());
            }
            if (objectList[item.name].GetComponent<Lootable>() != null)
            {
                l.Add(new Stat("Heal Amount", objectList[item.name].GetComponent<Lootable>().healAmount));
            }
            _inventory.Add(new Item(objectList[item.name].nom, item.owned, objectList[item.name].tags,l, objectList[item.name].description));
        }
        updateAffichage("setItemCount");
    }
    
	public void load(Inventory n_inv)
	{
		_inventory = n_inv;
		init();
		selected = 0;
	}

	private void updateAffichage(string from)
	{
		//Debug.Log("Update from "+from);
		for (int i = 0; i < listeButton.childCount; i++)
		{
			Destroy(listeButton.GetChild(i).gameObject);
		}
		for (int i = 0; i < _inventory.Count; i++)
        {
            if (objectList.ContainsKey(_inventory[i].name) && _inventory[i].owned > 0)
			{
                int owned = _inventory[i].owned;
                for (int j = 0; j < Math.Ceiling((float)_inventory[i].owned / objectList[_inventory[i].name].stackValue); j++)
				{
					var slot = (Transform)Instantiate(slotPrefab.transform, listeButton, false);
					slot.name = i.ToString();
					slot.FindChild("Image").GetComponent<Image>().sprite = objectList[_inventory[i].name].GetComponent<SpriteRenderer>().sprite;
					slot.FindChild("Text").GetComponent<Text>().text = _inventory[i].name;
                    if (owned > objectList[_inventory[i].name].stackValue)
                    {
                        slot.FindChild("Owned").GetComponent<Text>().text = (objectList[_inventory[i].name].stackValue).ToString();
                    }
                    else
                    {
                        slot.FindChild("Owned").GetComponent<Text>().text = (owned).ToString();
                    }
                    owned -= objectList[_inventory[i].name].stackValue;
                }
			}
		}
		if (_inventory.Count != 0)
		{
			Item item = _inventory[int.Parse(listeButton.GetChild(selected).name)];
			description.FindChild("Image").GetComponent<Image>().sprite = objectList[item.name].GetComponent<SpriteRenderer>().sprite;
			description.FindChild("Nom").GetComponentInChildren<Text>().text = item.name;
			description.FindChild("Description").GetComponentInChildren<Text>().text = item.description;
			description.FindChild("Nombre").GetComponentInChildren<Text>().text = _inventory[int.Parse(listeButton.GetChild(selected).name)].owned.ToString();
		}
		else
		{
			description.FindChild("Image").GetComponent<Image>().sprite = null;
			description.FindChild("Nom").GetComponentInChildren<Text>().text = "";
			description.FindChild("Description").GetComponentInChildren<Text>().text = "";
			description.FindChild("Nombre").GetComponentInChildren<Text>().text = "";
		}
	}

    public override bool Activate(bool a, string key)
    {
        if (key=="Inventory" && a)
        {
            open();
            return true;
        }
        if (key == "Inventory" && !a)
        {
            close();
            return true;
        }
        if (key == "Cancel" && !onFocus)
        {
            close();
            return true;
        }
        if (key == "Cancel" && onFocus)
        {
            cancelFocus();
            return true;
        }
        return false;
    }

    public override void Activate(bool a)
    {
        if (a && !active)
        {
            open();
        }
        if (active && !a)
        {
            close();
        }
        if (active && !onFocus)
        {
            close();
        }
        if (onFocus)
        {
            cancelFocus();
        }
    }

    public override bool nextStep(string key)
    {
        return true;
    }
}

[Serializable]
public class Inventory : List<Item>
{
    public Inventory() : base()
	{
		
	}
}

[Serializable]
public class Item : ItemShort
{
	public string description;
	public List<string> tags;
	public List<Stat> stats;

	public Item():base()
	{
		name = "Item";
		description = "description";
		tags = new List<string>();
		stats = new List<Stat>();
		owned = 1;
		description = "";
	}


    public Item(string nname, int nd, List<string> tag, List<Stat> nstat, string desc) : base(nname, nd)
    {
        tags = tag;
        stats = nstat;
        description = desc;
    }

    public Item(Item item) : base(item.name,item.owned)
    {
        tags = item.tags;
        stats = item.stats;
        description = item.description;
    }


}

[Serializable]
public class ItemShort
{
    public string name;
    public int owned;

    public ItemShort()
    {
        name = "Item";
        owned = 1;
    }

    public ItemShort(string nname, int nd)
    {
        name = nname;
        owned = nd;
    }
}

[Serializable]
public class Stat
{
	public string name;
	public float value;

	public Stat()
	{
		name = "";
		value = -1f;
	}

	public Stat(string a, float b)
	{
		name = a;
		value = b;
	}
}
