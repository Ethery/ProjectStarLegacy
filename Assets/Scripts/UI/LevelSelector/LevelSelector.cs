using UnityEngine;
using System.IO;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class LevelSelector : MonoBehaviour {

	public Transform buttonPrefab;
	public Transform canvas;

	private Fading fade;

	private MainStatus _status;

	// Use this for initialization
	void Start () {
		fade = FindObjectOfType<Fading>();
		if (Directory.Exists(Application.dataPath + "/Saves"))
		{
			foreach (string dir in Directory.GetDirectories(Application.dataPath + "/Saves"))
			{
				var button = Instantiate<Transform>(buttonPrefab,canvas);

				string last = dir.Replace(Application.dataPath + "/Saves\\","");

				button.GetComponentInChildren<Text>().text = last;
				button.name = last;
			}
			if (canvas.childCount > 0)
			{
				EventSystem.current.SetSelectedGameObject(canvas.GetChild(0).gameObject);
			}
		}
	}

	public void loadSave(string id)
	{
		Debug.Log("Loading " + id+":"+ id.Replace("save", ""));
		_status = MainStatus.load("Saves/" + id + "/main.etheremos");
		PlayerPrefs.SetInt("SessionID", int.Parse(id.Replace("save", "")));
		Debug.Log(PlayerPrefs.GetInt("SessionID") + ":"+_status.lastVisitedLevel);
		fade.FadeTo(_status.lastVisitedLevel);
	}

	public void back()
	{
		fade.FadeTo("Acceuil");
	}
}
