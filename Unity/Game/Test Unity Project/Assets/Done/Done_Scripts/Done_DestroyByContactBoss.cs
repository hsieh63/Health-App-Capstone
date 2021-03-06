﻿using UnityEngine;
using System.Collections;

public class Done_DestroyByContactBoss : MonoBehaviour
{
	public GameObject explosion;
	public GameObject playerExplosion;
	public int scoreValue;
	public int hitpoints;
	private Done_GameController gameController;
	private int boltCount;

	void Start ()
	{
		boltCount = 0;
		GameObject gameControllerObject = GameObject.FindGameObjectWithTag ("GameController");
		if (gameControllerObject != null)
		{
			gameController = gameControllerObject.GetComponent <Done_GameController>();
		}
		if (gameController == null)
		{
			Debug.Log ("Cannot find 'GameController' script");
		}
	}

	void OnTriggerEnter (Collider other)
	{
		if (other.tag == "Boundary" || other.tag == "Enemy" || other.tag == "Boss")
		{
			return;
		}

		boltCount++;

		if (explosion != null)
		{
			if (boltCount == hitpoints) {
				Instantiate(explosion, transform.position, transform.rotation);
			}
		}
		
		if (other.tag == "Player")
		{
			Instantiate(playerExplosion, other.transform.position, other.transform.rotation);
			gameController.GameOver();
		}
		
		Destroy (other.gameObject);
		if (boltCount == hitpoints) {
			gameController.AddScore (scoreValue);
			Destroy (gameObject);
		}
	}
}