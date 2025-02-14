﻿using UnityEngine;
using System.Collections;

public class JumpAndDuck : MonoBehaviour
{
    public Level level = null;
    public GameObject ground = null;
    public Collider2D standingCollider = null;
    public Collider2D duckingCollider = null;
    public AudioSource jumpAudioSource = null;
    public AudioClip jumpAudioClip = null;
    private Animator animator;
    private bool grounded = true;
    private bool ducking = false;
    private float jumpVelocity = 0f;
    private float gravity = 144f;
    private Vector3 startVector;
    public bool inverted = false;
    private float dinoStartLoc_y;

    void Start()
    {
        animator = GetComponent<Animator>();
        standingCollider.enabled = true;
        duckingCollider.enabled = false;
        dinoStartLoc_y = transform.position.y;
    }

    void Update()
    {
        if (Input.GetButtonDown("Invert") && !HitObstacles.stop)
        {
            inverted = !inverted;
            invert();
        }
        if (grounded)
        {
            if (Input.GetButton("Jump") || Input.GetAxis("Vertical") > 0)
            {
                jump();
            }
            else if (Input.GetAxis("Vertical") < 0)
            {
                duck();
            }
            else
            {
                stand();
            }
        }
        else
        {
            if (inverted)
            {
                handleInverteDino();
            }
            else
            {
                handleUninverteDino();
            }
        }

    }
    private void handleInverteDino()
    {
        transform.position -= jumpVelocity * Vector3.up * Time.deltaTime;
        jumpVelocity -= gravity * Time.deltaTime;

        if (transform.position.y > ground.transform.position.y)
        {
            grounded = true;
            transform.position = startVector;
            animator.SetBool("jumping", false);
        }
        else if (3 < transform.position.y && 20 < jumpVelocity)
        {
            jumpVelocity = 20;
        }
    }


    private void handleUninverteDino()
    {
        transform.position += jumpVelocity * Vector3.up * Time.deltaTime;
        jumpVelocity -= gravity * Time.deltaTime;

        if (transform.position.y < ground.transform.position.y)
        {
            grounded = true;
            transform.position = startVector;
            animator.SetBool("jumping", false);
        }
        else if (3 < transform.position.y && 20 < jumpVelocity)
        {
            jumpVelocity = 20;
        }
    }

    void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject == ground)
        {
            grounded = true;
            transform.position = startVector;
            animator.SetBool("jumping", false);
        }
    }



    void OnCollisionExit2D(Collision2D collision)
    {
        if (collision.gameObject == ground)
        {
            grounded = false;
            animator.SetBool("jumping", true);
        }
    }

    void jump()
    {
        if (!grounded)
        {
            return;
        }
        stand();
        if (jumpAudioSource && jumpAudioClip)
        {
            jumpAudioSource.PlayOneShot(jumpAudioClip, 1);
        }
        startVector = transform.position;
        jumpVelocity = 40f + level.mainSpeed / 10f;
        grounded = false;
        animator.SetBool("jumping", true);
    }

    void duck()
    {
        if (ducking || !grounded)
        {
            return;
        }

        standingCollider.enabled = false;
        duckingCollider.enabled = true;
        ducking = true;
        animator.SetBool("ducking", true);
    }

    void invert()
    {
        jumpAudioSource.PlayOneShot(jumpAudioClip, 1);
        var x = transform.localScale.x;
        var y = transform.localScale.y;
        var z = transform.localScale.z;
        transform.localScale = new Vector3(x, -y, z);

        var loc_x = transform.position.x;
        var loc_y = dinoStartLoc_y - (transform.position.y - dinoStartLoc_y);
        var loc_z = transform.position.z;
        transform.position = new Vector3(loc_x, loc_y, loc_z);
    }

    void stand()
    {
        if (!ducking)
        {
            return;
        }

        standingCollider.enabled = true;
        duckingCollider.enabled = false;
        ducking = false;
        animator.SetBool("ducking", false);
    }
}
