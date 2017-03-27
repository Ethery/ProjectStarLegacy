using System.Collections.Generic;
using UnityEngine;

public class ObjectDictionary : MonoBehaviour {

	public List<ObjectDictionaryItem> objects;

	[SerializeField]
	public Dictionary<string, ObjectDictionaryItem> list;
	
	public void initList()
	{
		list = new Dictionary<string, ObjectDictionaryItem>();
		foreach (ObjectDictionaryItem obj in objects)
		{
			list.Add(obj.nom, obj);
		}
	}
}

