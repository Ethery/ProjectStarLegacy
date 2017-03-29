using System.Collections.Generic;
using UnityEngine;

public class ObjectDictionary : MonoBehaviour {

	public List<ObjectDictionaryItem> objects;
	public List<ob> obj;
	
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

[System.Serializable]
public class ob
{
	public ObjectDictionaryItem item;
	public int a;
}


