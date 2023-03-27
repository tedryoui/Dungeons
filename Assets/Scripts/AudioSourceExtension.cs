using System.Collections;
using UnityEngine;

public static class AudioSourceExtension
{
    public static IEnumerator PlayAndDestroy(this AudioSource audioSource, AudioClip clip)
    {
        audioSource.transform.parent = null;
        audioSource.PlayOneShot(clip);
        yield return new WaitForSecondsRealtime(clip.length);
        Object.Destroy(audioSource.gameObject);
    }
}