using UnityEngine;
using System.Collections;

public class Menu_Links : MonoBehaviour {
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
		//Algorithms with user input to determine types of pages to link to
		//link to webpage
		//Application.OpenURL ("http://www.reddit.com/r/fitness");
		AndroidJavaClass testcl = new AndroidJavaClass ("com.unity3d.player.UnityPlayer");
		AndroidJavaObject testob = testcl.GetStatic<AndroidJavaObject> ("currentActivity");
		testob.Call ("Launch");
	}
}