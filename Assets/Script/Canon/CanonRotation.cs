using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

public class CanonRotation : MonoBehaviour
{
    public static CanonRotation Instance;
    public float rotationSpeed = 50f;
    public bool facingRight = true;
    public float currentRotationAngle;

    private Transform imageRectTransform;
    private PlayerMoving playerMoving;
    public float initialRotationAngle;
    public float difference;
    private void Awake()
    {
       Instance = this; 
    }
    void Start()
    {
        imageRectTransform = GetComponent<Transform>();
        initialRotationAngle = imageRectTransform.eulerAngles.z;
    }

    void Update()
    {
        playerMoving = transform.parent.GetComponent<PlayerMoving>();
        if (playerMoving != null)
        {
            facingRight = playerMoving.facingRight;
        }

        if (Input.GetKey(KeyCode.UpArrow))
        {
            RotateUp();
        }
        if (Input.GetKey(KeyCode.DownArrow))
        {
            RotateDown();
        }

        currentRotationAngle = imageRectTransform.eulerAngles.z;
    }

    void RotateUp()
    {
        if (facingRight && AngleDifference(initialRotationAngle, currentRotationAngle) < 90)
        {
            imageRectTransform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
        else if (!facingRight && AngleDifference(initialRotationAngle, currentRotationAngle) > -90)
        {
            imageRectTransform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);
        }
    }

    void RotateDown()
    {
        if (facingRight && AngleDifference(initialRotationAngle, currentRotationAngle) > -90)
        {
            imageRectTransform.Rotate(Vector3.forward, -rotationSpeed * Time.deltaTime);
        }
        else if (!facingRight && AngleDifference(initialRotationAngle, currentRotationAngle) < 90)
        {
            imageRectTransform.Rotate(Vector3.forward, rotationSpeed * Time.deltaTime);
        }
    }

   public  float AngleDifference(float initialAngle, float currentAngle)
    {
        difference = currentAngle - initialAngle;
        if (difference > 180) difference -= 360;

        return difference;
    }
    public float rotation()
    {
        return currentRotationAngle;
    }

}
