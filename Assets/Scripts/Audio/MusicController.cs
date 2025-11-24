using UnityEngine;

public class MusicController : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event playAmbienceEvent;
    [SerializeField] private AK.Wwise.Event menuMusic;

    [SerializeField] private AK.Wwise.State gameStartState;
    [SerializeField] private AK.Wwise.State levelState;
    [SerializeField] private AK.Wwise.State bossState;
    [SerializeField] private AK.Wwise.State deathState;
    [SerializeField] private AK.Wwise.State creditsState;

    private void Start()
    {
        playAmbienceEvent.Post(gameObject);
        menuMusic.Post(gameObject);

        gameStartState.SetValue();
    }

    public void SetGameStart()
    {
        gameStartState.SetValue();
    }

    public void SetLevelState()
    {
        levelState.SetValue();
    }

    public void SetBossState()
    {
        bossState.SetValue();
    }

    public void SetDeathState()
    {
        deathState.SetValue();
    }

    public void SetCreditsState()
    {
        creditsState.SetValue();
    }
}
