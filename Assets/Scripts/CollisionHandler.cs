using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.SceneManagement;

public class CollisionHandler : MonoBehaviour
{
    [SerializeField] ParticleSystem crashVFX;

    private void Update()
    {
        LoadNextLevelWithKey();
    }


    private void OnTriggerEnter(Collider other)
    {
        switch (other.gameObject.tag)
        {
            case "Success":
                StartSuccessSequence(); //Successful landing
                break;

            default:
                StartCrashSequence(); //Default collision
                break;
        }
    }

    void StartCrashSequence()
    {
        crashVFX.Play();
        GetComponent<Movement>().enabled = false;
        Invoke("ReloadLevel", 1f);
    }

    void ReloadLevel()
    {
        int currentSceneIndex =
            SceneManager.GetActiveScene()
                .buildIndex;
        SceneManager.LoadScene(
            currentSceneIndex);
    }

    void LoadNextLevel()
    {
        int nextSceneIndex = SceneManager.GetActiveScene().buildIndex + 1;
        if (nextSceneIndex == SceneManager.sceneCountInBuildSettings)
        {
            nextSceneIndex = 0;
        }

        SceneManager.LoadScene(
            nextSceneIndex);
    }

    private void LoadNextLevelWithKey()
    {
        if (Input.GetKeyDown(KeyCode.L))
        {
            LoadNextLevel();
        }
    }

    void StartSuccessSequence()
    {
        GetComponent<Movement>().enabled = false;
        Invoke("LoadNextLevel", 1);
    }
}