using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    public float speed = 0.1f;

    void Update()
    {
        if (Input.GetKey(KeyCode.RightArrow))
        {
            transform.position = transform.position + Vector3.right * speed;
        }
        else if (Input.GetKey(KeyCode.LeftArrow))
        {
            transform.position = transform.position + Vector3.left * speed;
        }
        else if (Input.GetKey(KeyCode.UpArrow))
        {
            transform.position = transform.position + Vector3.forward * speed;
        }
        else if (Input.GetKey(KeyCode.DownArrow))
        {
            transform.position = transform.position + Vector3.back * speed;
        }

    }
}
