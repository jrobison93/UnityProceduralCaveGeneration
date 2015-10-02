using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour 
{
	public float sensitivity = 1.0f;
	public GameObject player;

	private Vector3 offset;
	float rotationY;

	// Use this for initialization
	void Start () 
	{
		offset = transform.position - player.transform.position;
	
	}
	
	// Update is called once per frame
	void LateUpdate () 
	{
		transform.position = player.transform.position + offset;
		transform.rotation = player.transform.rotation;
		rotationY = Input.GetAxis("Mouse Y") * sensitivity;
		//rotationY = Mathf.Clamp(rotationY, -60.0f, 60.0f);
		//transform.Rotate(-rotationY, 0, 0);
	}
}
