using TMPro;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject creditsButton;
    [SerializeField] private GameObject menuButton;

    [SerializeField] private TextMeshProUGUI enemiesKilled;
    [SerializeField] private TextMeshProUGUI killsSCore;
    [SerializeField] private TextMeshProUGUI itemsCollected;
    [SerializeField] private TextMeshProUGUI collectablesScore;
    [SerializeField] private TextMeshProUGUI totalTime;
    [SerializeField] private TextMeshProUGUI timeBonus;
    [SerializeField] private TextMeshProUGUI totalScore;
    [SerializeField] private TextMeshProUGUI maxScore;

    private void Start()
    {
        bool isGameOver = PlayerInfo.GetGameData().gameOver;

        gameOverText.SetActive(isGameOver);
        winText.SetActive(!isGameOver);
        creditsButton.SetActive(!isGameOver);
        menuButton.SetActive(isGameOver);

        killsSCore.text = PlayerInfo.GetGameData().killsScore.ToString();
        enemiesKilled.text = PlayerInfo.GetGameData().enemiesKilled.ToString();

        collectablesScore.text = PlayerInfo.GetGameData().collectablesScore.ToString();
        itemsCollected.text = PlayerInfo.GetGameData().collectedItems.ToString();

        timeBonus.text = $"{PlayerInfo.GetGameData().timeBonus:F2} %";
        totalScore.text = PlayerInfo.GetGameData().totalScore.ToString();

        maxScore.text = PlayerInfo.GetGameData().maxScore.ToString();

        float newTime = PlayerInfo.GetGameData().totalTime;
        int minutes = Mathf.FloorToInt(newTime / 60);
        int seconds = Mathf.FloorToInt(newTime % 60);

        totalTime.text = $"{minutes:00}:{seconds:00}.{(newTime % 1f) * 10:0}";

        PlayerInfo.SavePlayerData();
    }
}
