using UnityEngine;

public class UIAudio : MonoBehaviour
{
    [SerializeField] private AK.Wwise.Event ambianceEvent;
    [SerializeField] private AK.Wwise.Event onClickEvent;
    [SerializeField] private AK.Wwise.Event onHoverEvent;


    public void PlayAmbianceSound()
    {
        ambianceEvent.Post(gameObject);
    }

    public void PlayClickSound()
    {
        onClickEvent.Post(gameObject);
    }

    public void PlayHoverSound()
    {
        onHoverEvent.Post(gameObject);
    }
}
