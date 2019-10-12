using UnityEngine;
using UnityEngine.SceneManagement;
using System.Collections;
using System;

public class HitObstacles : MonoBehaviour
{
    public AudioSource audioSource = null;
    public AudioClip audioClip = null;
    public GameObject restart = null;
    private Animator animator;
    public static bool stop = false;
    private Vector3 restartStartPosition = Vector3.zero;
    private int livesCount;

    void Start()
    {
        livesCount = 2;
        animator = GetComponent<Animator>();
        restartStartPosition = restart.transform.position;
        restart.transform.position = Vector3.up * 50;
    }

    void Update()
    {
        if (!stop || !Input.anyKeyDown)
        {
            return;
        }
        SceneManager.LoadScene("Game");
        Time.timeScale = 1;
        SceneManager.sceneLoaded += OnGameRestarted;
    }

    void OnGameRestarted(Scene scene, LoadSceneMode mode)
    {
        stop = false;
    }

    void OnTriggerEnter2D(Collider2D collider)
    {
        if (collider.gameObject.tag == "Obstacle")
        {
            handleCollisionWithObsacle(collider);
        }
        if (collider.gameObject.tag == "Collectable")
        {
            Level.coins += 1;
            Destroy(collider.gameObject);
        }
    }

    private void handleCollisionWithObsacle(Collider2D collider)
    {
        removeLife();
        Debug.Log(livesCount);
        if (livesCount == 0)
        {
            if (audioSource && audioClip)
            {
                audioSource.PlayOneShot(audioClip, 1);
            }
            restart.transform.position = restartStartPosition;
            Time.timeScale = 0;
            stop = true;
            animator.SetTrigger("hit");
            Animator otherAnimator = collider.gameObject.GetComponent<Animator>();
            if (otherAnimator == null)
            {
                return;
            }
            otherAnimator.SetTrigger("hit");
        }
    }

    void removeLife()
    {
        livesCount--;
        changeHeartsSprite(livesCount);
    }
    void changeHeartsSprite(int numOfLives)
    {
        Sprite[] sprites = Resources.LoadAll<Sprite> ("heart sheet");
        Debug.Log("sprlgth " + sprites.Length);
        switch (numOfLives)
        {
            case 2:
                GameObject.Find("Lives").GetComponent<SpriteRenderer>().sprite = (Sprite) sprites[2];
                break;
            case 1:
                GameObject.Find("Lives").GetComponent<SpriteRenderer>().sprite = (Sprite) sprites[1];
                break;
            case 0:
                GameObject.Find("Lives").GetComponent<SpriteRenderer>().sprite = (Sprite) sprites[0];
                break;
        }
    }
}
