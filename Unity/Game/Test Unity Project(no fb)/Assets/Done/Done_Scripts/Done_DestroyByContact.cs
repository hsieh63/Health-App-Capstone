using UnityEngine;
using System.Collections;

public class Done_DestroyByContact : MonoBehaviour
{
	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue;
	private Done_GameController gameController;
	private int playerHealth;

	void Start ()
	{
		GameObject gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");
		if (gameControllerObject != null) {
			gameController = gameControllerObject.GetComponent <Done_GameController> ();
		}
		if (gameController == null) {
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Boundary" || other.tag == "Enemy" || other.tag == "Boss") {
			return;
		}

		if (explosion != null) {
			Instantiate (explosion, transform.position, transform.rotation);
		}

		if (other.tag == "Player") {
			playerHealth = other.GetComponent<Done_PlayerController>().shieldPoint;
			Debug.Log ("OnTrigger Destroy health = " + playerHealth.ToString ());
			if (playerHealth == 0) {
				Instantiate (playerExplosion, other.transform.position, other.transform.rotation);
				gameController.GameOver ();
				Destroy (other.gameObject);
			}
			else {
				playerHealth--;
				other.GetComponent<Done_PlayerController>().shieldPoint = playerHealth;
				if(playerHealth == 0) {
					other.renderer.material.color = Color.white;
				}
			}
		}
		else {
				Destroy (other.gameObject);
		}

		gameController.AddScore (scoreValue);
		Destroy (gameObject);
	}
}