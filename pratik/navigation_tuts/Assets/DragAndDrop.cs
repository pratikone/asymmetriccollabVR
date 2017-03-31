using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class DragAndDrop : MonoBehaviour
{  
	private bool _mouseState;
	public GameObject Target;
	public GameObject textBox;
	public Vector3 screenSpace;
	public Vector3 offset;
	public Vector3 lastMousePos;
	public enum constraintTypes {NONE, X,Y,Z};
	public enum moveTypes {Transform, Rotate, Scale};
	public constraintTypes constraint;
	public moveTypes mode;
	// Use this for initialization
	void Start ()
	{
		constraint = constraintTypes.NONE;
		mode= moveTypes.Transform;
	}

	// Update is called once per frame
	void Update ()
	{

		Text text = textBox.GetComponent<Text> (); 

		//Debug.Log(_mouseState);
		if (Input.GetMouseButtonDown (0)) {
			RaycastHit hitInfo;
			if (Target == GetClickedObject (out hitInfo)) {
				_mouseState = true;
				lastMousePos = Camera.main.ScreenToWorldPoint (Input.mousePosition) + offset;
				screenSpace = Camera.main.WorldToScreenPoint (Target.transform.position);
				offset = Target.transform.position - Camera.main.ScreenToWorldPoint (new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenSpace.z));
			}
		}
		if (Input.GetMouseButtonUp (0)) {
			_mouseState = false;
		}

		if (Input.GetKeyDown ("z")) {
			constraint = constraintTypes.Z;
			text.text = "Z";
		} else if (Input.GetKeyDown ("x")) {
			constraint = constraintTypes.X;
			text.text = "X";
		} else if (Input.GetKeyDown ("y")) {
			constraint = constraintTypes.Y;
			text.text = "Y";
		}

		if (constraint != constraintTypes.NONE && Input.GetKeyDown ("space")) {
			constraint = constraintTypes.NONE;
			text.text = "None";
		}

		if (_mouseState) {
			//keep track of the mouse position
			var curScreenSpace = new Vector3 (Input.mousePosition.x, Input.mousePosition.y, screenSpace.z);

			//convert the screen mouse position to world point and adjust with offset
			var curPosition = Camera.main.ScreenToWorldPoint (curScreenSpace) + offset;

			//update the position of the object in the world
			//for constrained movement

			if (mode == moveTypes.Transform) {
				Vector3 tempValues = Target.transform.position;
				if (constraint == constraintTypes.NONE || constraint == constraintTypes.X)
					tempValues.x = curPosition.x;
				if (constraint == constraintTypes.NONE || constraint == constraintTypes.Y)
					tempValues.y = curPosition.y;
				if (constraint == constraintTypes.NONE || constraint == constraintTypes.Z)
					tempValues.z = curPosition.z;
				Target.transform.position = tempValues; 
			} else if (mode == moveTypes.Rotate) {
				float speedX = 5.0f;
				float speedY = 5.0f;
				float speedZ = 10.0f;
				Vector3 tempValues = new Vector3 (0, 0, 0);
				if (constraint == constraintTypes.NONE || constraint == constraintTypes.X)
					tempValues.y = -curPosition.x * speedX;   // TODO : Why difference in the coordinate system here ?
				if (constraint == constraintTypes.NONE || constraint == constraintTypes.Y)
					tempValues.x = curPosition.y * speedY;   
				if (constraint == constraintTypes.NONE || constraint == constraintTypes.Z)
					tempValues.z = curPosition.z * speedZ;   
				Target.transform.Rotate (tempValues, Space.World);
			} else if (mode == moveTypes.Scale) {
				/*
				Vector3 tempValues = Target.transform.localScale;
				if (constraint == constraintTypes.NONE || constraint == constraintTypes.X)
					tempValues.x += curPosition.x - lastMousePos.x;
				if (constraint == constraintTypes.NONE || constraint == constraintTypes.Y)
					tempValues.y += curPosition.y - lastMousePos.y;
				if (constraint == constraintTypes.NONE || constraint == constraintTypes.Z)
					tempValues.z += curPosition.z - lastMousePos.z;
				Target.transform.localScale = Vector3.Lerp (Target.transform.localScale, tempValues, Time.deltaTime);
				*/
			}
		}
	}


	GameObject GetClickedObject (out RaycastHit hit)
	{
		GameObject target = null;
		Ray ray = Camera.main.ScreenPointToRay (Input.mousePosition);
		if (Physics.Raycast (ray.origin, ray.direction * 10, out hit)) {
			target = hit.collider.gameObject;
		}

		return target;
	}
}