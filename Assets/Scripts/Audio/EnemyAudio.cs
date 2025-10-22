using UnityEngine;

public class EnemyAudio : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event hitEvent;
    [SerializeField] private AK.Wwise.Event shootEvent;
    [SerializeField] private AK.Wwise.Event slashEvent;


    public void PlayHitSound()
    {
        hitEvent.Post(gameObject);
    }

    public void PlayShootSound()
    {
        shootEvent.Post(gameObject);
    }

    public void PlaySlashSound()
    {
        slashEvent.Post(gameObject);
    }
}
