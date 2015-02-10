using UnityEngine;
using System.Collections;

public class SpaceshipController : MonoBehaviour {
	public float speed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	/**
	 * Keyboard control
	 **/
	void FixedUpdate(){
		float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
		float z = Input.GetAxis("Vertical") * Time.deltaTime * speed;
		transform.Translate(x, 0, z);
	}
}
