using UnityEngine;
using System.Collections;

public class BretterControl2 : MonoBehaviour {

	public bool IsWhite;
	public float TurnDuration;
	public float ElevateDuration;
	public float ElevateHeight;

	public bool AnimInProgress {
		get {
			return stateAnimation != 0;
		}
	}

	int stateAnimation = 0;
	float startTime;
	Vector3 start, stop;
	float startA, stopA;
	int orientation = 0;

	// Use this for initialization
	void Start () {

	}
	
	// Update is called once per frame
	void Update () {
		float progress;
		switch (stateAnimation) {
		case 1: // Alles für 2 vorbereiten
			start = transform.position;
			stop = start + ElevateHeight * new Vector3(0,1,0);
			startTime = Time.time;
			stateAnimation = 2;
			break;
		case 2: // Platten heben ab
			progress = (Time.time - startTime) / ElevateDuration;
			if (progress > 1) {
				transform.position = stop;
				stateAnimation = 3;
			} else {
				transform.position = Vector3.Lerp(start, stop, polynInterp(progress));
			}
			break;
		case 3: // Vorbereitung auf 4
			startA = transform.rotation.eulerAngles.y;
			if (IsWhite)
				orientation ++;
			else
				orientation --;
			stopA = 90 * orientation;
			orientation = (orientation + 4) % 4;
			startTime = Time.time;
			stateAnimation = 4;
			break;
		case 4: // Drehung
			progress = (Time.time - startTime) / TurnDuration;
			float curr;
			if (progress > 1) {
				curr = stopA;
				stateAnimation = 5;
			} else {
				curr = startA + (stopA - startA)*polynInterp(progress);
			}
			transform.rotation = Quaternion.Euler(0, curr, 0);
			break;
		case 5: // Vorbereitung auf 6
			startTime = Time.time;
			stateAnimation = 6;
			break;
		case 6: // Platten senken sich (rückwärts von stop -> start)
			progress = (Time.time - startTime) / ElevateDuration;
			if (progress > 1) {
				transform.position = start;
				stateAnimation = 0;
			} else {
				transform.position = Vector3.Lerp(stop, start, polynInterp(progress));
			}
			break;
		default: // Keine Animation
			break;
		}
	}
	float polynInterp(float x)
	{
		return -x * x * x + 2 * x * x;
	}

	public void BeginAnimation() {
		if (stateAnimation == 0) {
			var list = GetComponentsInChildren<FeldControl2>();
			Debug.Log("Component Count: " + list.Length);
			if (IsWhite) {
				foreach (var item in list) {
					item.Board = (item.Board +1) % 4;
				}
			} else {
				foreach (var item in list) {
					item.Board = (item.Board -1) % 4;
				}
			}

			stateAnimation = 1;
		}
	}
}
