using UnityEngine;
using System.Collections;

public class CameraController : MonoBehaviour {

	public Transform Target;

	Vector3 distance;

	// Use this for initialization
	void Start () {
		distance = transform.position - Target.position;
	}
	
	// Update is called once per frame
	void Update () {
		transform.position = Target.position + distance;
	}
}
