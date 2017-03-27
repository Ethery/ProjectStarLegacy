using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ObjectDictionaryItem : MonoBehaviour
{
	public string nom;
	public int stackValue, owned;

	[SerializeField]
	public List<string> tags;

	public ObjectDictionaryItem()
	{
		stackValue = 0;
		owned = 0;
		tags = new List<string>();
	}
}