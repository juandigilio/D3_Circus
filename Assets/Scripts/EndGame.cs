using System.Collections;
using TMPro;
using UnityEngine;

public class EndGame : MonoBehaviour
{
    [Header("UI References")]
    [SerializeField] private GameObject gameOverText;
    [SerializeField] private GameObject winText;
    [SerializeField] private GameObject creditsButton;
    [SerializeField] private GameObject menuButton;

    [Header("Score Texts")]
    [SerializeField] private TextMeshProUGUI enemiesKilled;
    [SerializeField] private TextMeshProUGUI killsSCore;
    [SerializeField] private TextMeshProUGUI itemsCollected;
    [SerializeField] private TextMeshProUGUI collectablesScore;
    [SerializeField] private TextMeshProUGUI totalTime;
    [SerializeField] private TextMeshProUGUI timeBonus;
    [SerializeField] private TextMeshProUGUI totalScore;
    [SerializeField] private TextMeshProUGUI maxScore;

    [Header("Animation Settings")]
    [SerializeField] private float countDuration = 1f;

    private void Start()
    {
        Init();

        StartCoroutine(AnimateScores(PlayerInfo.GetGameData()));

        PlayerInfo.SavePlayerData();
    }

    private void Init()
    {
        gameOverText.SetActive(PlayerInfo.GetGameData().gameOver);
        winText.SetActive(!PlayerInfo.GetGameData().gameOver);
        creditsButton.SetActive(false);
        menuButton.SetActive(false);

        enemiesKilled.text = "0";
        killsSCore.text = "0";
        itemsCollected.text = "0";
        collectablesScore.text = "0";
        timeBonus.text = "0 %";
        totalScore.text = "0";

        maxScore.text = PlayerInfo.GetGameData().maxScore.ToString();

        float t = PlayerInfo.GetGameData().totalTime;
        int minutes = Mathf.FloorToInt(t / 60);
        int seconds = Mathf.FloorToInt(t % 60);
        totalTime.text = $"{minutes:00}:{seconds:00}.{(t % 1f) * 10:0}";
    }

    private IEnumerator AnimateScores(GameData data)
    {
        int total = 0;

        yield return AnimateInt(enemiesKilled, 0, data.enemiesKilled);

        yield return AnimateInt(killsSCore, 0, data.killsScore);
        total += data.killsScore;
        yield return AnimateInt(totalScore, 0, total);

        yield return AnimateInt(itemsCollected, 0, data.collectedItems);

        yield return AnimateInt(collectablesScore, 0, data.collectablesScore);
        int previousTotal = total;
        total += data.collectablesScore;
        yield return AnimateInt(totalScore, previousTotal, total);

        yield return AnimateFloat(timeBonus, 0f, data.timeBonus, " %");
        previousTotal = total;
        total += Mathf.RoundToInt(data.timeBonus);
        yield return AnimateInt(totalScore, previousTotal, total);

        creditsButton.SetActive(!PlayerInfo.GetGameData().gameOver);
        menuButton.SetActive(PlayerInfo.GetGameData().gameOver);
    }

    private IEnumerator AnimateInt(TextMeshProUGUI text, int from, int to)
    {
        float elapsed = 0f;

        if (to != from)
        {
            while (elapsed < countDuration)
            {
                elapsed += Time.deltaTime;
                int value = Mathf.RoundToInt(Mathf.Lerp(from, to, elapsed / countDuration));
                text.text = value.ToString();
                yield return null;
            }
        }

        text.text = to.ToString();
    }

    private IEnumerator AnimateFloat(TextMeshProUGUI text, float from, float to, string suffix = "")
    {
        float elapsed = 0f;

        while (elapsed < countDuration)
        {
            elapsed += Time.deltaTime;
            float value = Mathf.Lerp(from, to, elapsed / countDuration);
            text.text = $"{value:F2}{suffix}";
            yield return null;
        }

        text.text = $"{to:F2}{suffix}";
    }
}
