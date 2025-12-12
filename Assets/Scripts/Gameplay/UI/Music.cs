using UnityEngine;

public class Music : MonoBehaviour
{
    [SerializeField] 
    private AudioClip musicSound;
    private AudioSource audioSource;
    void Awake()
    {
        audioSource = GetComponent<AudioSource>();
        audioSource.clip = musicSound;
        audioSource.loop = true;
    }

    void Start()
    {
        if (musicSound != null)
            audioSource.Play();
    }
}
