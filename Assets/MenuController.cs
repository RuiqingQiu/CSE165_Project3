using UnityEngine;
using System.Collections;

public class MenuController : MonoBehaviour {
	public GameObject game_mode;
	public GameObject build_mode;
	public GameObject load_level;
	// Use this for initialization
	void Start () {
		game_mode.renderer.material.color = Color.red;
		build_mode.renderer.material.color = Color.red;
		load_level.renderer.material.color = Color.red;
	}
	
	// Update is called once per frame
	void Update () {
	
	}
}
