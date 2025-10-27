using TMPro;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject winText;

    [SerializeField] private TextMeshProUGUI maxScore;
    [SerializeField] private TextMeshProUGUI actualScore;
    [SerializeField] private TextMeshProUGUI totalTime;
    [SerializeField] private TextMeshProUGUI enemiesKilled;

    private void Start()
    {
        bool isGameOver = PlayerInfo.IsGameOver();

        gameObject.SetActive(isGameOver);
        winText.SetActive(!isGameOver);

        maxScore.text = PlayerInfo.GetMaxScore().ToString();
        actualScore.text = PlayerInfo.GetActualScore().ToString();
        totalTime.text = PlayerInfo.GetTotalTime().ToString("F2") + "s";
        enemiesKilled.text = PlayerInfo.GetEnemiesKilled().ToString();
    }
}
