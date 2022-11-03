using UnityEngine;

public class SoundManager : MonoBehaviour
{
    AudioSource audioSource;
    public AudioClip shotFired;
    public AudioClip enemySpawn;
    public AudioClip enemyHit;
    public AudioClip stationDamaged;
    public AudioClip gameOver;
    public AudioClip ghoulSpawn;
    public AudioClip spectreSpawn;
    public AudioClip spectreMove;

    // Start is called before the first frame update
    void Start()
    {
        audioSource = GetComponent<AudioSource>();
    }

    public void playShotFired()
    {
        audioSource.PlayOneShot(shotFired);
    }

    public void playEnemySpawn()
    {
        audioSource.PlayOneShot(enemySpawn);
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

        public void playGhoulSpawn()
    {
        audioSource.PlayOneShot(ghoulSpawn);
    }

        public void playSpectreSpawn()
    {
        audioSource.PlayOneShot(spectreSpawn);
    }

        public void playSpectreMove()
    {
        audioSource.PlayOneShot(spectreMove);
    }
}
