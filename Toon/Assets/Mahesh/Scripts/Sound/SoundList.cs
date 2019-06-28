using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SoundList : MonoBehaviour
{
    public static SoundList instance;

    public AudioSource UIAudioSource;
    public AudioClip UINavClip;
    public AudioClip UISelectClip;
    public AudioClip OpenInventoryClip;

    private void Awake()
    {
        if (instance != null)
            Destroy(gameObject);
        else
            instance = this;
    }
}
