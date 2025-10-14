using System.Collections;
using UnityEngine;

public class Boss : MonoBehaviour
{
    [SerializeField] private GameObject mouth;
    [SerializeField] private Transform mouthStart;
    [SerializeField] private Transform mouthEnd;
    [SerializeField] private float mouthSpeed = 1.5f;
    [SerializeField] private float openDuration = 2f;
    [SerializeField] private Transform leftCannon;
    [SerializeField] private Transform rightCannon;


    private bool isMouthOpen = false;
    private Coroutine mouthRoutine;


    void Start()
    {
        mouth.transform.position = mouthStart.position;
    }

    void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            if (!isMouthOpen)
                StartOpenMouth();
        }
    }

    private void StartOpenMouth()
    {
        if (mouthRoutine != null)
            StopCoroutine(mouthRoutine);

        mouthRoutine = StartCoroutine(OpenMouthRoutine());
    }

    private IEnumerator OpenMouthRoutine()
    {
        isMouthOpen = true;

        yield return MoveMouth(mouthStart.position, mouthEnd.position);

        yield return new WaitForSeconds(openDuration);

        yield return MoveMouth(mouthEnd.position, mouthStart.position);

        isMouthOpen = false;
        mouthRoutine = null;
    }

    private IEnumerator MoveMouth(Vector3 from, Vector3 to)
    {
        float t = 0f;
        while (t < 1f)
        {
            t += Time.deltaTime * mouthSpeed;
            mouth.transform.position = Vector3.Lerp(from, to, t);
            yield return null;
        }
    }
}
