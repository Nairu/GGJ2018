using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(Controller2D))]
public class PushableObject : MonoBehaviour
{

    public float gravity = 12;
    Controller2D controller;
    Vector3 velocity;

    void Start()
    {
        controller = GetComponent<Controller2D>();
    }

    void Update()
    {
        velocity += Vector3.down * gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime, false);
        if (controller.collisions.below)
        {
            velocity = Vector3.zero;
        }

    }

    public void Push(Vector2 amount)
    {
        controller.Move(amount, false);
    }
}