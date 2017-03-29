using UnityEngine;

public class MainGameManager : MonoBehaviour {

    #region Variables

    public GameObject fadePrefab;
    private static Fading fadeObject;

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
		
	}
	
	#endregion
}