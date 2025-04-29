using UnityEngine;
using TMPro;            // if you’re using TextMeshPro
// using UnityEngine.UI; // if you’re using UI.Text instead

public class GameOverUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI finalScoreText;
    // [SerializeField] private Text finalScoreText; // if you use UI.Text

    void Start()
    {
        // pull the XP we stashed in RunData
        int xp = RunData.FinalXP;

        if (finalScoreText != null)
            finalScoreText.text = $"Final Score\n{xp}";
    }
}