using UnityEngine;
using System.Collections;
using UnityEngine.UI;

public class Scores : MonoBehaviour
{
    public Level level = null;
    public AudioSource audioSource = null;
    public AudioClip audioClip = null;
    public Number score = null;
    public Number highScore = null;
    public Number flashScore = null;
    public int flashIterations = 10;
    public float flashDuration = 1;
    private int lastHundred = 0;
    private Vector3 scoreStartPosition = Vector3.zero;
    private int iterated = 0;
    private static float highestScore = 0;

    void Start()
    {
        flashScore.transform.position = score.transform.position + Vector3.up * 50;
    }

    void Update()
    {
        int points = (int)level.getDistance() + Level.coins * 100;
        if (audioSource && audioClip && (int)(points / 100) != lastHundred)
        {
            lastHundred = (int)(points / 100);
            flashScore.Value = lastHundred * 100;
            audioSource.PlayOneShot(audioClip, 1);
        }
        score.Value = (int)points;
        highScore.Value = (int)highestScore;
    }

    void Awake()
    {
        highScore.Value = (int)highestScore;
    }

    void OnDestroy()
    {
        int points = (int)level.getDistance() + Level.coins * 100;
        highestScore = Mathf.Max(points, highestScore);
    }
}
