using UnityEngine;
using System.Collections;

public class SpaceshipController : MonoBehaviour {
	public float speed;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		//transform.Translate(Vector3.forward * speed * Time.deltaTime)
	}

	/**
	 * Keyboard control
	 **/
	void FixedUpdate(){
//		float x = Input.GetAxis("Horizontal") * Time.deltaTime * speed;
//		float y = Input.GetAxis ("Jump") * Time.deltaTime * speed;
//		float z = Input.GetAxis("Vertical") * Time.deltaTime * speed;
//		transform.Translate(x, y, z);
	}
}
