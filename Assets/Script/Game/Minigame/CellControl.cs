using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class CellControl : MonoBehaviour
{
    public BoardControl board;

    public Sprite xCell;
    public Sprite oCell;

    public int row;
    public int col;

    private Image image;
    private Button btn;
    // Start is called before the first frame update
    void Awake()
    {
        image = GetComponent<Image>();
        btn = GetComponent<Button>();
        btn.onClick.AddListener(OnClick);
    }

    private void Start()
    {
        board = FindObjectOfType<BoardControl>();
    }

    public void ChangeImage(string cell)
    {
        if (cell == "x")
        {
            image.sprite = xCell;
        } 
        else if (cell == "o")
        {
            image.sprite = oCell;
        }
    }

    public void OnClick()
    {
        if (board.matrix[row, col] == "")
        {
            board.matrix[row, col] = board.currentTurn;
            ChangeImage(board.currentTurn);

            if (board.CheckIfWon(row, col, board.currentTurn)) 
            {
                Debug.Log(board.currentTurn + "Won");
            }

            if (board.currentTurn == "x")
            {
                board.currentTurn = "o";
            } else if (board.currentTurn == "o")
            {
                board.AITurn();
            }
        }
    }
}
