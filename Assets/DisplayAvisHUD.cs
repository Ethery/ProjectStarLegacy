using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class DisplayAvisHUD : MonoBehaviour {

    public Text t;
    public bool toUpdate;

	// Update is called once per frame
	void Update ()
    {
        if (toUpdate)
        {
            foreach (Text item in GetComponentsInChildren<Text>())
            {
                Destroy(item.gameObject);
            }
            Dictionary<object, object> avis = FindObjectOfType<SavesManager>().main.getAvisAsDictionnary();
            foreach (KeyValuePair<object, object> obj in avis)
            {
                Text txt = Instantiate(t, transform, false);
                txt.text = obj.Key + ":" + obj.Value;
            }
            toUpdate = false;
        }
    }
}
