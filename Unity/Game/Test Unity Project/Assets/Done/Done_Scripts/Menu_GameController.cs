using UnityEngine;
using System.Collections;

public class Menu_GameController : MonoBehaviour
{	
	void Start () {
		//used for every unique device
		//can be used as ID
		string deviceID = SystemInfo.deviceUniqueIdentifier.ToString ();
	}

	void Update ()
	{
		if (Input.GetKeyDown (KeyCode.Escape)) {
			Application.Quit();
		}
	}

}