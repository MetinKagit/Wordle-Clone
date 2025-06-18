using UnityEngine;
using TMPro;

public class Tile : MonoBehaviour
{
    private TextMeshProUGUI textMesh;
    public char letter { get; private set; }

    private void Awake()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
    }
    public void SetLetter(char letter)
    {
        if (textMesh != null)
        {
            this.letter = letter;
            textMesh.text = letter.ToString();
        }
    }
}
