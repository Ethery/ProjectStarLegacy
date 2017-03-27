using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.SceneManagement;

public class AirBar : MonoBehaviour {

	public GameObject airBar;
	private Image jauge;
	private Text txt;
	public bool correctBar = true;

	public float totalAir;
    public float actualAir;
    public Color colorAir;


    // Use this for initialization
    void Start () {
		if (airBar != null)
		{
			jauge = airBar.transform.FindChild("Air").GetComponent<Image>();
			txt = airBar.transform.FindChild("Text").GetComponent<Text>();
			if (jauge == null || txt == null)
			{
				Debug.Log("airBar n'a pas le bon format, le Texte doit s'appeller 'Text' et la jauge 'Air'");
				correctBar = false;
			}
			correctBar = true;
			jauge.color = colorAir;
		}
		else
			correctBar = false;
        actualAir = totalAir;
		if (correctBar)
			airBar.SetActive(false);
        updateBar();
    }

    public void updateBar()
    {
        if (correctBar)
        {
			jauge.fillAmount = getPourcentageAir();
            txt.text = actualAir + "/" + totalAir;
        }
    }

    public float getPourcentageAir()
    {
        return (actualAir / totalAir);
    }

    public void resetAir()
    {
        actualAir = totalAir;
        updateBar();
    }

    public void loseOxygen(int quantity)
    {
        actualAir -= quantity;
        updateBar();
    }
}
