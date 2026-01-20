using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MouseLook : MonoBehaviour
{

    Transform playerBody;
    public float mouseSensitivity = 476;

    float xAccumulator;
    float yAccumulator;
    const float Snappiness = 300.0f;

    float pitch = 0;

    // Start is called before the first frame update
    void Start()
    {
        playerBody = transform.parent.transform;

        Cursor.visible = false; //hide cursor
        Cursor.lockState = CursorLockMode.Locked; //lock cursor to center of screen
    }

    // Update is called once per frame
    void Update()
    {
        //        if (!PauseMenuBehavior.isGamePaused)
        //        {
        float moveX = Input.GetAxis("Mouse X") * mouseSensitivity; //* Time.deltaTime;
        xAccumulator = Mathf.Lerp(xAccumulator, moveX, Snappiness * Time.deltaTime);
        float moveY = Input.GetAxis("Mouse Y") * mouseSensitivity; //* Time.deltaTime;
        yAccumulator = Mathf.Lerp(yAccumulator, moveY, Snappiness * Time.deltaTime);


        //yaw
        playerBody.Rotate(Vector3.up * xAccumulator);

        //pitch
        pitch -= yAccumulator;

        pitch = Mathf.Clamp(pitch, -90f, 90f);
        transform.localRotation = Quaternion.Euler(pitch, 0, 0);
        //        }

    }
}

