using UnityEngine;
using System.Collections;
using UnityEngine.UI;

[RequireComponent(typeof(Text))]
public class FluctuationLife : MonoBehaviour {

    public int speed = 2;

	public Color color;
	public string txt;

	private Vector3 direction = new Vector3(0,1,0);

    private Vector2 movement;

    private Transform m_player;

    // Use this for initialization
    void Start () {
		GetComponent<Text>().color = color;
		GetComponent<Text>().text = txt;
		Destroy(gameObject, 3); // 3sec
    }
	

    private void FixedUpdate()
    {
        // Application du mouvement
        gameObject.GetComponent<Transform>().position += (direction*speed)*Time.fixedDeltaTime;
    }

}
