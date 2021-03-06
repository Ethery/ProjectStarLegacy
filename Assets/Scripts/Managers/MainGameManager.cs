using UnityEngine;

public class MainGameManager : MonoBehaviour {

    #region Noms globaux
    public static string NOM_VAISSEAU = "Voiron";

    #endregion

    #region Variables

    public GameObject fadePrefab;
    private static Fading fadeObject;

	public Health health;
	public 

    #endregion


    #region Unity Methods

    // Use this for initialization
    void Start ()
	{
        DontDestroyOnLoad(this);
        fadeObject = FindObjectOfType<Fading>();
        if (fadeObject == null)
        {
            fadeObject = Instantiate(fadePrefab).GetComponent<Fading>();
        }
    }
	
	// Update is called once per frame
	void Update ()
	{
		foreach (MainGameManager a in FindObjectsOfType<MainGameManager>())
		{
			if (a != this)
			{
				Destroy(a.gameObject);
			}
		}
	}
	
	#endregion
}