using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip shotFired;
    public AudioClip enemyHit;
    public AudioClip stationDamaged;
    public AudioClip gameOver;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void playShotFired()
    {
        audioSource.PlayOneShot(shotFired);
    }

    public void playEnemyHit()
    {
        audioSource.PlayOneShot(enemyHit);
    }

    public void playStationDamaged()
    {
        audioSource.PlayOneShot(stationDamaged);
    }

    public void playGameOver()
    {
        audioSource.PlayOneShot(gameOver);
    }
}
