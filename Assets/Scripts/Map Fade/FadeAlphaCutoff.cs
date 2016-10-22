using UnityEngine;
using System.Collections;

public class FadeAlphaCutoff : MonoBehaviour
{
    // assign in inspector
    public GameObject backgroundFade;
    public GameObject backgroundFadeFire;

    // Use this for initialization
    void Start ()
    {
        //FadeMapIn();
    }

    public void FadeMapIn()
    {
        StartCoroutine(ChangeAlpha(backgroundFade.transform, 3f));
        StartCoroutine(ChangeAlpha(backgroundFadeFire.transform, 3f));
    }

    private float alpha = 0;

    IEnumerator ChangeAlpha(Transform target, float time)
    {
        Renderer renderer = target.GetComponent<Renderer>();

        float originalAlpha = renderer.material.GetFloat("_Cutoff");
        float destinationAlpha = 1f;

        float currentTime = 0.0f;

        do
        {
            renderer.material.SetFloat("_Cutoff", Mathf.Lerp(originalAlpha, destinationAlpha, currentTime / time));
            currentTime += Time.deltaTime;
            yield return null;
        }
        while (currentTime <= time);

        // remove fade covers
        Destroy(backgroundFade);
        Destroy(backgroundFadeFire);
    }
}
