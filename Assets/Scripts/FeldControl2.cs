using UnityEngine;
using System.Collections;

public class FeldControl2 : MonoBehaviour {

	public int Row;
	public int Line;
	public int BoardStart;

	public int Board {
		get {
			return _board;
		}
		set {
			while (value < 0)
				value += 4;
			_board = value % 4;
		}
	}
	int _board;


	public string Field {
		get {
			return _field;
		}
	}
	string _field;

	// Use this for initialization
	void Start () {
		_field = ((char)('A' + Line)).ToString() + ((char)('1' + Row)).ToString();
		Board = BoardStart;
		int fig = ChessBoard.GetInitialFigureId (Row, Line);
		if (fig != -1) {
			Transform obj = (Transform)Instantiate(ChessBoard.GetPrefabById(fig, ChessBoard.IsIntialFigureWhite(Row)), transform.position, Quaternion.Euler(0,90*Board,0));
			obj.parent = transform;
		}

	}
	
	// Update is called once per frame
	void Update () {
	
	}
}

public static class ChessBoard {
	// 0 Bauer, 1 Turm, 2 Pferd, 3 Springer, 4 Dame, 5 König

	static int[] initBoard = {1,2,3,4,5,3,2,1};
	static string[] figNames = {"Bauer", "Turm", "Pferd", "Läufer", "Dame", "König"};
	static Transform[] figPrefabs = new Transform[12];

	public static int GetInitialFigureId(int row, int line) {
		if (row > 1 && row < 6)
			return -1; // No figure
		if (row == 1 || row == 6)
			return 0; // Bauer
		return initBoard[line];
	}
	public static string GetInitialFigureName(int row, int line) {
		int fig = GetInitialFigureId (row, line);
		if (fig == -1) {
			Debug.LogError("No figure found!");
			return "";
		}
		return GetFigureNameById(fig, IsIntialFigureWhite(row));
	}
	public static bool IsIntialFigureWhite(int row) {
		return row < 4;
	}

	public static Transform GetPrefabById(int id, bool isWhite) {
		int index = ((isWhite) ? 0 : 6) + id;
		if (figPrefabs [index] != null)
			return figPrefabs [index];

		Transform res = Resources.Load<Transform>("Models/" + GetFigureNameById(id, isWhite));
		figPrefabs [index] = res;
		return res;
	}
	public static string GetFigureNameById(int id, bool isWhite)
	{
		return (isWhite ? "W" : "S") + " " + figNames [id];
	}
}
