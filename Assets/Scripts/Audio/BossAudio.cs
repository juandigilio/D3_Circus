using UnityEngine;

public class BossAudio : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event laughEvent;
    [SerializeField] private AK.Wwise.Event shootEvent;


    public void PlayLaughSound()
    {
        laughEvent.Post(gameObject);
    }

    public void PlayShootSound()
    {
        shootEvent.Post(gameObject);
    }
}
