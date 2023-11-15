using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Serialization;


public class Movement : MonoBehaviour
{
    [Header("General Setup Settings")] [SerializeField] [Tooltip("How fast ship moves up and down upon player input")]
    private float controlSpeed = 20f;

    [Header("Range Restriction for X and Y dimensions")] [SerializeField]
    private float xRange = 13f;

    [SerializeField] private float yRange = 13f;


    [SerializeField] private GameObject[] lasers;
    [SerializeField] private GameObject[] extraLasers;
    private bool extraLaserActive = false;


    [Header("Rotation Elements")]
    [Tooltip("How big is your rotation On X dimension based on position")]
    [SerializeField]
    private float positionPitchFactor = -2f;

    [Tooltip("How big is your rotation On X dimension based on control")] [SerializeField]
    private float controlPitchFactor = -15f;

    [Tooltip("How big is your rotation On Y dimension based on position")] [SerializeField]
    private float positionYawFactor = 2f;

    [Tooltip("How big is your rotation On Z dimension based on control")] [SerializeField]
    private float controlRollFactor = -20f;

    private float xThrow;
    private float yThrow;


    // Update is called once per frame
    void Update()
    {
        ProcessTranslation();
        ProcessRotation();
        ProcessFiring();
        ProcessExtraFiring();
    }


    void ProcessRotation()
    {
        float pitchDueToPosition = transform.localPosition.y * positionPitchFactor;
        float pitchDueToControl = yThrow * controlPitchFactor;

        float yawDueToPosition = transform.localPosition.x * positionYawFactor;


        float rollDueToControl = xThrow * controlRollFactor;


        float pitch = pitchDueToControl + pitchDueToPosition;
        float yaw = yawDueToPosition;
        float roll = rollDueToControl;


        transform.localRotation = Quaternion.Euler(-pitch, yaw, roll);
    }

    private void ProcessTranslation()
    {
        xThrow = Input.GetAxis("Horizontal");
        yThrow = Input.GetAxis("Vertical");


        float xOffset = xThrow * Time.deltaTime * controlSpeed;
        float rawXPos = transform.localPosition.x + xOffset;
        float clampedXPos = Mathf.Clamp(rawXPos, -xRange, xRange); // to restrict movement


        float yOffset = yThrow * Time.deltaTime * controlSpeed;
        float rawYPos = transform.localPosition.y + yOffset;
        float clampedYPos = Mathf.Clamp(rawYPos, -yRange + 10, yRange);

        transform.localPosition =
            new Vector3(clampedXPos, clampedYPos, transform.localPosition.z);
    }

    void ProcessFiring()
    {
        if (Input.GetKey(KeyCode.Space))
        {
            ExtraLasersActive(extraLaserActive);
            SetLasersActive(true);
        }
        else
        {
            ExtraLasersActive(false);
            SetLasersActive(false);
        }
    }

    void SetLasersActive(bool isActive)
    {
        foreach (GameObject laser in lasers)
        {
            var emissionModule = laser.GetComponent<ParticleSystem>().emission;

            emissionModule.enabled = isActive;
        }
    }

    void ProcessExtraFiring()
    {
        if (Input.GetKeyDown(KeyCode.H))
        {
            extraLaserActive = !extraLaserActive; // toggle
        }
    }


    void ExtraLasersActive(bool isActive)
    {
        foreach (GameObject extraLaser in extraLasers)
        {
            var emissionModule = extraLaser.GetComponent<ParticleSystem>().emission;

            emissionModule.enabled = isActive;
        }
    }
}