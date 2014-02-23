using UnityEngine;
using System.Collections;

public class Done_Mover_Left : MonoBehaviour
{
	public float speed;

	void Start ()
	{
		rigidbody.velocity = transform.right * speed;
	}
}
