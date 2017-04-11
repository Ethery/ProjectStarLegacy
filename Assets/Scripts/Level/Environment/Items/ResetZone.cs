using System.Collections;
using System.Collections.Generic;
using UnityEngine.SceneManagement;
using UnityEngine;

public class ResetZone : MonoBehaviour {

	private void OnTriggerEnter2D(Collider2D collision)
	{
		if (collision.name == "Player")
		{
			FindObjectOfType<Fading>().FadeTo(SceneManager.GetActiveScene().name);
		}
		else
		{
			GameObject.Destroy(collision.gameObject);
		}
	}
}
