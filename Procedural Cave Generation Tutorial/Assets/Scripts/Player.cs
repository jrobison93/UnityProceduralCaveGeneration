using UnityEngine;
using System.Collections;

public class Player : MonoBehaviour 
{
	public float sensitivity = 20.0f;
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
	
	// Update is called once per frame
	void Update () 
	{
		velocity = new Vector3(Input.GetAxisRaw("Horizontal"), 0, Input.GetAxisRaw("Vertical")).normalized * 10;

		rotationX = Input.GetAxis("Mouse X") * sensitivity;
		rotationY = Input.GetAxis("Mouse Y") * sensitivity;

	
	}

	void FixedUpdate()
	{
		//transform.localEulerAngles = new Vector3(0, rotationX, 0);
		transform.Rotate(0, rotationX, 0);
		camera.transform.Rotate(-rotationY, 0, 0);
		velocity = transform.rotation * velocity;
		rigidBody.MovePosition(rigidBody.position + velocity * Time.fixedDeltaTime); 
	}
}
