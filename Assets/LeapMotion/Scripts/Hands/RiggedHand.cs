/******************************************************************************\
* Copyright (C) Leap Motion, Inc. 2011-2014.                                   *
* Leap Motion proprietary. Licensed under Apache 2.0                           *
* Available at http://www.apache.org/licenses/LICENSE-2.0.html                 *
\******************************************************************************/

using UnityEngine;
using System.Collections;
using Leap;

// Class to setup a rigged hand based on a model.
public class RiggedHand : HandModel {

  public Transform palm;
  public GameObject spaceship;
	public GameObject game_mode;
	public GameObject build_mode;
	public GameObject load_level;
  public Transform foreArm;

  public Vector3 modelFingerPointing = Vector3.forward;
  public Vector3 modelPalmFacing = -Vector3.up;

  public override void InitHand() {
    UpdateHand();
  }

  public Quaternion Reorientation() {
    return Quaternion.Inverse(Quaternion.LookRotation(modelFingerPointing, -modelPalmFacing));
  }

  public override void UpdateHand() {
    if (palm != null) {
			//Modify spaceship's position
	Vector3 position = GetPalmPosition();
	Debug.Log (position);
	//position.Scale(new Vector3(5,5,-5));
	if(position.y < -2.0){
		build_mode.renderer.material.color = Color.red;
		game_mode.renderer.material.color = Color.green;
		load_level.renderer.material.color = Color.red;

	}
	else if(position.y < -1.0){
				game_mode.renderer.material.color = Color.red;
				build_mode.renderer.material.color = Color.green;
				load_level.renderer.material.color = Color.red;

	}
	else if(position.y < 0.0){
				game_mode.renderer.material.color = Color.red;
				build_mode.renderer.material.color = Color.red;
				load_level.renderer.material.color = Color.green;

	}

	//spaceship.transform.position += (Vector3.forward + position) * Time.deltaTime ;
     palm.position = new Vector3(0,-300,0);
	  //palm.position = GetPalmPosition();
      palm.rotation = GetPalmRotation() * Reorientation();
    }

    if (foreArm != null)
      foreArm.rotation = GetArmRotation() * Reorientation();

    for (int i = 0; i < fingers.Length; ++i) {
      if (fingers[i] != null)
        fingers[i].UpdateFinger();
    }
  }
}
