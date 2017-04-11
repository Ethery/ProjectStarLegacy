using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Indications : MonoBehaviour {

	public List<IndicationsTexts> textes;

	public string getRandom(string contexte)
	{
		foreach (IndicationsTexts context in textes)
		{
			if (context.name == contexte)
			{
				return context.txts[Random.Range(0, context.txts.Count)];
			}
		}
		return "";
		//TODO : TEST
	}
}

[System.Serializable]
public class IndicationsTexts
{
	public string name;
	public List<string> txts;
}
