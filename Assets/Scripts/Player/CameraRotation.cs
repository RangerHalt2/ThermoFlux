//Purpose: This script rotates the gun and camera up and down
//Author: Logan Baysinger.

using UnityEngine;

public class CameraRotation : MonoBehaviour
{
    [SerializeField] private Transform player;
    [SerializeField] private Vector3 offset = new Vector3(0f, 2f, -4f);
    [SerializeField] private float sensitivity = 150f;
    [SerializeField] private float minPitch = -30f;
    [SerializeField] private float maxPitch = 50f;

    private float yaw;
    private float pitch;
    private InputManager inputs;

    private void Start()
    {
        inputs = GameObject.FindAnyObjectByType<InputManager>();

        Vector3 angles = transform.eulerAngles;
        yaw = angles.y;
        pitch = angles.x;
        if (pitch > 180f) pitch -= 360f;
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);
    }

    private void LateUpdate()
    {
        RotateCamera(inputs.LookInput);
    }

    private void RotateCamera(Vector2 lookInput)
    {
        yaw += lookInput.x * sensitivity * Time.deltaTime;
        pitch -= lookInput.y * sensitivity * Time.deltaTime;
        //Debug.Log("yaw : " +  yaw);
        //Debug.Log("pitch :" + pitch);
        pitch = Mathf.Clamp(pitch, minPitch, maxPitch);

        // Compute orbit rotation
        Quaternion rotation = Quaternion.Euler(pitch, yaw, 0f);

        // Compute camera position around player
        Vector3 desiredPosition = player.position + rotation * offset;
        transform.position = desiredPosition;

        // --- STABLE LookAt ---
        // Compute look target (e.g. player’s head)
        Vector3 lookTarget = player.position + Vector3.up * 1.5f;

        // Use player's up direction instead of world up to stabilize
        Vector3 upVector = player.up; // or Vector3.up if player doesn’t tilt

        transform.LookAt(lookTarget, upVector);
    }

}
