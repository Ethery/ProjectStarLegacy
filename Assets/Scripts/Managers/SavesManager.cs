using UnityEngine;
using System.Xml.Serialization;
using System;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine.SceneManagement;
using System.Xml;
using System.Text;
using System.IO;

public class SavesManager : MonoBehaviour {

	private InventoryManager _im;
	private HealthBar _player;
    private Stockage _stock;

	public Progression prog;
	public MainStatus main;
	private CheckPoint[] checkpoints;

	public bool canSave;

    public string OBJECTIF_COURANT = "Les développeurs sont un peut en retard du coup pas d'objectifs pour l'instant mais vous pouvez explorer le monde en toute liberté.";
	
	// Use this for initialization
	void Start () {
		checkpoints = FindObjectOfType<CheckPoint>().transform.parent.GetComponentsInChildren<CheckPoint>();
		_im = FindObjectOfType<InventoryManager>();
        _stock = FindObjectOfType<Stockage>();
		_im.init();
		_player = GameObject.Find("Player").GetComponent<HealthBar>();
		switch (PlayerPrefs.GetInt("SessionID", 0))
		{
			case 0:
				main = new MainStatus();
				main.init();
				saveAll(PlayerPrefs.GetInt("SessionID", 0),true);
				break;
			default:
                if (!File.Exists(Application.dataPath + "/Saves/save" + PlayerPrefs.GetInt("SessionID", 0) + "/" + SceneManager.GetActiveScene().name + "/All.etheremos"))
                {
                    main = MainStatus.load("Saves/save" + PlayerPrefs.GetInt("SessionID", 0) + "/main.etheremos");
                    saveAll(PlayerPrefs.GetInt("SessionID", 0),true);
                }
				loadAll(PlayerPrefs.GetInt("SessionID", 0));
				break;
		}
	}

    void Update()
    {

    }

    public void saveAll(int saveNb)
    {
        if (canSave)
        {
            if (saveNb == 0)
            {
                if (PlayerPrefs.HasKey("BiggestSave"))
                {
                    PlayerPrefs.SetInt("BiggestSave", PlayerPrefs.GetInt("BiggestSave") + 1);
                }
                else
                {
                    PlayerPrefs.SetInt("BiggestSave", 1);
                }
                saveNb = PlayerPrefs.GetInt("BiggestSave");

                _player.transform.position = checkpoints[0].transform.position;
            }
            prog.health = _player.save();
            prog.save("Saves/save" + saveNb + "/" + SceneManager.GetActiveScene().name + "/All.etheremos");
            main.items = _im.getInventory();
            if (_stock != null)
            {
                main.stock = _stock.getStock();
            }
            main.lastVisitedLevel = SceneManager.GetActiveScene().name;
            main.save("Saves/save" + saveNb + "/main.etheremos");
            PlayerPrefs.SetInt("SessionID", saveNb);
            PlayerPrefs.SetInt("LastSessionID", saveNb);
            PlayerPrefs.Save();
            return;
        }
    }

    public void saveAll(int saveNb,bool force_save)
    {
        if (canSave || force_save)
        {
            if (saveNb == 0)
            {
                if (PlayerPrefs.HasKey("BiggestSave"))
                {
                    PlayerPrefs.SetInt("BiggestSave", PlayerPrefs.GetInt("BiggestSave") + 1);
                }
                else
                {
                    PlayerPrefs.SetInt("BiggestSave", 1);
                }
                saveNb = PlayerPrefs.GetInt("BiggestSave");

                _player.transform.position = checkpoints[0].transform.position;
            }
            prog.health = _player.save();
            prog.save("Saves/save" + saveNb + "/" + SceneManager.GetActiveScene().name + "/All.etheremos");
            main.items = _im.getInventory();
            if (_stock != null)
            {
                main.stock = _stock.getStock();
            }
            main.lastVisitedLevel = SceneManager.GetActiveScene().name;
            foreach (TalkingManager item in FindObjectsOfType<TalkingManager>())
            {
                AvisPNJ avisTmp = main.avis.Find(((e) => 
                {
                    return (e.name == item.name);
                }));
                if(item.avis != null&&avisTmp != null)
                {
                    avisTmp = item.avis;
                }
                if(item.avis != null && avisTmp ==null)
                {
                    main.avis.Add(item.avis);
                }
            }
            main.save("Saves/save" + saveNb + "/main.etheremos");
            PlayerPrefs.SetInt("SessionID", saveNb);
            PlayerPrefs.SetInt("LastSessionID", saveNb);
            PlayerPrefs.Save();
            return;
        }
    }

    public void addLevelObjects(Parameter obj)
    {
        if (prog.levelObjects.Contains(obj))
        {
            prog.levelObjects.Remove(obj);
        }
        prog.levelObjects.Add(obj);
    }
    public void addLevelObjects(List<Parameter> obj)
    {
        foreach (Parameter o in obj)
        {
            if (prog.levelObjects.Contains(o))
            {
                prog.levelObjects.Remove(o);
            }
            prog.levelObjects.Add(o);
        }
    }

    public void saveStockInventory(Inventory stock, Inventory inventory)
    {
        main.items = inventory;
        main.stock = stock;
        main.save("Saves/save" + PlayerPrefs.GetInt("SessionID", 0) + "/main.etheremos");
    }

    public void saveInventory(Inventory inventory)
    {
        main.items = inventory;
        main.save("Saves/save" + PlayerPrefs.GetInt("SessionID", 0) + "/main.etheremos");
    }

    public void loadAll(int saveNb)
	{
		if (Directory.Exists(Application.dataPath + "/Saves"))
		{
			main = MainStatus.load("Saves/save" + saveNb + "/main.etheremos");
			
			prog = Progression.load("Saves/save" + saveNb + "/" + SceneManager.GetActiveScene().name + "/All.etheremos");
			_im.load(main.items);
            if (_stock != null)
            {
                _stock.load(main.stock);
            }

			_player.load(prog.health);
			
			CheckPoint tmp = checkpoints[0];
			foreach (CheckPoint cp in checkpoints)
			{
				if (prog.checkPointId.Contains(cp.id))
				{
					cp.activated = true;
				}
				if (prog.checkPointId.Count >= 1)
				{
					if (prog.checkPointId[prog.checkPointId.Count - 1] == cp.id)
					{
						tmp = cp;
					}
				}
			}

            /*
			string str = "";
			for (int i = 0; i < prog.checkPointId.Count; i++)
			{
				str += prog.checkPointId[i] + ":";
			}
			Debug.Log("cp:" + str);*/
			_player.transform.position = tmp.transform.position;
		}
	}

	public void setLastCheckpoint(string id)
	{
		if (prog.checkPointId.Contains(id))
		{
			prog.checkPointId.Remove(id);
		}
		prog.checkPointId.Add(id);
	}

	public void finished()
	{
		main.levelFinished();
		prog.reset();
		saveAll(PlayerPrefs.GetInt("SessionID", 0));
	}
    
	public static void EnsureFolder(string path)
	{
		string directoryName = Path.GetDirectoryName(path);
		if ((directoryName.Length > 0) && (!Directory.Exists(directoryName)))
		{
			Directory.CreateDirectory(directoryName);
		}
	}
}

[Serializable]
[XmlRoot("PlayerStatus")]
public class Progression
{
	public List<string> checkPointId;

	[XmlElement("HealthStatus")]
	public Health health;

    public List<Parameter> levelObjects;

	public Progression()
	{
		health = new Health();
		checkPointId = new List<string>();
        levelObjects = new List<Parameter>();
	}

	public void reset()
	{
		health = new Health();
		checkPointId = new List<string>();
        levelObjects = new List<Parameter>();
    }

	public static Progression load(string fileName)
	{
		if (File.Exists(Application.dataPath + "/" + fileName))
		{
			var serializer = new XmlSerializer(typeof(Progression));
			var stream = new FileStream(Application.dataPath + "/" + fileName, FileMode.Open);
			var container = serializer.Deserialize(stream) as Progression;
			stream.Close();
			return container;
		}
		return new Progression();
	}

	public void save(string fileName)
	{
		var serializer = new XmlSerializer(typeof(Progression));

		SavesManager.EnsureFolder(Application.dataPath + "/" + fileName);
		var encoding = Encoding.GetEncoding("UTF-8");
		var stream = new StreamWriter(Application.dataPath + "/" + fileName, false, encoding);

		serializer.Serialize(stream, this);
		stream.Close();
	}
}


[XmlRoot("MainStatus")]
[Serializable]
public class MainStatus
{
	public string lastVisitedLevel;
    public List<LevelStats> levels;
    public List<AvisPNJ> avis;
    [XmlArray(ElementName = "Stock", IsNullable = false, Order = 1)]
    [XmlArrayItem("Item")]
    public Inventory stock;
    [XmlArray(ElementName = "Inventory", IsNullable = false, Order = 2)]
    [XmlArrayItem("Item")]
    public Inventory items;

    public MainStatus()
	{
		lastVisitedLevel = "";
		levels = new List<LevelStats>();
        avis = new List<AvisPNJ>();
        items = new Inventory();
        stock = new Inventory();
    }

	public void init()
	{
		for (int i = 0; i < SceneManager.sceneCountInBuildSettings; i++)
		{
			levels.Add(new LevelStats(i, SceneManager.GetSceneByBuildIndex(i).name, false));
		}
	}

	public void levelFinished()
	{
		foreach (LevelStats lvl in levels)
		{
			if (lvl.nom == SceneManager.GetActiveScene().name)
			{
				lvl.finished = true;
				lastVisitedLevel = "Navette";
				break;
			}
		}
	}

    public Dictionary<object,object> getAvisAsDictionnary()
    {
        Dictionary<object, object> dic = new Dictionary<object, object>();
        foreach (AvisPNJ item in avis)
        {
            dic.Add(item.name, item.avis);
        }
        return dic;
    }

	public static MainStatus load(string fileName)
	{
		var serializer = new XmlSerializer(typeof(MainStatus));
		var stream = new FileStream(Application.dataPath + "/" + fileName, FileMode.Open);
		var container = serializer.Deserialize(stream) as MainStatus;
		stream.Close();
		return container;
	}

	public void save(string fileName)
    {
        var serializer = new XmlSerializer(typeof(MainStatus));

		SavesManager.EnsureFolder(Application.dataPath + "/" + fileName);
		var encoding = Encoding.GetEncoding("UTF-8");
		var stream = new StreamWriter(Application.dataPath + "/" + fileName, false, encoding);

		serializer.Serialize(stream, this);
		stream.Close();
	}
}

public class LevelStats
{
    public int id;
	public string nom;
    public bool finished;

    public LevelStats()
    {
		id = -1;
		nom = "Non Visit�";
        finished = false;
    }

    public LevelStats(int nid,String nNom, bool nFinished)
    {
		id = nid;
		if (nNom == null)
			nom = "";
		else
			nom = nNom;
        finished = nFinished;
    }
}