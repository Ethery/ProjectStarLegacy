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

	public Progression prog;
	public MainStatus main;
	private CheckPoint[] checkpoints;

	public bool canSave;

    public string OBJECTIF_COURANT = "Les développeurs sont un peut en retard du coup pas d'objectifs pour l'instant mais vous pouvez explorer le monde en toute liberté.";
	
	// Use this for initialization
	void Start () {
		checkpoints = FindObjectOfType<CheckPoint>().transform.parent.GetComponentsInChildren<CheckPoint>();
		_im = GameObject.FindObjectOfType<InventoryManager>();
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
                if (!File.Exists(Application.dataPath + "Saves/save" + PlayerPrefs.GetInt("SessionID", 0) + "/" + SceneManager.GetActiveScene().name + "/All.etheremos"))
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
        /*Debug.Log("HEALTH:" + prog.health.totalPv);
        */
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
            prog.save(_player.save(), _im.save(), "Saves/save" + saveNb + "/" + SceneManager.GetActiveScene().name + "/All.etheremos");

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
            prog.save(_player.save(), _im.save(), "Saves/save" + saveNb + "/" + SceneManager.GetActiveScene().name + "/All.etheremos");
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

    public void loadAll(int saveNb)
	{
		if (Directory.Exists(Application.dataPath + "/Saves"))
		{
			main = MainStatus.load("Saves/save" + saveNb + "/main.etheremos");

            /*foreach (TalkingManager item in FindObjectsOfType<TalkingManager>())
            {
                AvisPNJ avisTmp = main.avis.Find(((e) => 
                {
                    return (e.name == item.name);
                }));
                if(avisTmp != null)
                {
                    item.avis = avisTmp;
                }
            }*/

			prog = Progression.load("Saves/save" + saveNb + "/" + SceneManager.GetActiveScene().name + "/All.etheremos");
			_im.load(prog.items);
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

	[XmlArray("InventoryStatus")]
	public Inventory items;
	

	public Progression()
	{
		health = new Health();
		items = new Inventory();
		checkPointId = new List<string>();
	}

	public void reset()
	{
		health = new Health();
		items = new Inventory();
		checkPointId = new List<string>();
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

	public void save(Health h, Inventory inv,string fileName)
	{
		health = h;
		items = inv;
		var serializer = new XmlSerializer(typeof(Progression));

		SavesManager.EnsureFolder(Application.dataPath + "/" + fileName);
		var encoding = Encoding.GetEncoding("UTF-8");
		var stream = new StreamWriter(Application.dataPath + "/" + fileName, false, encoding);

		serializer.Serialize(stream, this);
		stream.Close();
		
	}
}


[XmlRoot("MainStatus")]
public class MainStatus
{
	public string lastVisitedLevel;
    public List<LevelStats> levels;
    public List<AvisPNJ> avis;

	public MainStatus()
	{
		lastVisitedLevel = "";
		levels = new List<LevelStats>();
        avis = new List<AvisPNJ>();
	}

	public void init()
	{
		levels = new List<LevelStats>();
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