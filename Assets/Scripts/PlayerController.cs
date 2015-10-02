using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController : MonoBehaviour {
	public GameObject EbenenWeiss;
	public GameObject EbenenSchwarz;
	public float MoveTime;
	public float JumpHeight;

	CharacterController cc;
	//int playerLevel;
	GameObject lastColl;

	float startTime;
	bool isAnimation = false;
	Vector3 start, stop;


	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController> ();
	}
	
	// Update is called once per frame
	void Update () {
		/*
		Vector3 moveLR = Input.GetAxis ("Horizontal") * MoveSpeed * transform.TransformDirection(Vector3.right);
		Vector3 moveFB = Input.GetAxis ("Vertical") * MoveSpeed * transform.TransformDirection (Vector3.forward);
		cc.Move((moveLR + moveFB) * Time.deltaTime);
		*/
		if (!isAnimation) {
			start = transform.position;
			stop = start;
			if (Input.GetAxis ("Horizontal") > 0) { // Rechts
				isAnimation = true;
				stop += new Vector3 (1, 0, 0);
			} else if (Input.GetAxis ("Horizontal") < 0) { // Links
				isAnimation = true;
				stop += new Vector3 (-1, 0, 0);
			}
			if (Input.GetAxis ("Vertical") > 0) { // Vor
				isAnimation = true;
				stop += new Vector3 (0, 0, 1);
			} else if (Input.GetAxis ("Vertical") < 0) { // Zurück
				isAnimation = true;
				stop += new Vector3 (0, 0, -1);
			}
			if (isAnimation) {
				startTime = Time.time;
				//stop += new Vector3(0F,0.1F,0F);
			}

			cc.SimpleMove (Physics.gravity);
		} else {
			float progress = (Time.time - startTime) / MoveTime;
			if (progress > 1) {
				transform.position = stop;
				isAnimation = false;
			} else {
				transform.position = Vector3.Lerp(start, stop, progress) + new Vector3(0,1,0) * parabelJump(progress) * JumpHeight;
			}
		}

		
		//playerLevel = (int) System.Math.Round (transform.position.y / 2.0);
	}
	float parabelJump(float x) {
		return 1.0F - 4.0F * (x - 0.5F) * (x - 0.5F);
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		if (hit.gameObject.Equals (lastColl) || isAnimation) {
			Debug.Log("Nothing new");
			return;
		}
		lastColl = hit.gameObject;
		transform.parent = lastColl.transform; // Aneinander kleben

		if (hit.gameObject.tag == "Finish")
			Application.LoadLevel (1);
		else if (hit.gameObject.tag == "Respawn") {
			Application.LoadLevel (0);
		} else if (hit.gameObject.tag == "Weiß") {
			EbenenWeiss.GetComponent<EbenenController> ().BeginHover (true);
		} else if (hit.gameObject.tag == "Schwarz") {
			EbenenSchwarz.GetComponent<EbenenController> ().BeginHover (false);
		} else {
			Debug.Log("Collission not recognized");
		}
	}
}
