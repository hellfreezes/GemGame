using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public struct Piece
{
    public int x;
    public int y;
    public int col;
    public int row;
    public int kind;
    public int match;
}

public class GameController : MonoBehaviour {

    private Vector2 offset = new Vector2(48, 24);
    private Piece[,] grid = new Piece[10,10];

	// Use this for initialization
	void Start () {
		
	}
	
	// Update is called once per frame
	void Update () {
		
	}
}
