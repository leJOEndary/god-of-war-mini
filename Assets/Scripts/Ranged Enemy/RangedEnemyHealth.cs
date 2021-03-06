﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class RangedEnemyHealth : MonoBehaviour
{
    [Tooltip("Colliders that take damage in the archer.")]
    public Collider[] DamageTaker;
    [Tooltip("Whether or not the archer is alive.")]
    public bool isAlive;
    [Tooltip("Current health of the archer.")]
    public int currentHealth;

    private int startingHealth = 40;
    private float timeSinceLastHit = 2f;
    private float dissapearSpeed = 2f;
    private float timer = 0f;
    private Animator anim;
    private NavMeshAgent nav;
    private Rigidbody rigidBody;
    private bool dissapearEnemy = false;
    private AudioSource [] audios;

    public GameObject fallingDmg;
    public GameObject blood;

    // Use this for initialization
    void Start()
    {
        GameManager.instance.RegisterRangedEnemy (this) ;
        rigidBody = GetComponent<Rigidbody>();
        nav = GetComponent<NavMeshAgent>();
        anim = GetComponent<Animator>();
        currentHealth = startingHealth;
        isAlive = true;
        audios = GetComponents<AudioSource>();

    }

    // Update is called once per frame
    void Update()
    {
        if (Input.GetKeyDown(KeyCode.F4))
        {
            currentHealth = 0;
            KillEnemy();
        }
        timer += Time.deltaTime;

        if (dissapearEnemy)
        {
            transform.Translate(-Vector3.up * dissapearSpeed * Time.deltaTime);
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (timer >= timeSinceLastHit && !GameManager.instance.gameOver && isAlive)
        {
            if (other.tag == "PlayerWeapon")
            {
                takeHit(GameManager.instance.damage);
                timer = 0f;
            }
        }
    }

    void takeHit(int amount)
    {
        if(fallingDmg != null) {
            GameObject instance = Instantiate(fallingDmg, transform);
            instance.GetComponent<FallingDmg>().SetText(amount.ToString());
        }
        GameManager.instance.EnemyHit();
        anim.Play("Get Hit");
        audios[0].Play();
        Instantiate(blood,this.gameObject.transform.position + new Vector3(0,1,0),this.gameObject.transform.rotation);

        if (currentHealth > 0)
        {   
            currentHealth -= amount;
        }

        if (currentHealth <= 0)
        {
            isAlive = false;
            audios[1].Play();
            KillEnemy();
        }
    }

    void KillEnemy()
    {
        GameManager.instance.KilledRangedEnemy(this) ;
        nav.enabled = false;
        anim.Play("Die");

        StartCoroutine(removeEnemy());
    }

    IEnumerator removeEnemy()
    {
        yield return new WaitForSeconds(6);
        dissapearEnemy = true;
        yield return new WaitForSeconds(2);
        Destroy(gameObject);
    }
}
