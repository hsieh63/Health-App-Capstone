using UnityEngine;
using System.Collections;

public class Menu_GameController : MonoBehaviour
{	
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit();
		}
	}

}