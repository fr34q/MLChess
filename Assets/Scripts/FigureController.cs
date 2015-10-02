using UnityEngine;
using System.Collections;

public class FigureController : MonoBehaviour {

	// 0 Bauer, 1 Turm, 2 Pferd, 3 Springer, 4 Dame, 5 König
	public int FigureType;
	public bool isWhite;

	TextMesh posText;
	BretterControl2 BretterWeiß;
	BretterControl2 BretterSchwarz;

	// Use this for initialization
	void Start () {
		posText = GameObject.Find ("Positionstext").GetComponent<TextMesh>();
		BretterWeiß = GameObject.Find ("Bretter Weiß").GetComponent<BretterControl2> ();
		BretterSchwarz = GameObject.Find ("Bretter Schwarz").GetComponent<BretterControl2> ();
	}
	
	// Update is called once per frame
	void Update () {
	
	}

	void OnMouseDown() {
		Debug.Log ("Click registered: " + transform.name);
		FeldControl2 fc = transform.parent.parent.GetComponent<FeldControl2> ();
		posText.text = "B: " + (fc.Board + 1) + " - P: " + fc.Field;
		if (fc.tag == "Weiß") {
			BretterWeiß.BeginAnimation ();
		} else {
			BretterSchwarz.BeginAnimation();
		}
	}
}
