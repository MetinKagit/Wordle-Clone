using System;
using NUnit.Framework;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
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
    [Header("State")]
    public Tile.State emtyState;
    public Tile.State occupiedState;
    public Tile.State correctState;
    public Tile.State wrongSpotState;
    public Tile.State incorrectState;

    private String[] solutions;
    private String[] validWords;

    private String word;

    private Row[] rows;
    private int rowIndex = 0;
    private int columnIndex = 0;

    [Header("UI")]
    public TextMeshProUGUI messageText;
    public Button newWordButton;
    public Button tryAgainButton;


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

    public void NewGame()
    {
        ClearBoard();
        SetRandomWord();
        enabled = true;
    }

    public void TryAgain()
    {
        ClearBoard();
        enabled = true;
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
            currentRow.tiles[columnIndex].SetState(emtyState);
            messageText.gameObject.SetActive(false);
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
                    currentRow.tiles[columnIndex].SetState(occupiedState);
                    columnIndex++;
                    break;
                }
            }
        }
    }

    private void SubmitRow(Row row)
    {
        if (!IsValidWord(row.word))
        {
            messageText.text = "Invalid Word!";
            messageText.gameObject.SetActive(true);
            return;
        }
        string remaining = word;
        for (int i = 0; i < row.tiles.Length; i++)
        {
            Tile tile = row.tiles[i];
            if (tile.letter == word[i])
            {
                tile.SetState(correctState);
                remaining = remaining.Remove(i, 1);
                remaining = remaining.Insert(i, " ");
            }
            else if (!word.Contains(tile.letter))
            {
                tile.SetState(incorrectState);
            }
        }

        for (int i = 0; i < row.tiles.Length; i++)
        {
            Tile tile = row.tiles[i];
            if (tile.state != correctState && tile.state != incorrectState)
            {
                if (remaining.Contains(tile.letter))
                {
                    tile.SetState(wrongSpotState);
                    int index = remaining.IndexOf(tile.letter);
                    remaining = remaining.Remove(index, 1);
                    remaining = remaining.Insert(index, " ");
                }
                else
                {
                    tile.SetState(incorrectState);
                }
            }
        }

        if (HasWon(row))
        {
            enabled = false;
            messageText.text = "Congratulations! You Win!";
            messageText.gameObject.SetActive(true);
            return;
        }

        rowIndex++;
        columnIndex = 0;
        if (rowIndex >= rows.Length)
        {
            messageText.text = "Game Over!";
            messageText.gameObject.SetActive(true);
            enabled = false;
        }
    }

    private bool IsValidWord(string word)
    {
        for (int i = 0; i < validWords.Length; i++)
        {
            if (validWords[i] == word)
            {
                return true;
            }
        }
        return false;
    }

    private bool HasWon(Row row)
    {
        for (int i = 0; i < row.tiles.Length; i++)
        {
            if (row.tiles[i].state != correctState)
            {
                return false;
            }
        }
        return true;
    }

    private void OnEnable()
    {
        tryAgainButton.gameObject.SetActive(false);
        newWordButton.gameObject.SetActive(false);

    }
    private void OnDisable()
    {
        tryAgainButton.gameObject.SetActive(true);
        newWordButton.gameObject.SetActive(true);
    }

    private void ClearBoard()
    {
        for (int i = 0; i < rows.Length; i++)
        {
            for (int j = 0; j < rows[i].tiles.Length; j++)
            {
                rows[i].tiles[j].SetLetter('\0');
                rows[i].tiles[j].SetState(emtyState);
            }
        }
        rowIndex = 0;
        columnIndex = 0;
        messageText.gameObject.SetActive(false);
    }
}
