using UnityEngine;
using System.Collections;

public class CameraController2 : MonoBehaviour {

	public float TurnDuration;

	public int CurrView {
		get {
			return currView;
		}
	}

	bool isAnimation = false;
	float start, end;
	float currAngle;
	float startTime;
	int currView = 0;

	GameObject beschr;
	TextMesh brettnr;

	// Use this for initialization
	void Start () {
		beschr = transform.Find ("Bretter Beschriftung").gameObject;
		brettnr = beschr.transform.Find ("Brettnummer").gameObject.GetComponent<TextMesh> ();
	}
	
	// Update is called once per frame
	void Update () {
		if (!isAnimation) {
			start = transform.rotation.eulerAngles.y;
			if (Input.GetAxis ("Mouse ScrollWheel") > 0) { // Rechts
				isAnimation = true;
				currView --;
				end = 90*currView;
				if (currView < 0)
					currView += 4;
			} else if (Input.GetAxis ("Mouse ScrollWheel") < 0) { // Links
				isAnimation = true;
				currView ++;
				end = 90*currView;
				if (currView > 3)
					currView -= 4;
			}
			if (isAnimation) {
				startTime = Time.time;
				beschr.SetActive(false);
				brettnr.text = (currView+1).ToString();
			}
		} else {
			float progress = (Time.time - startTime) / TurnDuration;
			if (progress > 1) {
				currAngle = end;
				isAnimation = false;
				beschr.SetActive(true);
			} else {
				currAngle = start + (end - start)*polynInterp(progress);
			}
			transform.rotation = Quaternion.Euler(0, currAngle % 360, 0);
		}
	}
	float polynInterp(float x)
	{
		return -x * x * x + 2 * x * x;
	}
}
