using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System.Collections;

public class MainMenu : MonoBehaviour
{
    private Fading fadeObject;
	public Button continueButton;
	private bool choiceDone =false;

	public void Start()
	{
		Cursor.lockState = CursorLockMode.Locked;
		fadeObject = FindObjectOfType<Fading>();
		if (continueButton != null)
		{
			continueButton.transform.parent.FindChild("Options").GetComponent<Button>().interactable = false;
		}
	}

    public void Update()
    {
        if (continueButton != null)
        {
            continueButton.interactable = PlayerPrefs.HasKey("LastSessionID") && Directory.Exists(Application.dataPath + "/Saves/save" + PlayerPrefs.GetInt("LastSessionID"));
        }
    }

	public void New()
	{
		if (!choiceDone)
		{
			PlayerPrefs.SetInt("SessionID", 0);
			fadeObject.FadeTo("Navette");
			choiceDone = true;
		}
	}

	public void Continue()
	{
		if (!choiceDone)
		{
			PlayerPrefs.SetInt("SessionID", PlayerPrefs.GetInt("LastSessionID"));
			MainStatus _main = MainStatus.load("Saves/save" + PlayerPrefs.GetInt("LastSessionID") + "/main.etheremos");
			fadeObject.FadeTo(_main.lastVisitedLevel);
			choiceDone = true;
		}
	}

	public void Options()
	{
		if (!choiceDone)
		{
			/*fadeObject.FadeTo("OptionMenu");
			choiceDone = true;*/
		}
	}

	public void LevelSelection()
	{
		if (!choiceDone)
		{
			fadeObject.FadeTo("LevelSelector");
			choiceDone = true;
		}
	}
	

	public void Quit()
	{
		if (!choiceDone)
		{
			StartCoroutine(Exit());
			choiceDone = true;
		}
    }

    public void ClearSaves()
    {
        PlayerPrefs.DeleteAll();
        if (Directory.Exists(Application.dataPath + "/Saves"))
            Directory.Delete(Application.dataPath + "/Saves", true);
        choiceDone = false;
    }

    public static IEnumerator Exit()
	{
		yield return new WaitForEndOfFrame();


        Application.Quit();
		
	}
}
