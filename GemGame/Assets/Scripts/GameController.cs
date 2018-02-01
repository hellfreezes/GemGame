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
    public GameObject go;
    public SpriteRenderer spriteRenderer;
}

public class GameController : MonoBehaviour {
    [SerializeField]
    private GameObject cellPrefab;
    [SerializeField]
    private Transform table;
    [SerializeField]
    private Sprite[] gemBase;

    private Vector2 offset = new Vector2(48, 24);
    private Piece[,] grid = new Piece[10,10];


    private int cellSize = 54;
    private int click = 0;
    private int x0, x1, y0, y1;
    private int isMoving = 0;
    private bool isSwap = false;
    private int score = 0;

	// Use this for initialization
	void Start () {
        CreateGrid();

    }
	
	// Update is called once per frame
	void Update () {
        PlayerInputHandler();
    }

    private void CreateGrid()
    {
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                GameObject newCell = (GameObject)Instantiate(cellPrefab);
                newCell.transform.parent = table;
                newCell.name = "Cell[" + i + ", " + j + "]";
                newCell.transform.localPosition = new Vector3(i + 0.5f, j + 0.5f, 0);

                grid[i, j].kind = Random.Range(0, 6);
                grid[i, j].col = j;
                grid[i, j].row = i;
                grid[i, j].spriteRenderer = newCell.GetComponent<SpriteRenderer>();
                
                grid[i, j].go = newCell;
                grid[i, j].spriteRenderer.sprite = gemBase[grid[i, j].kind];
            }
        }
    }

    private void Swap(ref Piece p1, ref Piece p2)
    {
        Piece tempP1 = p1;
        Piece tempP2 = p2;

        p1 = p2;
        p2 = tempP1;

        p1.spriteRenderer.sprite = gemBase[p1.kind];
        p2.spriteRenderer.sprite = gemBase[p2.kind];

        StartCoroutine(MoveTo(p1.go, tempP1.go.transform.position, p2.go, tempP2.go.transform.position));
    }

    private void CheckMatch()
    {
        //Проверка совпадений
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                if (IsCellVaild(i + 1))
                {
                    if (grid[i, j].kind == grid[i + 1, j].kind)
                    {
                        if (IsCellVaild(i - 1))
                        {
                            if (grid[i, j].kind == grid[i - 1, j].kind)
                            {
                                for (int n = -1; n <= 1; n++)
                                {
                                    if (IsCellVaild(i + n))
                                    {
                                        grid[i + n, j].match++;
                                    }
                                }
                            }
                        }
                    }
                }

                if (IsCellVaild(j + 1))
                {
                    if (grid[i, j].kind == grid[i, j + 1].kind)
                    {
                        if (IsCellVaild(j - 1))
                        {
                            if (grid[i, j].kind == grid[i, j - 1].kind)
                            {

                                for (int n = -1; n <= 1; n++)
                                {
                                    if (IsCellVaild(j + n))
                                    {
                                        grid[i, j + n].match++;
                                    }
                                }
                            }
                        }
                    }
                }
            }
        }

        //Начисление очков
        int localScore = 0;
        for (int i = 0; i < 8; i++)
        {
            for (int j = 0; j < 8; j++)
            {
                localScore += grid[i, j].match;
            }
        }

        //Если очков не начислено, возвращаем всё назад
        if (isSwap && isMoving == 0)
        {
            if (localScore == 0)
                Swap(ref grid[x0, y0], ref grid[x1, y1]);
            isSwap = false;
        }

        //Обновление сетки
        if (isMoving == 0)
        {
            for (int j = 7; j >= 0; j--)
            {
                for (int i = 0; i < 8; i++)
                {
                    if (grid[i,j].match > 0)
                    {
                        for (int n = i; n >= 0; n--)
                        {
                            if (grid[n, j].match == 0)
                            {
                                Swap(ref grid[j,n], ref grid[j,i]);
                                break;
                            }
                        }
                    }
                }
            }


            for (int j = 0; j < 8; j++)
            {
                for (int i = 7; i >= 0; i--)
                {
                    if (grid[i, j].match == 1)
                    {
                        grid[i, j].kind = Random.Range(0, 6);
                        grid[i, j].match = 0;
                    }
                }
            }
        }
    }

    private bool IsCellVaild(int v)
    {
        if (v >= 0 && v<= 7)
        {
            return true;
        } else
        {
            return false;
        }
    }

    IEnumerator MoveTo(GameObject go1, Vector3 moveTo1, GameObject go2, Vector3 moveTo2)
    {
        isMoving++;
        
        bool end = false;
        while (!end)
        {
            go1.transform.position = Vector3.Lerp(go1.transform.position, moveTo1, 10f * Time.deltaTime);
            go2.transform.position = Vector3.Lerp(go2.transform.position, moveTo2, 10f * Time.deltaTime);
            if (go1.transform.position == moveTo1 && go2.transform.position == moveTo2)
            {
                end = true;
            }
            yield return null;
        }
        isMoving--;
        CheckMatch();
    }

    private void PlayerInputHandler()
    {
        if (isMoving == 0 && Input.GetMouseButtonDown(0))
        {
            int cx, cy;
            float x = Input.mousePosition.x-offset.x;
            float y = Input.mousePosition.y-offset.y;


            if (x >= 0 && x <= 740 && y >= 0 && y <= 480)
            {
                cx = Mathf.CeilToInt(x / cellSize) - 1;
                cy = Mathf.CeilToInt(y / cellSize) - 1;

                click++;
                if (click == 2)
                {
                    x1 = cx;
                    y1 = cy;
                    if (Mathf.Abs(x1 - x0) + Mathf.Abs(y1 - y0) == 1)
                    {
                        Swap(ref grid[x0, y0], ref grid[x1, y1]);
                        isSwap = true;
                        click = 0;
                    }
                    else
                    {
                        click = 1;
                    }
                }
                if (click == 1)
                {
                    x0 = cx;
                    y0 = cy;
                }
            }
        }
    }
}
