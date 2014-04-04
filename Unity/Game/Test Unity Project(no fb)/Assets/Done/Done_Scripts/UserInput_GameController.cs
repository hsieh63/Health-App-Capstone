using UnityEngine;
using System.Collections;

public class UserInput_GameController : MonoBehaviour
{	
	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.LoadLevel(0);
		}
	}

}