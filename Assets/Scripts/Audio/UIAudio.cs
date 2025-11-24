using UnityEngine;

public class UIAudio : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event onClickEvent;
    [SerializeField] private AK.Wwise.Event onHoverEvent;

    [SerializeField] private AK.Wwise.Event smallCoinEvent;
    [SerializeField] private AK.Wwise.Event bigCoinEvent;
    [SerializeField] private AK.Wwise.Event ammoEvent;
    [SerializeField] private AK.Wwise.Event healthEvent;
    [SerializeField] private AK.Wwise.Event ballonPopEvent;
    [SerializeField] private AK.Wwise.Event scoreEvent;



    public void PlayClickSound()
    {
        onClickEvent.Post(gameObject);
    }

    public void PlayHoverSound()
    {
        onHoverEvent.Post(gameObject);
    }

    public void PlaySmallCoinSound()
    {
        smallCoinEvent.Post(gameObject);
    }

    public void PlayBigCoinSound()
    {
        bigCoinEvent.Post(gameObject);
    }

    public void PlayBallonPopSound()
    {
        ballonPopEvent.Post(gameObject);
    }

    public void PlayScoreSound()
    {
        scoreEvent.Post(gameObject);
    }

    public void PlayAmmoSound()
    {
        ammoEvent.Post(gameObject);
    }

    public void PlayHealthSound()
    {
        healthEvent.Post(gameObject);
    }
}
