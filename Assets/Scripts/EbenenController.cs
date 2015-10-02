using UnityEngine;
using System.Collections;

public class EbenenController : MonoBehaviour {

	public bool IsWhite;
	public float HoverDuration;
	public int StartLevel;
	public int MinLevel;
	public int MaxLevel;
	public bool StartMovingUp;

	bool isAnimation = false;
	int currLevel, playerLevel;
	float startTime;
	bool isMovingUp;

	// Use this for initialization
	void Start () {
		currLevel = StartLevel;
		isMovingUp = StartMovingUp;
	}
	
	// Update is called once per frame
	void Update () {
		if (isAnimation) {
			float progress = (Time.time - startTime) / HoverDuration;
			Vector3 start, end;
			start = new Vector3(0, currLevel*2,0);
			if(isMovingUp) {
				end = new Vector3(0, (currLevel+1)*2, 0);
			} else {
				end = new Vector3(0, (currLevel-1)*2, 0);
			}
			if (progress > 1) {
				transform.position = end;
				isAnimation = false;
				if (isMovingUp) {
					currLevel ++;
					if (currLevel == MaxLevel)
						isMovingUp = false;
				} else {
					currLevel --;
					if (currLevel == MinLevel)
						isMovingUp = true;
				}
			} else {
				transform.position = Vector3.Lerp(start, end, polynInterp(progress));
			}
		}
	}
	float polynInterp(float x)
	{
		return -x * x * x + 2 * x * x;
	}

	public void BeginHover(bool isWhite) {
		if (isWhite != IsWhite || isAnimation)
			return;
		Debug.Log ("Begin Hover: " + isWhite);
		if ((isMovingUp && currLevel < MaxLevel) || (!isMovingUp && currLevel > MinLevel)) {
			isAnimation = true;
			startTime = Time.time;
		}
	}
}
