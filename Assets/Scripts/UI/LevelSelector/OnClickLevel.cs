using UnityEngine;
using UnityEngine.UI;

public class OnClickLevel : MonoBehaviour {

	public void onClick()
	{
		LevelSelector ls = FindObjectOfType<LevelSelector>();
		ls.loadSave(transform.GetComponentInChildren<Text>().text);
	}
}
