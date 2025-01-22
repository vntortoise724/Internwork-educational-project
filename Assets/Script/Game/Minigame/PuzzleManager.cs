using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;


public class PuzzleManager : MonoBehaviour
{
    [SerializeField]
    private GameObject UI;
    [SerializeField] 
    private Transform gameTransform;
    [SerializeField] 
    private Transform piecePrefab;
    [SerializeField]
    private int size;

    [SerializeField]
    private MinigameCompletion minigame;

    private List<Transform> pieces;
    private int emptyLocation;
    private bool shuffling = false;

    private CountDown currentMinigame;

    private void CreateGamePieces(float gapThickness)
    {
        //Độ rộng của mỗi mảnh ghép.
        float width = 1 / (float)size;
        for (int row = 0; row < size; row++)
        {
            for (int col = 0; col < size; col++)
            {
                Transform piece = Instantiate(piecePrefab, gameTransform);
                pieces.Add(piece);
                //Từng mảnh ghép sẽ được đặt vào bảng từ -1 đến 1.
                piece.localPosition = new Vector3(-1 + (2 * width * col) + width, +1 - (2 * width * row) - width, 0);
                piece.localScale = ((2 * width) - gapThickness) * Vector3.one;
                piece.name = $"{(row * size) + col}";
                // Tạo một ô trống ở gốc dưới bên phải.
                if ((row == size - 1 ) && (col == size - 1))
                {
                    emptyLocation = (size * size) - 1;
                    piece.gameObject.SetActive(false);
                } else {
                    float gap = gapThickness / 2;
                    Mesh mesh = piece.GetComponent<MeshFilter>().mesh;
                    Vector2[] uv = new Vector2[4];
                    // Trình tự tọa độ UV: (0, 1), (1, 1), (0, 0), (1, 0)
                    uv[0] = new Vector2((width * col) + gap, 1 - ((width * (row + 1)) - gap));
                    uv[1] = new Vector2((width * (col + 1)) - gap, 1 - ((width * (row + 1)) - gap));
                    uv[2] = new Vector2((width * col) + gap, 1 - ((width * row) + gap));
                    uv[3] = new Vector2((width * (col + 1)) - gap, 1 - ((width * row) + gap));
                    // Gán các UV mới vào mesh.
                    mesh.uv = uv;
                }
            }
        }
    }

    private bool CheckCompletion()
    {
        for (int i = 0; i < pieces.Count; i++)
        {
            if (pieces[i].name != $"{i}")
            {
                return false;
            }
        }
        return true;
    }

    private IEnumerator WaitShuffle(float duration)
    {
        yield return new WaitForSeconds(duration);
        Shuffle();
        shuffling = false;
    }

    private void Shuffle()
    {
        int count = 0;
        int last = 0;
        while (count < (size * size * size)) {
            // Chọn một vị trí ngẫu nhiên
            int rand = Random.Range(0, size * size);
            if (rand == last) { continue; }
            last = emptyLocation;
            // Thử những ô xung quanh để tìm hướng đi hợp lệ
            if (SwapIfValid(rand, -size, size))
            {
                count++;
            } else if (SwapIfValid(rand, +size, size))
            {
                count++; 
            } else if (SwapIfValid(rand, -1, 0))
            {
                count++;
            } else if (SwapIfValid(rand, +1, size - 1))
            {
                count++;
            }
        }
    }



    // Start is called before the first frame update
    void Start()
    {
        currentMinigame = FindObjectOfType<CountDown>();

        pieces = new List<Transform>();
        CreateGamePieces(0.01f);
        Shuffle();
    }

    // Update is called once per frame
    void Update()
    {
        // Gửi một ray kiểm tra lệnh tương tác chuột với một mảnh ghép.
        if (Input.GetMouseButtonDown(0))
        {
            RaycastHit2D hit = Physics2D.Raycast(Camera.main.ScreenToWorldPoint(Input.mousePosition), Vector2.zero);
            if (hit)
            {
                for (int i = 0; i < pieces.Count; i++)
                {
                    if (pieces[i] == hit.transform)
                        // Kiểm tra mọi hướng đi để tìm hướng đi hợp lệ.
                        // Break các hướng đi hợp lệ nhằm không bị hoán đổi.
                    {
                        if (SwapIfValid (i, -size, size)) { break; }
                        if (SwapIfValid (i, +size, size)) { break; }
                        if (SwapIfValid (i, -1, 0)) { break; }
                        if (SwapIfValid (i, +1, size - 1)) { break; } 
                    }
                }
            }
        }

        if (!shuffling && CheckCompletion())
        {
            SceneManager.LoadScene("NewSchoolScene");
        }

    }
    private bool SwapIfValid(int i, int offset, int colCheck)
    {
        if (((i % size) != colCheck) && ((i + offset) == emptyLocation))
        {
            // Hoán đổi vị trí trong game
            (pieces[i], pieces[i + offset]) = (pieces[i + offset], pieces[i]);
            // Hoán đổi Transform của các mảnh ghép
            (pieces[i].localPosition, pieces[i + offset].localPosition) = ((pieces[i + offset].localPosition, pieces[i].localPosition));
            // Cập nhật vị trí trống moidaden
            emptyLocation = i;
            return true;
        }
        return false;
    }
}
