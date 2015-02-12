using UnityEngine;
using System.Collections;

public class GateTriggerBad : MonoBehaviour {
	// Use this for initialization
	void Start () {
	}
	
	// Update is called once per frame
	void Update () {
		
	}
	void OnTriggerEnter(Collider other) {
		//Debug.Log ("enter");
//				foreach (UniMoveController move in UniMoveTest.moves) {
//					move.SetRumble (1);
//				}
		
	}
	void OnTriggerExit(Collider other){
		//Debug.Log ("exit");
//				foreach (UniMoveController move in UniMoveTest.moves) {
//					move.SetRumble (0);
//				}

	}
}
