using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TableController : MonoBehaviour {
    [SerializeField]
    private int width = 8;
    [SerializeField]
    private int height = 8;
    [SerializeField]
    private GameObject cellPrefab;
    [SerializeField]
    private Sprite[] gemBase;

    private Vector2 offset = new Vector2(48, 24);
    private Gem[,] grid;

    private Vector2 firstSelection;
    private Vector2 secondSelection;
    private int click = 0;

    private static TableController instance;

    public static TableController Instance
    {
        get
        {
            return instance;
        }
    }

    // Use this for initialization
    void Start () {
        instance = this;
        CreateGrid();

    }
	
	// Update is called once per frame
	void Update () {
		
	}

    private void CreateGrid()
    {
        grid = new Gem[width, height];
        for (int i = 0; i < width; i++)
        {
            for (int j = 0; j < height; j++)
            {
                GameObject newCell = (GameObject)Instantiate(cellPrefab);
                newCell.transform.parent = transform;
                newCell.name = "Cell[" + i + ", " + j + "]";
                newCell.transform.localPosition = new Vector3(i + 0.5f, j + 0.5f, 0);

                grid[i, j].kind = Random.Range(0, gemBase.Length-1);
                grid[i, j].col = i;
                grid[i, j].row = j;
                grid[i, j].sprite = gemBase[grid[i, j].kind];

                grid[i, j].go = newCell;

                newCell.GetComponent<GemRenderer>().ChangeGem(grid[i,j]);
            }
        }
    }

    public void AcceptSelection(Gem data)
    {
        click++;
        
        if (click == 2)
        {
            secondSelection = new Vector2(data.col, data.row);
            if (Mathf.Abs(secondSelection.x - firstSelection.x) + Mathf.Abs(secondSelection.y - firstSelection.y) == 1)
            {
                SwapGems(ref grid[(int)firstSelection.x, (int)firstSelection.y], ref grid[(int)secondSelection.x, (int)secondSelection.y]);
                //isSwap = true;
                click = 0;
            }
            else
            {
                click = 1;
            }
        }
        if (click == 1)
        {
            firstSelection = new Vector2(data.col, data.row);
        }
    }

    private void SwapGems(ref Gem gem1, ref Gem gem2)
    {
        Gem tempG1 = gem1;
        Gem tempG2 = gem2;

        gem1 = gem2;
        gem2 = tempG1;

        gem1.go.GetComponent<GemRenderer>().ChangeGem(tempG1);
        gem2.go.GetComponent<GemRenderer>().ChangeGem(tempG2);
    }
}
