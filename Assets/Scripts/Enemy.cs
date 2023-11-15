using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;

public class Enemy : MonoBehaviour
{
    [SerializeField] private GameObject deathFX;
    [SerializeField] private GameObject hitVFX;


    private GameObject parentGameObject;


    [SerializeField] private int scorePerHit = 15;
    [SerializeField] private int enemyHealth = 100;
    [SerializeField] private int damagePerHit = 20;
    private int pointPerKill = 500;

    ScoreBoard scoreBoard;


    void Start()
    {
        AddRigidBody();
        scoreBoard = FindObjectOfType<ScoreBoard>();
        parentGameObject = GameObject.FindWithTag("Spawn at Runtime");
    }

    private void AddRigidBody()
    {
        Rigidbody rb = gameObject.AddComponent<Rigidbody>();
        rb.useGravity = false;
    }

    void OnParticleCollision(GameObject other)
    {
        ProcessHit();

        if (enemyHealth <= Mathf.Epsilon)
        {
            KillEnemy();
        }
    }

    private void ProcessHit()
    {
        GameObject vfx = Instantiate(hitVFX, transform.position, Quaternion.identity);
        vfx.transform.parent = parentGameObject.transform;
        enemyHealth -= damagePerHit;
        scoreBoard.IncreaseScore(scorePerHit);
    }

    private void KillEnemy()
    {
        GameObject vfx = Instantiate(deathFX, transform.position, Quaternion.identity);
        vfx.transform.parent = parentGameObject.transform;

        Destroy(gameObject);

        scoreBoard.IncreaseScore(pointPerKill);
    }
}