using UnityEngine;
using TMPro;

public class Score : MonoBehaviour
{
    public static Score Instance { get; private set; }

    [SerializeField]
    private TextMeshProUGUI textMesh;

    private int score = 0;

    void Awake()
    {
        Instance = this;
    }

    private void Start()
    {
        textMesh.text = score.ToString();
    }

    public void IncrementScore(int increment)
    {
        score += increment;
        textMesh.text = score.ToString();
    }
}
