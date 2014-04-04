using UnityEngine;
using System.Collections;

public class Menu_Playgame : MonoBehaviour {
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
		//load level
		//look at Unity File->Build Settings (number next to scenes)
		Application.LoadLevel(1);
	}
}