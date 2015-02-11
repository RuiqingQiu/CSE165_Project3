using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {
	public GameObject text1;
	public GameObject text2;
	// Use this for initialization
	void Start () {
		text1.renderer.material.color = Color.blue;
		text2.renderer.material.color = Color.green;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
