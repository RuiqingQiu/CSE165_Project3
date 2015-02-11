using UnityEngine;
using System.Collections;

public class ArrowControl : MonoBehaviour {
	public GameObject spaceship;
	// Use this for initialization
	void Start () {
	
	}
	
	// Update is called once per frame
	void Update () {
		Vector3 to = GameController.gate_list[GameController.active_one].transform.position - spaceship.transform.position;
		to.Normalize();
		transform.rotation = Quaternion.FromToRotation(spaceship.transform.forward, to);
	}
}
