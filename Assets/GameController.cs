using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;  
using System;
using Leap;

public class GameController : MonoBehaviour {
	public GameObject Gate_Prefab;
	private List<Vector3> center_points = new List<Vector3>();
	private List<Vector3> right_vector = new List<Vector3>();
	private List<Vector3> up_vector = new List<Vector3> ();
	public static List<GameObject> gate_list = new List<GameObject>();
	//Keep track which one is active
	public static int active_one = 0;
	public static int total_num = 0;
	public HandController hand;
	private float time = 0.0f;
	//0 for game mode and 1 for build mode
	public static int Mode = 0;
	public static bool load_level = false;
	public static string file_to_load = "";
	// Use this for initialization
	void Start () {
		//default play mode
		Mode = 0;
		//Debug.Log(Load("/Users/margaretwm3/Desktop/CSE165_Project3/Assets/test1.txt"));
		//Debug.Log (Load ("/Users/ruiqingqiu/CSE165/Assets/test1.txt"));
		Debug.Log (Load ("/Users/ruiqingqiu/CSE165/Assets/competition_7.txt"));
		//After loading the file, add gate based on the point is at
		total_num = center_points.Count;
		for(int i = 0; i < center_points.Count; i++){
			Vector3 up = up_vector[i];
			Vector3 right = right_vector[i];
			Vector3 location = center_points[i];
			Debug.Log (location);
			Debug.Log (up);
			Debug.Log (right);

//
//			Vector3 normal = Vector3.Cross(up,right);
//			Vector3 axis = Vector3.Cross(normal, new Vector3(0,0,1));
//			double angle = Math.Acos(Vector3.Dot(normal, new Vector3(0,0,1)) / normal.magnitude);
//			float a = (float)angle;
//			Quaternion tmp = Quaternion.AngleAxis(a,axis);


			//location.y = location.y - 2.5f;	
			up.Normalize();
			right.Normalize();
			Quaternion first = Quaternion.FromToRotation(new Vector3(0,1,0), up);
			Debug.Log ("first " + first);
			Quaternion second = Quaternion.FromToRotation(new Vector3(1,0,0), right);
			Debug.Log ("second " + second);

			GameObject gate = (GameObject) Instantiate(Gate_Prefab, location, first);
			gate.GetComponent<GateTrigger>().num = i;
			gate_list.Add(gate);
//			foreach (Renderer r in gate.GetComponentsInChildren<Renderer>()){
//				r.material.color = Color.red;
//			}
			//GameObject gate = (GameObject) Instantiate(Gate_Prefab, location,Quaternion.AngleAxis(a, aor));
		}
		/*
		for (var y = 0; y < gridY; y++) {
			for (var x=0;x<gridX;x++) {
				var pos = Vector3 (x, 0, y) * spacing;
				Instantiate(prefab, pos, Quaternion.identity);
			}
		}*/
	}
	
	// Update is called once per frame
	void Update () {
		if (load_level) {
			Debug.Log ("load new level");
			for(int i = 0; i < gate_list.Count; i++){
				Destroy(gate_list[i]);
			}
			gate_list.Clear();
			active_one = 0;
			Debug.Log ("loading file " + file_to_load);
			Debug.Log (Load (file_to_load));
			//After loading the file, add gate based on the point is at
			total_num = center_points.Count;
			for(int i = 0; i < center_points.Count; i++){
				Vector3 up = up_vector[i];
				Vector3 right = right_vector[i];
				Vector3 location = center_points[i];
				Debug.Log (location);
				Debug.Log (up);
				Debug.Log (right);
				
				//
				//			Vector3 normal = Vector3.Cross(up,right);
				//			Vector3 axis = Vector3.Cross(normal, new Vector3(0,0,1));
				//			double angle = Math.Acos(Vector3.Dot(normal, new Vector3(0,0,1)) / normal.magnitude);
				//			float a = (float)angle;
				//			Quaternion tmp = Quaternion.AngleAxis(a,axis);
				
				
				//location.y = location.y - 2.5f;	
				up.Normalize();
				right.Normalize();
				Quaternion first = Quaternion.FromToRotation(new Vector3(0,1,0), up);
				Debug.Log ("first " + first);
				Quaternion second = Quaternion.FromToRotation(new Vector3(1,0,0), right);
				Debug.Log ("second " + second);
				
				GameObject gate = (GameObject) Instantiate(Gate_Prefab, location, first);
				gate.GetComponent<GateTrigger>().num = i;
				gate_list.Add(gate);
			}
			load_level = false;
		}
		if (active_one == total_num) {
			//Game done
			Debug.Log ("you won");
		} else {
						foreach (Renderer r in gate_list[active_one].GetComponentsInChildren<Renderer>()) {
								r.material.color = Color.red;
						}
		}
		for (int i = 0; i < active_one; i++) {
			foreach (Renderer r in gate_list[i].GetComponentsInChildren<Renderer>()) {
				r.material.color = Color.green;
			}
		}
	}
	void OnGUI()
	{
		time += Time.deltaTime;
		string display = "Running: " + time + "s";
		string score = active_one + " / " + total_num;
		
		GUI.Label(new Rect(10, 0, 500, 100), display);
		GUI.Label(new Rect(20, 10, 500, 100), score);
	}
	private bool Load(string fileName)
	{
		// Handle any problems that might arise when reading the text
		center_points.Clear ();
		right_vector.Clear ();
		up_vector.Clear ();
		try
		{
			string line;
			// Create a new StreamReader, tell it which file to read and what encoding the file
			// was saved as
			StreamReader theReader = new StreamReader(fileName, Encoding.Default);
			
			// Immediately clean up the reader after this block of code is done.
			// You generally use the "using" statement for potentially memory-intensive objects
			// instead of relying on garbage collection.
			// (Do not confuse this with the using directive for namespace at the 
			// beginning of a class!)
			using (theReader)
			{
				// While there's lines left in the text file, do this:
				do
				{
					line = theReader.ReadLine();
					
					if (line != null)
					{
						// Do whatever you need to do with the text line, it's a string now
						// In this example, I split it into arguments based on comma
						// deliniators, then send that array to DoStuff()
						string[] entries = line.Split(' ');
						if (entries.Length > 0){
							//Add center points to our list
							center_points.Add(new Vector3(float.Parse(entries[0]),
							                             float.Parse(entries[1]),
							                             float.Parse(entries[2])));
							//Add right vector to our list
							right_vector.Add(new Vector3(float.Parse(entries[3]),
							                             float.Parse(entries[4]),
							                             float.Parse(entries[5])));

							//Add up vector to our list
							up_vector.Add(new Vector3(float.Parse(entries[6]),
							                          float.Parse(entries[7]),
							                          float.Parse(entries[8])));
						}
					}
				}
				while (line != null);
				
				// Done reading, close the reader and return true to broadcast success    
				theReader.Close();
				return true;
			}
		}
		
		// If anything broke in the try block, we throw an exception with information
		// on what didn't work
		catch (Exception e)
		{
			Debug.Log (e.Message);
			//Console.WriteLine("{0}\n", e.Message);
			return false;
		}
	}

}
