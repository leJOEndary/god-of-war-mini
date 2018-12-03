﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerHealth : MonoBehaviour
{
    public int startingHealth = 100;
    public float timeSinceLastHit = 2f;
    private float timer = 0f;
    private CharacterController characterController;
    private Animator anim;
    private int currentHealth;

    // Use this for initialization
    void Start()
    {
        anim = GetComponent<Animator>();
        characterController = GetComponent<CharacterController>();
        currentHealth = startingHealth;
    }

    // Update is called once per frame
    void Update()
    {
        timer += Time.deltaTime;
    }
    private void OnTriggerEnter(Collider other)
    {
        //to be modified after taggs are set for all bosses
        if (timer >= timeSinceLastHit)
            if (other.tag == "Weapon")
            {
                takeHit();
                timer = 0;
            }

    }

    void takeHit()
    {
        if (currentHealth > 0)
        {
            //GameManager.instance.PlayerHit(currentHealth);
            anim.Play("Hurt");
            currentHealth -= 10;

        }
        if (currentHealth <= 0)
        {
            killPlayer();

        }

    }
    void killPlayer()
    {
        //GameManager.instance.PlayerHit(currentHealth);
        anim.SetTrigger("herodie");
        characterController.enabled = false;
    }
}