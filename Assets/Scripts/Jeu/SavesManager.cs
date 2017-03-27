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
	
	private Progression _prog;
	private MainStatus _main;
	public List<CheckPoint> checkpoints;

	public bool canSave;
	

	// Use this for initialization
	void Start () {
		_im = GameObject.FindObjectOfType<InventoryManager>();
		_im.init();
		_player = GameObject.Find("Player").GetComponent<HealthBar>();
		_prog = new Progression();
		_main = new MainStatus();
		switch (PlayerPrefs.GetInt("SessionID", 0))
		{
			case 0:
				canSave = true;
				saveAll(PlayerPrefs.GetInt("SessionID", 0));
                canSave = false;
				break;
			default:
                loadAll(PlayerPrefs.GetInt("SessionID", 0));
                break;
		}
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
			_prog.save(_player.save(), _im.save(), "Saves/save" + saveNb + "/" + SceneManager.GetActiveScene().name + "/All.etheremos");

			_main.lastVisitedLevel = SceneManager.GetActiveScene().name;
			_main.save("Saves/save" + saveNb + "/main.etheremos");
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
			_main = MainStatus.load("Saves/save" + saveNb + "/main.etheremos");
			_prog = Progression.load("Saves/save" + saveNb + "/" + SceneManager.GetActiveScene().name + "/All.etheremos");
			_im.load(_prog.items);
			_player.load(_prog.health);

            Debug.Log("LOAD:"+checkpoints[0]);
			CheckPoint tmp = checkpoints[0];
			foreach (CheckPoint cp in checkpoints)
			{
				if (_prog.checkPointId.Contains(cp.id))
				{
					cp.activated = true;
				}
				if (_prog.checkPointId.Count >= 1)
				{
					if (_prog.checkPointId[_prog.checkPointId.Count - 1] == cp.id)
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
		if (_prog.checkPointId.Contains(id))
		{
			_prog.checkPointId.Remove(id);
		}
		_prog.checkPointId.Add(id);
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

	public int ended;

	public Progression()
	{
		health = new Health();
		items = new Inventory();
		checkPointId = new List<string>();
		ended = 0;
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

	public void save(Health h, Inventory i,string fileName)
	{
		health = h;
		items = i;
		var serializer = new XmlSerializer(typeof(Progression));

		SavesManager.EnsureFolder(Application.dataPath + "/" + fileName);
		var stream = new FileStream(Application.dataPath + "/" + fileName, FileMode.Create);

		serializer.Serialize(stream, this);
		stream.Close();
		
	}
}


[XmlRoot("MainStatus")]
public class MainStatus
{
	public string lastVisitedLevel;
    public List<LevelStats> levels;

	public MainStatus()
	{
		lastVisitedLevel = "";
        levels = new List<LevelStats>();
        for(int i = 0;i<SceneManager.sceneCount;i++)
        {
            levels.Add(new LevelStats(SceneManager.GetSceneAt(i).name, false));
        }
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
		var stream = new FileStream(Application.dataPath + "/" + fileName, FileMode.Create);

		serializer.Serialize(stream, this);
		stream.Close();

	}
}

public class LevelStats
{
    public string nom;
    public bool finished;

    public LevelStats()
    {
        nom = "";
        finished = false;
    }

    public LevelStats(string nNom, bool nFinished)
    {
        nom = nNom;
        finished = nFinished;
    }
}