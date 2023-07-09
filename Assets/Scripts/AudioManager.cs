using UnityEngine;

public class AudioManager : MonoBehaviour
{

    public AudioSource source;

    [Header("Sounds")]

    public AudioClip button;
    public AudioClip menu;
    public AudioClip shotgun;

    // Singleton pattern
    static public AudioManager I = null;
    
    void Awake() {
        if (I == null) I = this;
        else if (I != this) Destroy(gameObject);
    }

    public void PlaySound(AudioClip clip) {
        source.PlayOneShot(clip);
    }

}
