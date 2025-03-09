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

            transform.position = targetPosition;

            transform.LookAt(player);
        }
    }
}
