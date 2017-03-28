using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class moveit : MonoBehaviour {
	float distance = 10;

	void onMouseDrag(){
		Vector3 mousePosition = new Vector3(Input.mousePosition.x, Input.mousePosition.y, distance);
		Vector3 objPosition = Camera.main.ScreenToWorldPoint (mousePosition);
		transform.position = objPosition;
	}

}
