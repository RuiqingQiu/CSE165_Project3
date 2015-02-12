 /**
 * UniMove API - A Unity plugin for the PlayStation Move motion controller
 * Copyright (C) 2012, 2013, Copenhagen Game Collective (http://www.cphgc.org)
 * 					         Patrick Jarnfelt
 * 					         Douglas Wilson (http://www.doougle.net)
 *
 *
 * All rights reserved.
 *
 * Redistribution and use in source and binary forms, with or without
 * modification, are permitted provided that the following conditions are met:
 *
 *    1. Redistributions of source code must retain the above copyright
 *       notice, this list of conditions and the following disclaimer.
 *
 *    2. Redistributions in binary form must reproduce the above copyright
 *       notice, this list of conditions and the following disclaimer in the
 *       documentation and/or other materials provided with the distribution.
 *
 * THIS SOFTWARE IS PROVIDED BY THE COPYRIGHT HOLDERS AND CONTRIBUTORS "AS IS"
 * AND ANY EXPRESS OR IMPLIED WARRANTIES, INCLUDING, BUT NOT LIMITED TO, THE
 * IMPLIED WARRANTIES OF MERCHANTABILITY AND FITNESS FOR A PARTICULAR PURPOSE
 * ARE DISCLAIMED. IN NO EVENT SHALL THE COPYRIGHT HOLDER OR CONTRIBUTORS BE
 * LIABLE FOR ANY DIRECT, INDIRECT, INCIDENTAL, SPECIAL, EXEMPLARY, OR
 * CONSEQUENTIAL DAMAGES (INCLUDING, BUT NOT LIMITED TO, PROCUREMENT OF
 * SUBSTITUTE GOODS OR SERVICES; LOSS OF USE, DATA, OR PROFITS; OR BUSINESS
 * INTERRUPTION) HOWEVER CAUSED AND ON ANY THEORY OF LIABILITY, WHETHER IN
 * CONTRACT, STRICT LIABILITY, OR TORT (INCLUDING NEGLIGENCE OR OTHERWISE)
 * ARISING IN ANY WAY OUT OF THE USE OF THIS SOFTWARE, EVEN IF ADVISED OF THE
 * POSSIBILITY OF SUCH DAMAGE.
 **/

using UnityEngine;
using System;
using System.Collections.Generic;

public class UniMoveTest : MonoBehaviour
{
	// This is the (3d object prototype in the scene)
	private GameObject moveControllerPrefab;
	public GameObject spaceship;
	public GameObject game_scene;
	public GameObject menu_scene;
	public GameObject game_mode;
	public GameObject build_mode;
	public GameObject load_level;

	// We save a list of Move controllers.
	public static List<UniMoveController> moves = new List<UniMoveController>();
	// This is a list of graphical representations of move controllers (3d object)
	public static List<MoveController> moveObjs = new List<MoveController>();
	private float time = 0.0f;
	private float speed = 100.0f;
	private float direction = 1.0f;
	//Indicating if there's a menu
	private bool menu = false;
	public bool set_orientation = false;


	void Start()
	{
		/* NOTE! We recommend that you limit the maximum frequency between frames.
		 * This is because the controllers use Update() and not FixedUpdate(),
		 * and yet need to update often enough to respond sufficiently fast.
		 * Unity advises to keep this value "between 1/10th and 1/3th of a second."
		 * However, even 100 milliseconds could seem slightly sluggish, so you
		 * might want to experiment w/ reducing this value even more.
		 * Obviously, this should only be relevant in case your framerare is starting
		 * to lag. Most of the time, Update() should be called very regularly.
		 */
		Time.maximumDeltaTime = 0.1f;

		moveControllerPrefab = GameObject.Find("MoveController");
		Destroy(moveControllerPrefab);
		if(moveControllerPrefab == null || moveControllerPrefab.GetComponent<MoveController>() == null)
			Debug.LogError("GameObject with object named \"MoveController\" with script MoveController is missing from the scene");



		int count = UniMoveController.GetNumConnected();

		// Iterate through all connections (USB and Bluetooth)
		for (int i = 0; i < count; i++)
		{
			UniMoveController move = gameObject.AddComponent<UniMoveController>();	// It's a MonoBehaviour, so we can't just call a constructor


			// Remember to initialize!
			if (!move.Init(i))
			{
				Destroy(move);	// If it failed to initialize, destroy and continue on
				continue;
			}



			// This example program only uses Bluetooth-connected controllers
			PSMoveConnectionType conn = move.ConnectionType;
			if (conn == PSMoveConnectionType.Unknown || conn == PSMoveConnectionType.USB)
			{
				Destroy(move);
			}
			else
			{
				moves.Add(move);

				move.OnControllerDisconnected += HandleControllerDisconnected;

				move.InitOrientation();
				move.ResetOrientation();

				// Start all controllers with a white LED
				move.SetLED(Color.white);

				// adding the MoveController Objects on screen
				GameObject moveController = GameObject.Instantiate(moveControllerPrefab,
					Vector3.right * count * 2 +  Vector3.left * i * 4, Quaternion.identity) as GameObject;
				MoveController moveObj = moveController.GetComponent<MoveController>();
				moveObjs.Add(moveObj);
				moveObj.SetLED(Color.white);

			}
		}
	}


	void Update()
	{
		int i = 0;
		time += Time.deltaTime;
		foreach(UniMoveController move in moves)
		{

			MoveController moveObj = moveObjs[i];

			// Instead of this somewhat kludge-y check, we'd probably want to remove/destroy
			// the now-defunct controller in the disconnected event handler below.
			if (move.Disconnected) continue;

			// Button events. Works like Unity's Input.GetButton
			if (move.GetButtonDown(PSMoveButton.Circle)){
				Debug.Log("Circle Down");
			}
			if (move.GetButtonUp(PSMoveButton.Circle)){
				Debug.Log("Circle UP");
			}
			float x = 0.0f; 
			float y = 0.0f; 
			float z = 0.0f;
			// Change the colors of the LEDs based on which button has just been pressed:
			if (move.GetButton(PSMoveButton.Circle)){
				y = 0.1f * Time.deltaTime * speed;
			}
			else if(move.GetButton(PSMoveButton.Cross)){
				y = -0.1f * Time.deltaTime * speed;
				//moveObj.SetLED(Color.red);move.SetLED(Color.red);
			}
			else if(move.GetButton(PSMoveButton.Square)) 	{
				x = -0.1f * Time.deltaTime * speed;
				//moveObj.SetLED(Color.yellow);move.SetLED(Color.yellow);
			}
			else if(move.GetButton(PSMoveButton.Triangle)) 	{
				x = 0.1f * Time.deltaTime * speed;
				//moveObj.SetLED(Color.magenta);move.SetLED(Color.magenta);
			}
			else if (move.GetButtonDown(PSMoveButton.Start)){
				Debug.Log ("start button");
				if(menu){
					if(game_mode.renderer.material.color == Color.green){
						Debug.Log ("game mode");
					}
					else if(build_mode.renderer.material.color == Color.green){
						Debug.Log ("build mode");
					}
				}
				menu = !menu;
				game_scene.SetActive(!menu);
				menu_scene.SetActive(menu);
			}
			else if(move.GetButtonDown(PSMoveButton.Select)){
				set_orientation = !set_orientation;
			}
//			if (move.GetButtonDown(PSMoveButton.Circle)){
//				move.SetLED(Color.red);
//			}
//			else if(move.GetButtonDown(PSMoveButton.Cross)){
//				move.SetLED(Color.blue);
//			}
//			else if(move.GetButtonDown(PSMoveButton.Square)) 	{
//				move.SetLED(Color.magenta);
//			}
//			else if(move.GetButtonDown(PSMoveButton.Triangle)) 	{
//				move.SetLED(Color.green);
//			}

			// On pressing the move button we reset the orientation as well.
			// Remember to keep the controller leveled and pointing at the screen
			// Reset once in a while because of drifting
			else if(move.GetButtonDown(PSMoveButton.Move)) {
				move.ResetOrientation();
				if(direction == 1.0f)
					direction = -1.0f;
				else 
					direction = 1.0f;
//				moveObj.SetLED(Color.black);
//				move.SetLED(Color.black);

			}
			spaceship.transform.Translate(x, y, z);			
			// Set the rumble based on how much the trigger is down
			//move.SetRumble(move.Trigger);
			Debug.Log (move.Trigger);
			moveObj.gameObject.transform.localRotation = move.Orientation;
			spaceship.transform.position += spaceship.transform.forward * Time.deltaTime * (direction * 100 * move.Trigger);
			if(time > 0.1f){
				time = 0.0f;
				if(set_orientation)
					spaceship.transform.localRotation = move.Orientation;
			}
			i++;

		}
		//Debug.Log ("time " + time);
	}

	void HandleControllerDisconnected (object sender, EventArgs e)
	{
		// TODO: Remove this disconnected controller from the list and maybe give an update to the player
	}

	void OnGUI()
	{
        string display = "";

		if (moves.Count > 0)
		{
            for (int i = 0; i < moves.Count; i++)
			{
                display += string.Format("PS Move {0}: ax:{1:0.000}, ay:{2:0.000}, az:{3:0.000} gx:{4:0.000}, gy:{5:0.000}, gz:{6:0.000}\n",
					i+1, moves[i].Acceleration.x, moves[i].Acceleration.y, moves[i].Acceleration.z,
					moves[i].Gyro.x, moves[i].Gyro.y, moves[i].Gyro.z);
            }
        }
        else display = "No Bluetooth-connected controllers found. Make sure one or more are both paired and connected to this computer.";

        GUI.Label(new Rect(10, Screen.height-100, 500, 100), display);
    }
	public void Rumble(){
		foreach (UniMoveController move in moves) {
			move.SetRumble (1);
		}
	}
}
