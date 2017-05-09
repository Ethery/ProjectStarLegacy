using System.Collections.Generic;
using UnityEngine;

public class ObjectDictionary : MonoBehaviour {

	public List<ObjectDictionaryItem> objects;
	
	public Dictionary<string, ObjectDictionaryItem> list;
	
	public void initList()
	{
        string str = "Init : ";
		list = new Dictionary<string, ObjectDictionaryItem>();
		foreach (ObjectDictionaryItem obj in objects)
		{
            str += obj.nom + "/";
			list.Add(obj.nom, obj);
		}

        //Debug.Log(str);
	}

    public void Awake()
    {
        initList();
    }
}



