using UnityEngine;
using System.Collections;

public class PlayerBall : MonoBehaviour 
{
	public float speed = 5.0f;
	public GameObject camera;

	Rigidbody rigidBody;
	Vector3 velocity;
	float rotationX;
	float rotationY;

	// Use this for initialization
	void Start () 
	{
		rigidBody = GetComponent<Rigidbody>();
	
	}

	void FixedUpdate()
	{

		float moveHorizontal = Input.GetAxis ("Horizontal");
		float moveVertical = Input.GetAxis ("Vertical");
		
		Vector3 movement = new Vector3 (moveHorizontal, 0.0f, moveVertical);
		movement = transform.rotation * movement;
		
		rigidBody.AddForce(movement * speed);
	}
}
