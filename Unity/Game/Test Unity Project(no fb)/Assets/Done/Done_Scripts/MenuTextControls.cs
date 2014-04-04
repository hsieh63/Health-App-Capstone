using UnityEngine;
using System.Collections;

public class MenuTextControls : MonoBehaviour
{
	private bool isQuitButton = false;

	void Start ()
	{
		if ( guiText.text.Equals("Exit") )
		{
			isQuitButton = true;
		}
	}
	
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
		if( isQuitButton )
		{
			//quit
			Application.Quit();
		}
		else
		{
			//load level
			//look at Unity File->Build Settings (number next to scenes)
			Application.LoadLevel(1);
		}
	}
}