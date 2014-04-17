using UnityEngine;
using System.Collections;

[System.Serializable]
public class Done_Boundary 
{
	public float xMin, xMax, zMin, zMax;
}

public class Done_PlayerController : MonoBehaviour
{
	public float speed;
	public float tilt;
	public Done_Boundary boundary;
	public bool updateTriShoot;
	public bool updateBigShot;
	public bool updateDualBigShot;
	public int shieldPoint;

	public GameObject shot;
	public GameObject shotRight;
	public GameObject shotLeft;
	public GameObject bigShot;
	public Transform shotSpawn;
	public Transform shotSpawnRight;
	public Transform shotSpawnLeft;
	public float fireRate;
	 
	private float nextFire;

//	void Start() {
//		if (shieldPoint > 0) {
//			renderer.material.color = Color.blue;
//		} else {
//			renderer.material.color = Color.white;
//		}
//	}
	
	void Update ()
	{
		if (Input.GetButton("Fire1") && Time.time > nextFire) 
		{
			nextFire = Time.time + fireRate;
			if (updateBigShot) {
				if(updateDualBigShot) {
					Instantiate(bigShot  , shotSpawnRight.position, shotSpawnRight.rotation);
					Instantiate(bigShot  , shotSpawnLeft.position, shotSpawnLeft.rotation);
				}
				else {
					Instantiate(bigShot  , shotSpawn.position, shotSpawn.rotation);
				}
			}
			else{
				Instantiate(shot, shotSpawn.position, shotSpawn.rotation);
				if(updateTriShoot) {
					Instantiate(shotRight, shotSpawnRight.position, shotSpawnRight.rotation);
					Instantiate(shotLeft, shotSpawnLeft.position, shotSpawnLeft.rotation);
				}
			}
			audio.Play ();
		}
	}

	void FixedUpdate ()
	{
		//float moveHorizontal = Input.GetAxis ("Horizontal");
		//float moveVertical = Input.GetAxis ("Vertical");

		//Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		Vector3 movement = Vector3.zero;
		movement.x = Input.acceleration.x;
		movement.y = 0.0f;
		movement.z = Input.acceleration.y;
		rigidbody.velocity = movement * speed;
		
		rigidbody.position = new Vector3
		(
			Mathf.Clamp (rigidbody.position.x, boundary.xMin, boundary.xMax), 
			0.0f, 
			Mathf.Clamp (rigidbody.position.z, boundary.zMin, boundary.zMax)
		);
		
		rigidbody.rotation = Quaternion.Euler (0.0f, 0.0f, rigidbody.velocity.x * -tilt);
	}
}
