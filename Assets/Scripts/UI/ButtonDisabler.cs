using UnityEngine;
using UnityEngine.UI;

public class ButtonDisabler : MonoBehaviour {

	public bool loadEverywhere;


	SavesManager _sm;
	private void Start()
	{
		_sm = GameObject.Find("_GameManager").GetComponent<SavesManager>();
	}
	// Update is called once per frame
	void Update () {
		GetComponent<Button>().interactable = _sm.canSave || loadEverywhere;
	}
}
