// COMP30019 - Graphics and Interaction
// (c) University of Melbourne, 2022

using UnityEngine;

public class PlaySoundOnStart : MonoBehaviour
{
    [SerializeField] private string soundPath;
    [SerializeField] private float volume = 1.0f;
    [SerializeField] private float pitch = 1.0f;
    [SerializeField] private float pitchVariance = 0.1f;

    private GameObject _soundGameObject;
    private AudioSource _audioSource;

    private void Awake()
    {
        // Create a new game object to play the sound. We need a separate
        // game object so that the sound will continue to play even if the
        // parent game object is destroyed.
        this._soundGameObject = new GameObject("Sound");

        // Load and add an audio source to the game object.
        this._audioSource = this._soundGameObject.AddComponent<AudioSource>();
        this._audioSource.clip = Resources.Load<AudioClip>(this.soundPath);
        this._audioSource.volume = this.volume;

        // Randomise the pitch of the sound to make it sound more natural.
        this._audioSource.pitch = Random.Range(
            this.pitch - this.pitchVariance,
            this.pitch + this.pitchVariance);
    }

    private void Start()
    {
        // Play the sound.
        this._audioSource.Play();

        // Destroy the game object after the sound has finished playing (with
        // an additional delay to account for any lag).
        Destroy(this._soundGameObject, this._audioSource.clip.length + 0.1f);
    }
}
