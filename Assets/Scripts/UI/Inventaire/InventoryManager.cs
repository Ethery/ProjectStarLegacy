using UnityEngine;
using UnityEngine.EventSystems;
using System.Collections.Generic;
using UnityEngine.UI;
using System.IO;
using System.Xml.Serialization;
using System;

[RequireComponent(typeof(ObjectDictionary))]
public class InventoryManager : MonoBehaviour {

	public GameObject display;
	public int selected;
	
	public GameObject slotPrefab;
	public bool opened;

	public bool onFocus;

	[HideInInspector]
	public bool first = true;

	public Transform listeButton;
	public Transform description;
	public Transform useButton,deleteButton;
	
	[SerializeField]
	private Inventory _inventory;
	public List<PlayerWeapon> weaponList;
	public Dictionary<string, ObjectDictionaryItem> objectList;


	public void Start()
	{
		useButton = description.FindChild("UseButton");
		deleteButton = description.FindChild("DeleteButton");
		init();
		updateAffichage("After Init");
	}

	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown("Inventory") && !opened && Time.timeScale != 0f)
		{
			open();
		}
		else if (Input.GetButtonDown("Inventory") && opened)
		{
			close();
		}
		if (Input.GetButtonDown("Cancel") && opened && !onFocus )
		{
			close();
		}
		else if (Input.GetButtonDown("Cancel") && onFocus)
		{
			cancelFocus();
		}
	}


	public void init()
	{
		GetComponent<ObjectDictionary>().initList();
		objectList = GetComponent<ObjectDictionary>().list;
		weaponList = new List<PlayerWeapon>();
		_inventory = new Inventory();
		
		opened = display.activeSelf;

		foreach (KeyValuePair<string,ObjectDictionaryItem> item in FindObjectOfType<ObjectDictionary>().list)
		{
			if (item.Value.owned > 0)
			{
				_inventory.Add(new Item(item.Value.nom, item.Value.owned, item.Value.tags));
				if (item.Value.tags.Contains("Weapon"))
				{
					PlayerWeapon w = item.Value.GetComponent<PlayerWeapon>();
					List<Stat> l = new List<Stat>();
					l.Add(new Stat("damage", w.shotDamages));
					l.Add(new Stat("color.R", w.color.r));
					l.Add(new Stat("color.G", w.color.g));
					l.Add(new Stat("color.B", w.color.b));
					_inventory[_inventory.Count - 1].stats = l;
				}
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
		opened = true;
		display.SetActive(true);
		GameObject.Find("Player").GetComponent<PlayerInputManager>().canMove = false;
		EventSystem.current.SetSelectedGameObject(null);
		if (listeButton.childCount != 0)
		{
			EventSystem.current.SetSelectedGameObject(listeButton.GetChild(0).gameObject);
		}
		selected =0;
		goToFocus(false);
	}

	public void close()
	{
		Time.timeScale = 1f;
		opened = false;
		display.SetActive(false);
		GameObject.Find("Player").GetComponent<PlayerInputManager>().canMove = true;
	}

	public void useItem()
	{
		Item item = _inventory[int.Parse(listeButton.GetChild(selected).name)];
		if (objectList[item.name].GetComponent<Lootable>() != null)
		{
			objectList[item.name].GetComponent<Lootable>().use();
			deleteItem();
		}
		else
		{
			FindObjectOfType<PlayerWeaponManager>().changeWeapon(weaponList.IndexOf(objectList[item.name].GetComponent<PlayerWeapon>()));
		}
	}

	public void deleteItem()
	{
		Item item = _inventory[int.Parse(listeButton.GetChild(selected).name)];
		item.owned -= 1;
		if (item.owned == 0)
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
			updateAffichage("use last:"+listeButton.childCount);
		}
		else
		{
			updateAffichage("use not last");
		}
	}

	//Si l'item existe dans l'inventaire on en ajoute le nombre necessaire. Sinon on en crée le nombre necessaire.
	public void addItem(string name, int amount)
	{
		Item exists = null;
		foreach (Item item in _inventory)
		{
			if (item.name == name)
			{
				exists = item;
				break;
			}
		}
		
		if (exists != null)
		{
			exists.owned += amount;
		}
		else
		{
			Debug.Log(name + "::");
			_inventory.Add(new Item(objectList[name].name, amount, objectList[name].tags));
			if (objectList[name].GetComponent<PlayerWeapon>() != null)
			{
				_inventory[_inventory.Count - 1].stats.Add(new Stat("shotDamage", objectList[name].GetComponent<PlayerWeapon>().shotDamages));
				_inventory[_inventory.Count - 1].stats.Add(new Stat("Color.R", objectList[name].GetComponent<PlayerWeapon>().color.r));
				_inventory[_inventory.Count - 1].stats.Add(new Stat("Color.G", objectList[name].GetComponent<PlayerWeapon>().color.g));
				_inventory[_inventory.Count - 1].stats.Add(new Stat("Color.B", objectList[name].GetComponent<PlayerWeapon>().color.b));
			}
			if (objectList[name].GetComponent<Lootable>() != null)
			{
				_inventory[_inventory.Count - 1].stats.Add(new Stat("Heal Amount", objectList[name].GetComponent<Lootable>().healAmount));
			}
		}
		updateAffichage("Add item");
	}

	public void save(string fileName)
	{
		_inventory.save(fileName);
	}

	public Inventory save()
	{
		return _inventory;
	}

	public void load(string fileName)
	{
		_inventory = Inventory.load(fileName);
		if (first)
		{
			init();
			first = false;
		}
		updateAffichage("load inv w/ first = " + first);
		selected = 0;
		goToFocus(false);
	}

	public void load(Inventory n_inv)
	{
		_inventory = n_inv;
		if (first)
		{
			init();
			first = false;
		}
		updateAffichage("load inv w/ first = " + first);
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
			if (objectList.ContainsKey(_inventory[i].name))
			{
				if (_inventory[i].owned > objectList[_inventory[i].name].stackValue)
				{
					int j = 0;
					for (j = 0; j < Math.Ceiling((float)_inventory[i].owned / objectList[_inventory[i].name].stackValue) - 1; j++)
					{
						var slot = (Transform)Instantiate(slotPrefab.transform, listeButton, false);
						slot.name = i.ToString();
						slot.FindChild("Image").GetComponent<Image>().sprite = objectList[_inventory[i].name].GetComponent<SpriteRenderer>().sprite;
						slot.FindChild("Text").GetComponent<Text>().text = _inventory[i].name;
						slot.FindChild("Owned").GetComponent<Text>().text = (objectList[_inventory[i].name].stackValue).ToString();
					}
					var slot2 = (Transform)Instantiate(slotPrefab.transform, listeButton, false);
					slot2.name = i.ToString();
					slot2.FindChild("Image").GetComponent<Image>().sprite = objectList[_inventory[i].name].GetComponent<SpriteRenderer>().sprite;
					slot2.FindChild("Text").GetComponent<Text>().text = _inventory[i].name;
					slot2.FindChild("Owned").GetComponent<Text>().text = (_inventory[i].owned - (objectList[_inventory[i].name].stackValue * j)).ToString();
				}
				else
				{
					var slot = (Transform)Instantiate(slotPrefab.transform, listeButton, false);
					slot.name = i.ToString();
					slot.FindChild("Image").GetComponent<Image>().sprite = objectList[_inventory[i].name].GetComponent<SpriteRenderer>().sprite;
					slot.FindChild("Text").GetComponent<Text>().text = _inventory[i].name;
					slot.FindChild("Owned").GetComponent<Text>().text = _inventory[i].owned.ToString();
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
}

[Serializable]
[XmlRoot("Inventaire")]
public class Inventory : List<Item>
{
	public Inventory() : base()
	{
		//this.Add(new Item());
	}

	public void save(string fileName)
	{
		var serializer = new XmlSerializer(typeof(Inventory));
		var stream = new FileStream(Application.dataPath + "/"+fileName, FileMode.Create);

		serializer.Serialize(stream, this);
		stream.Close();
	}

	public static Inventory load(string fileName)
	{
		var serializer = new XmlSerializer(typeof(Inventory));
		var stream = new FileStream(Application.dataPath + "/" + fileName, FileMode.Open);
		var container = serializer.Deserialize(stream) as Inventory;
		stream.Close();
		return container;
	}
}

public class Item
{
	public string name, description;
	public int owned;
	public List<string> tags;
	public List<Stat> stats;

	public Item()
	{
		name = "Item";
		description = "description";
		tags = new List<string>();
		stats = new List<Stat>();
		owned = 1;
		description = "";
	}

	public Item(string nname,int nd, List<string> tag)
	{
		name = nname;
		tags = tag;
		stats = new List<Stat>();
		owned = nd;
		description = "Description de " + name + ". CECI EST UN EXEMPLE ENCULÉ !";
	}
}

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
