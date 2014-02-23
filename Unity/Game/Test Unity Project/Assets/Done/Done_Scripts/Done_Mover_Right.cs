using UnityEngine;
using System.Collections;

public class Done_Mover_Right : MonoBehaviour
{
	public float speed;

	void Start ()
	{
		rigidbody.velocity = transform.right * speed;
	}
}
