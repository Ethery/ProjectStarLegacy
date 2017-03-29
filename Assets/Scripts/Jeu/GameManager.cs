using UnityEngine;
using UnityEngine.EventSystems;

public class GameManager : MonoBehaviour {

    public GameObject display;
	public bool isPaused;
	public Fading fadePrefab;
	private InventoryManager _inventoryManager;
	private CharacterManager _player;
	private SavesManager _saves;
	private Fading fadeObject;
    public bool buttonReleased = true;


	// Use this for initialization
	void Start () {
		_saves = GetComponent<SavesManager>();
		fadeObject = FindObjectOfType<Fading>();
		if (fadeObject == null)
		{
			fadeObject = Instantiate(fadePrefab);
		}
		pause(display.activeSelf);
	}
	
	// Update is called once per frame
	void Update ()
	{
		if (Input.GetButtonDown("Pause"))
		{
			pause(!isPaused);
			if(isPaused)
				EventSystem.current.SetSelectedGameObject(display.transform.FindChild("ResumeButton").gameObject);
            buttonReleased = false;
		}
		if (FindObjectOfType<TextConvManager>().isActive)
		{
			buttonReleased = false;
		}
        if(Input.GetButtonUp("Pause"))
        {
            buttonReleased = true;
        }
		if (buttonReleased && isPaused && Input.GetButtonDown("Cancel"))
		{
			pause(false);
		}
	}

	void pause(bool on)
	{
		if (on)
		{
			if (Time.timeScale != 0f)
			{
				Time.timeScale = 0f;
				isPaused = on;
				display.SetActive(isPaused);
			}
		}
		else
		{
			isPaused = on;
			display.SetActive(isPaused);
			Time.timeScale = 1f;
		}
	}

	public void resume()
	{
		pause(false);
	}

	public void options()
	{
	
	}

	public void quit(bool save)
	{
		if (save)
		{
			_saves.saveAll(PlayerPrefs.GetInt("SessionID", 0));
		}
		fadeObject.FadeTo("Acceuil");
	}
}
