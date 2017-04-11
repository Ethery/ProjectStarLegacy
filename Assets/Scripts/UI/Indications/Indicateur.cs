using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class Indicateur : MonoBehaviour {

	public float delai;
    public string name;
	Indications txts;
	public float tempsDAffichage;
	public AnimationCurve curve;

	// Use this for initialization
	void Start () {
		delai = 0f;
		txts = FindObjectOfType<Indications>();
        if (txts != null)
            GetComponent<Text>().text = txts.getRandom(name);
        else
            GetComponent<Text>().text = "ATTENTION ! INDICATIONS MANQUANTES(ceci est un message d'erreur)";

    }
	
	// Update is called once per frame
	void Update () {
        if (txts != null)
        {
            delai += Time.unscaledDeltaTime;
        
            if (delai > tempsDAffichage - 1)
            {
                StartCoroutine(fadeOut());
            }
            if (delai > tempsDAffichage)
            {
                GetComponent<Text>().text = txts.getRandom(name);
                StartCoroutine(fadeIn());
                delai = 0f;
            }
        }
    }

	IEnumerator fadeOut()
	{
		float t = 1f;

		while (t > 0f)
		{
			t -= Time.unscaledDeltaTime;
			float a = curve.Evaluate(t);
			GetComponent<Text>().color = new Color(GetComponent<Text>().color.r, GetComponent<Text>().color.g, GetComponent<Text>().color.b, a);
			yield return a;
		}
	}

	IEnumerator fadeIn()
	{
		float t = 0f;

		while (t < 1f)
		{
			t += Time.unscaledDeltaTime;
			float a = curve.Evaluate(t);
			GetComponent<Text>().color = new Color(GetComponent<Text>().color.r, GetComponent<Text>().color.g, GetComponent<Text>().color.b, a);
			yield return a;
		}
	}
}
