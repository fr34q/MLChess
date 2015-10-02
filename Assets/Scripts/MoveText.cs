using UnityEngine;
using System.Collections;

public class MoveText : MonoBehaviour {

	public Transform startMarker;
	public Transform endMarker;
	public float speed = 1.0F;
	private float startTime;
	private float journeyLength;
	private bool backwards = false;
	void Start() {
		startTime = Time.time;
		journeyLength = Vector3.Distance(startMarker.position, endMarker.position);
	}
	void Update() {
		float distCovered = (Time.time - startTime) * speed;
		float fracJourney = distCovered / journeyLength;
		if (backwards)
			fracJourney = 1 - fracJourney;
		transform.position = Vector3.Lerp(startMarker.position, endMarker.position, polynInterp(fracJourney));

		Debug.Log (fracJourney + " --> " + polynInterp (fracJourney));
		if (fracJourney > 1F) {
			startTime = Time.time;
			backwards = true;
		} else if (fracJourney < 0F) {
			startTime = Time.time;
			backwards = false;
		}
	}

	float polynInterp(float x)
	{
		return -x * x * x + 2 * x * x;
	}
}
