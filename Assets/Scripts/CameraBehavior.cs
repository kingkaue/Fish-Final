using UnityEngine;
using System.Collections;
using System.Collections.Generic;

public class CameraBehavior : MonoBehaviour
{
    public Vector3 CamOffset;
    public float smoothTime = 0.3f;
    private Transform player;
    private Vector3 velocity = Vector3.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        player = GameObject.FindGameObjectWithTag("Player").transform;
    }

    // Update is called once per frame
    void Update()
    {
        if (player != null)
        {
            Vector3 targetPosition = player.position + CamOffset;

            // Clamp the target position within your world bounds
            targetPosition.x = Mathf.Clamp(targetPosition.x, 23f, 95f);
            targetPosition.z = Mathf.Clamp(targetPosition.z, 15f, 98f);

            transform.position = targetPosition;

            // Keep camera facing straight without rotating
            transform.rotation = Quaternion.Euler(45f, 0f, 0f);
        }
    }
}