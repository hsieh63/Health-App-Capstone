using UnityEngine;
using System.Collections;

public class Menu_Exitgame : MonoBehaviour {
	void OnMouseEnter()
	{
		//change the color of the text
		guiText.material.color = Color.cyan;
	}
	
	void OnMouseExit()
	{
		//change the color of the text
		guiText.material.color = Color.white;
	}
	
	void OnMouseUp()
	{
		//quit
		Application.Quit();
	}
}