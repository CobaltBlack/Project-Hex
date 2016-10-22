using UnityEngine;
using System.Collections;

// TECHOLOGY ^ TM

public class Mask : MonoBehaviour
{
    // assign in inspector
    // consider making multiple different unmaskCluster and choosing randomly to increase the feeling of randomness in line appearing
    public GameObject unmaskCluster;

    public void SpawnUnmasker(Vector2 spawnPosition)
    {
        GameObject instance = Instantiate(unmaskCluster, spawnPosition, Quaternion.identity) as GameObject;

        for (int i = 0; i < instance.transform.childCount; ++i)
        {
            StartCoroutine(Resize(instance.transform.GetChild(i), 1.5f));
        }
    }

    private IEnumerator Resize(Transform target, float time)
    {
        Vector3 originalScale = target.localScale;
        Vector3 destinationScale = new Vector3(1.0f, 1.0f, 1.0f);

        float currentTime = 0.0f;

        do
        {
            target.localScale = Vector3.Lerp(originalScale, destinationScale, currentTime / time);
            currentTime += Time.deltaTime;
            yield return null;
        }
        while (currentTime <= time);
    }
}
