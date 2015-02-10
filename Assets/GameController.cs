using UnityEngine;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.IO;  
using System;

public class GameController : MonoBehaviour {
	public GameObject Gate_Prefab;
	private List<Vector3> center_points = new List<Vector3>();
	private List<Vector3> right_vector = new List<Vector3>();
	private List<Vector3> up_vector = new List<Vector3> ();
	// Use this for initialization
	void Start () {
		Debug.Log(Load("/Users/ruiqingqiu/CSE165/Assets/test1.txt"));
		//After loading the file, add gate based on the point is at
		for(int i = 0; i < center_points.Count; i++){
			//Create gate at different locations
			Vector3 v1 = new Vector3(0,1,0);
			Vector3 v2 = right_vector[i];
			Vector3 a = Vector3.Cross(v1, v2);
			double w = Math.Sqrt((v1.sqrMagnitude) * (v2.sqrMagnitude)) + Vector3.Dot(v1, v2);
			float w1 = (float)w;
			Quaternion q = new Quaternion(a.x, a.y, a.z, w1);
			Debug.Log ("quaternion " + q);
			//Create the gate
			GameObject gate = (GameObject) Instantiate(Gate_Prefab, center_points[i], q);
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
	
	}
	private bool Load(string fileName)
	{
		// Handle any problems that might arise when reading the text
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
							center_points.Add(new Vector3(float.Parse(entries[0].Remove(0,2)),
							                             float.Parse(entries[1].Remove(0,2)),
							                             float.Parse(entries[2].Remove(0,2))));
							right_vector.Add(new Vector3(float.Parse(entries[3].Remove(0,2)),
							                             float.Parse(entries[4].Remove(0,2)),
							                             float.Parse(entries[5].Remove(0,2))));
							up_vector.Add(new Vector3(float.Parse(entries[6].Remove(0,2)),
							                          float.Parse(entries[7].Remove(0,2)),
							                          float.Parse(entries[8].Remove(0,2))));
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
