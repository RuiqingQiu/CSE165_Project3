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
	public HandController hand;
	// Use this for initialization
	void Start () {
		Debug.Log(Load("/Users/margaretwm3/Desktop/CSE165_Project3/Assets/test1.txt"));
		//After loading the file, add gate based on the point is at
		for(int i = 0; i < center_points.Count; i++){
			Vector3 up = up_vector[i];
			Vector3 right = right_vector[i];
			Vector3 location = center_points[i];
			location.y = location.y - 2.5f;			
			GameObject gate = (GameObject) Instantiate(Gate_Prefab, location, Quaternion.FromToRotation(new Vector3(0,1,0), up));

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
							//Add right vector to our list
							right_vector.Add(new Vector3(float.Parse(entries[3].Remove(0,2)),
							                             float.Parse(entries[4].Remove(0,2)),
							                             float.Parse(entries[5].Remove(0,2))));

							//Add up vector to our list
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
