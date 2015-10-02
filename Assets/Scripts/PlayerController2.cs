using UnityEngine;
using System.Collections;

[RequireComponent(typeof(CharacterController))]
public class PlayerController2 : MonoBehaviour {

	public float MoveTime;
	public float JumpHeight;

	public GameObject BretterWeiß;
	public GameObject BretterSchwarz;

	public TextMesh PosText;

	BretterControl2 cBretterWeiß;
	BretterControl2 cBretterSchwarz;

	CharacterController cc;
	//int playerLevel;
	GameObject lastColl;
	
	float startTime;
	bool isAnimation = false;
	Vector3 start, stop;
	bool grounded = false;

	// Use this for initialization
	void Start () {
		cc = GetComponent<CharacterController> ();
		cBretterWeiß = BretterWeiß.GetComponent<BretterControl2> ();
		cBretterSchwarz = BretterSchwarz.GetComponent<BretterControl2> ();
	}
	
	// Update is called once per frame
	void Update () {
		
		if (!isAnimation) {
			if (!cBretterWeiß.AnimInProgress && !cBretterSchwarz.AnimInProgress && grounded) {
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
					grounded = false;
					//stop += new Vector3(0F,0.1F,0F);
				}
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

	}
	float parabelJump(float x) {
		return 1.0F - 4.0F * (x - 0.5F) * (x - 0.5F);
	}

	void OnControllerColliderHit(ControllerColliderHit hit) {
		if (hit.gameObject.Equals (lastColl) || isAnimation) {
			//Debug.Log("Nothing new: " + hit.gameObject.name);
			return;
		}
		Debug.Log ("Collision with: " + hit.gameObject);
		lastColl = hit.gameObject;
		transform.parent = lastColl.transform; // Aneinander kleben
		
		if (hit.gameObject.tag == "Finish")
			Application.LoadLevel (1);
		else if (hit.gameObject.tag == "Respawn") {
			Application.LoadLevel (0);
		} else if (hit.gameObject.tag == "Weiß" || hit.gameObject.tag == "Schwarz") {
			FeldControl2 fc = hit.gameObject.GetComponent<FeldControl2>();
			Debug.Log("Auf Schachbrett " + (1 + fc.Board) + " auf Feld " + fc.Field);
			PosText.text = "B: " + (fc.Board + 1) + " - P: " + fc.Field;
			if (!cBretterWeiß.AnimInProgress && !cBretterSchwarz.AnimInProgress) {
				if (hit.gameObject.tag == "Weiß") {
					transform.parent = BretterWeiß.transform;
					cBretterWeiß.BeginAnimation();
				} else {
					transform.parent = BretterSchwarz.transform;
					cBretterSchwarz.BeginAnimation();
				}
			}
		} else {
			Debug.Log("Collission not recognized: " + lastColl);
		}
		grounded = true;
	}
}
