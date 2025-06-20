using System;
using UnityEngine;

public class Board : MonoBehaviour
{
    private static readonly KeyCode[] SUPPRTED_KEYS = new KeyCode[]
    {
        KeyCode.A, KeyCode.B, KeyCode.C, KeyCode.D, KeyCode.E,
        KeyCode.F, KeyCode.G, KeyCode.H, KeyCode.I, KeyCode.J,
        KeyCode.K, KeyCode.L, KeyCode.M, KeyCode.N, KeyCode.O,
        KeyCode.P, KeyCode.Q, KeyCode.R, KeyCode.S, KeyCode.T,
        KeyCode.U, KeyCode.V, KeyCode.W, KeyCode.X, KeyCode.Y,
        KeyCode.Z
    };

    private String[] solutions;
    private String[] validWords;

    private String word;

    private Row[] rows;
    private int rowIndex = 0;
    private int columnIndex = 0;

    private void Awake()
    {
        rows = GetComponentsInChildren<Row>();
        if (rows.Length == 0)
        {
            Debug.LogError("No rows found in the board.");
        }
    }

    public void Start()
    {
        LoadData();
        SetRandomWord();
    }
    public void LoadData()
    {
        TextAsset textFile = Resources.Load("official_wordle_all") as TextAsset;
        validWords = textFile.text.Split('\n');

        textFile = Resources.Load("official_wordle_common") as TextAsset;
        solutions = textFile.text.Split('\n');
    }

    private void SetRandomWord()
    {
        if (solutions.Length == 0)
        {
            Debug.LogError("No solutions available.");
            return;
        }

        int randomIndex = UnityEngine.Random.Range(0, solutions.Length);
        word = solutions[randomIndex].ToLower().Trim();
        Debug.Log($"Selected word: {word}");
    }


    void Update()
    {
        Row currentRow = rows[rowIndex];
        if (Input.GetKeyDown(KeyCode.Backspace))
        {
            columnIndex = Mathf.Max(columnIndex - 1, 0);
            currentRow.tiles[columnIndex].SetLetter(('\0'));
        }
        else if (columnIndex >= currentRow.tiles.Length)
        {
            if (Input.GetKeyDown(KeyCode.Return))
            {
                SubmitRow(currentRow);
            }
        }
        else
        {
            for (int i = 0; i < SUPPRTED_KEYS.Length; i++)
            {
                KeyCode key = SUPPRTED_KEYS[i];
                if (Input.GetKeyDown(key))
                {
                    currentRow.tiles[columnIndex].SetLetter((char)key);
                    columnIndex++;
                    break;
                }
            }
        }
    }

    private void SubmitRow(Row row)
    {
        for (int i = 0; i < row.tiles.Length; i++)
        {
            Tile tile = row.tiles[i];
            if (tile.letter == word[i])
            {

            }
            else if (word.Contains(tile.letter))
            {

            }
            else
            {

            }
        }
        rowIndex++;
        columnIndex = 0;
        if (rowIndex >= rows.Length)
        {
            Debug.Log("Game Over! No more rows available.");
            enabled = false;
        }
    }
}
