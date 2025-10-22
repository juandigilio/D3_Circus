using UnityEngine;

public class CharacterAudio : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event shootEvent;
    [SerializeField] private AK.Wwise.Event deathEvent;
    [SerializeField] private AK.Wwise.Event hitEvent;
    [SerializeField] private AK.Wwise.Event jumpEvent;
    [SerializeField] private AK.Wwise.Event pickAmmoEvent;
    [SerializeField] private AK.Wwise.Event pickHealthEvent;
    [SerializeField] private AK.Wwise.Event pickWeaponEvent;


    public void PlayShootSound()
    {
        shootEvent.Post(gameObject);
    }

    public void PlayDeathSound()
    {
        deathEvent.Post(gameObject);
    }

    public void PlayHitSound()
    {
        hitEvent.Post(gameObject);
    }

    public void PlayJumpSound()
    {
        jumpEvent.Post(gameObject);
    }

    public void PlayPickAmmoSound()
    {
        pickAmmoEvent.Post(gameObject);
    }

    public void PlayPickHealthSound()
    {
        pickHealthEvent.Post(gameObject);
    }

    public void PlayPickWeaponSound()
    {
        pickWeaponEvent.Post(gameObject);
    }
}
