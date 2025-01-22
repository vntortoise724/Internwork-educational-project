using System.Collections;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using UnityEngine;
using UnityEngine.UI;

public class BoardControl : MonoBehaviour
{
    [Header("Board Setup")]
    public GameObject cell;
    public Transform board;
    public GridLayoutGroup layout;

    [Header("Change Board Size")]
    public int size;

    [Header("Status")]
    public string currentTurn = "x";
    public string[,] matrix;

    public int lastRow;
    public int lastCol;

    // Start is called before the first frame update
    void Start()
    {
        matrix = new string[size, size];
        layout.constraintCount = size;
        CreateBoard();
    }

    private void CreateBoard()
    {
        for (int i = 0; i < size; i++)
        {
            for(int j = 0; j < size; j++)
            {
                GameObject cellTrans = Instantiate(cell, board);
                CellControl cellcon = cellTrans.GetComponent<CellControl>();
                cellcon.row = i;
                cellcon.col = j;
                matrix[i, j] = "";
            } 
        }
    }

    public bool CheckIfWon(int row, int col, string player)
    {
        bool result = false;
        int count;
        //Vertical check
        count = 0;
        for (int i = row - 1; i >= 0; i-- ) //Up
        {
            if (matrix[i, col] == player)
            {
                count++;
            } else break;
        }

        for (int i = row + 1; i < size; i++) //Down
        {
            if (matrix[i, col] == player)
            {
                count++;
            } else break;
        }

        if (count + 1 >= 5) result = true;

        //Horizontal check
        count = 0;
        for (int i = col - 1; i >= 0; i--) //Left
        {
            if (matrix[row, i] == player)
            {
                count++;
            }
            else break;
        }

        for (int i = col + 1; i < size; i++) //Right
        {
            if (matrix[row, i] == player)
            {
                count++;
            }
            else break;
        }
        if (count + 1 >= 5) result = true;

        //Diagonal check
        count = 0;
        for (int i = 1; i <= Mathf.Min(row, col); i++) //Up Left 
        {
            if (matrix[row - i , col - i] == player)
            {
                count++;
            }
            else break;
        }

        for (int i = 1; i < Mathf.Min(size - row, size - col); i++) //Down Right 
        {
            if (matrix[row + i, col + i] == player)
            {
                count++;
            }
            else break;
        }
        if (count + 1 >= 5) result = true;

        count = 0;
        for (int i = 1; i <= Mathf.Min(row, size - col - 1); i++) //Up Right 
        {
            if (matrix[row - i, col + i] == player)
            {
                count++;
            }
            else break;
        }

        for (int i = 1; i < Mathf.Min(size - row, col + 1); i++) //Down Left
        {
            if (matrix[row + i, col - i] == player)
            {
                count++;
            }
            else break;
        }
        if (count + 1 >= 5) result = true;

        return result;
    }

    private List<(int, int)> PossibleMoves()
    {
        List<(int, int)> moves = new();
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                if (matrix[row, col] == "")
                {
                    moves.Add((row, col));
                }
            }
        }
        return moves;
    }

    private int EvaluateBoard()
    {
        int score = 0;

        //Rows
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size - 4; col++)
            {
                score += EvaluateLine(row, col, 0, 1);
            }
        }

        //Columns
        for (int col = 0; col < size; col++)
        {
            for (int row = 0; row < size - 4; row++)
            {
                score += EvaluateLine(row, col, 1, 0);
            }
        }

        //Diagonals
        for (int row = 0; row < size - 4; row++)
        {
            for (int col = 0; col < size - 4; col++)
            {
                score += EvaluateLine(row, col, 1, 1);
            }
        }

        for (int row = 4; row < size; row++)
        {
            for (int col = 0; col < size - 4; col++)
            {
                score += EvaluateLine(row, col, -1, 1);
            }
        }

        return score;
    }

    private int EvaluateLine(int row, int col, int rowIncre, int colIncre)
    {
        int score = 0;
        int xCount = 0;
        int oCount = 0;

        for (int i = 0; i < 5; i++)
        {
            if (matrix[row + i * rowIncre, col + i * colIncre] == "x")
                xCount++;
            else if (matrix[row + i * rowIncre, col + i * colIncre] == "o")
                oCount++;
        }

        if (xCount > 0 && oCount > 0)
            return 0;
        else if (xCount > 0)
        {
            score += xCount switch
            {
                5 => 10000,//Win
                4 => 1000,
                3 => 100,
                2 => 10,
                _ => 1,
            };
        }
        else if (oCount > 0)
        {
            score += oCount switch
            {
                5 => 10000,//Lose
                4 => 1000,
                3 => 100,
                2 => 10,
                _ => 1,
            };
        }
        return score;
    }

    private readonly Dictionary<string, int> transpositionTable = new();

    private List<(int, int)> PossibleOrder()
    {
        List<(int, int)> moves = PossibleMoves();
        moves.Sort((move1, move2) =>
        {
            matrix[move1.Item1, move2.Item2] = "x";
            int score1 = EvaluateBoard();
            matrix[move1.Item1, move2.Item2] = "";

            matrix[move2.Item1, move2.Item2] = "x";
            int score2 = EvaluateBoard();
            matrix[move2.Item1, move2.Item2] = "";

            return score2.CompareTo(score1);
        });

        return moves;
    }

    private int Minimax(int depth, int alpha, int beta, bool isMaximizing)
    {
        string boardKey = GetBoardKey();
        if (transpositionTable.ContainsKey(boardKey))
        {
            return transpositionTable[boardKey];
        }

        int evaluation = EvaluateBoard();
        if (Mathf.Abs(evaluation) >= 100000 || depth == 0)
        {
            return evaluation;
        }

        List<(int, int)> possibleMoves = PossibleOrder();

        int bestValue;
        if (isMaximizing)
        {
            bestValue = int.MinValue;
            foreach (var move in possibleMoves)
            {
                matrix[move.Item1, move.Item2] = "x";
                lastRow = move.Item1;
                lastCol = move.Item2;
                int eval = Minimax(depth - 1, alpha, beta, false);
                matrix[move.Item1, move.Item2] = "";
                bestValue = Mathf.Max(bestValue, eval);
                alpha = Mathf.Max(alpha, eval);
                if (beta <= alpha)
                {
                    break;
                }
            }
        }
        else
        {
            bestValue = int.MaxValue;
            foreach (var move in possibleMoves)
            {
                matrix[move.Item1, move.Item2] = "o";
                lastRow = move.Item1;
                lastCol= move.Item2;
                int eval = Minimax(depth - 1, alpha, beta, true);
                matrix[move.Item1, move.Item2] = "";
                bestValue = Mathf.Min(bestValue, eval);
                beta = Mathf.Min(beta, eval);
                if (beta <= alpha)
                {
                    break;
                }
            }
        }
        transpositionTable[boardKey] = bestValue;
        return bestValue;
    }

    private string GetBoardKey()
    {
        string key = "";
        for (int i = 0; i < size; i++)
        {
            for (int j = 0; j < size; j++)
            {
                key += matrix[i, j] == "" ? "-" : matrix[i, j];
            }
        }
        return key;
    }

    public (int, int) BestMove(int maxDepth)
    {
        int bestValue = int.MinValue;
        (int, int) bestMove = (-1, -1);

        for (int depth = 1; depth <= maxDepth; depth++)
        {
            foreach (var move in PossibleMoves())
            {
                matrix[move.Item1, move.Item2] = "x";
                lastRow = move.Item1;
                lastCol = move.Item2;
                int moveValue = Minimax(depth - 1, int.MinValue, int.MaxValue, false);
                matrix[move.Item1, move.Item2] = "";

                if (moveValue > bestValue)
                {
                    bestMove = move;
                    bestValue = moveValue;
                }
            }
        }   
        return bestMove;
    }

    public void AITurn()
    {
        (int, int) bestMove = BestMove(3);
        if (bestMove.Item1 != -1 && bestMove.Item2 != -1)
        {
            matrix[bestMove.Item1, bestMove.Item2] = "x";
            lastRow = bestMove.Item1;
            lastCol = bestMove.Item2;
            currentTurn = "o";
            CellControl cell = board.GetChild(bestMove.Item1 * size + bestMove.Item2).GetComponent<CellControl>();
            cell.ChangeImage("x");
        }
    }
}
