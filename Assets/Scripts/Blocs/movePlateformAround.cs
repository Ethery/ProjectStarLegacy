using UnityEngine;
using System.Collections;

public class movePlateformAround : MonoBehaviour {

    public Vector2 distance;
    public float speed;
    public GameObject[] plateforms;
    
    public bool goRight = true;
    public float time = 0;

    // Use this for initialization
    void Start () {
		time = 0;
	}

    // Update is called once per frame
    void Update()
    {
        if(time > 2*Mathf.PI)
        {
            time = 0;
        }
        else
        {
            time += Time.deltaTime * speed;
        }

        float angle = ((2 * Mathf.PI) / plateforms.Length);
        for (int i=0;i < plateforms.Length;i++)
        {
            if (plateforms[i] != null)
            {
                Vector2 p = new Vector2(Mathf.Cos(time + (angle * i)) * distance.x, Mathf.Sin(time + (angle * i)) * distance.y);
                plateforms[i].transform.localPosition = p;
            }
        }
    }
}
