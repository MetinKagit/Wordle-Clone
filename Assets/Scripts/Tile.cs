using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Tile : MonoBehaviour
{
    [System.Serializable]
    public class State
    {
        public Color fillColor;
        public Color outlineColor;
    }
    private TextMeshProUGUI textMesh;
    private Image fill;
    private Outline outline;

    public State state { get; private set; }
    public char letter { get; private set; }

    private void Awake()
    {
        textMesh = GetComponentInChildren<TextMeshProUGUI>();
        fill = GetComponent<Image>();
        outline = GetComponent<Outline>();
    }
    public void SetLetter(char letter)
    {
        if (textMesh != null)
        {
            this.letter = letter;
            textMesh.text = letter.ToString();
        }
    }

    public void SetState(State state)
    {
        this.state = state;
        fill.color = state.fillColor;
        outline.effectColor = state.outlineColor;
    }
}
