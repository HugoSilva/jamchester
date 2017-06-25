using UnityEngine;

public class RandomSound : MonoBehaviour
{
    public AudioClip[] sounds;

    void Start()
    {
        AudioClip sound = sounds[Random.Range(0, sounds.Length)];
        GetComponent<AudioSource>().PlayOneShot(sound);
    }
}