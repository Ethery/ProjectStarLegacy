using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FadeManager : MonoBehaviour {
	public bool ended = false;

	public void Ready()
	{
		ended = true;
	}
}
