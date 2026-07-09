using System;
using UnityEngine;

public class LookComponent : MonoBehaviour
{
    [SerializeField] private float lookSpeed = 120f;
    [SerializeField] private Transform cameraPivot;

    private Vector2 lookInput;
    private float pitch;

    public void SetLookInput(Vector2 input)
    {
        lookInput = input;
    }

    private void Update()
    {
        float yaw = lookInput.x * lookSpeed * Time.deltaTime;
        transform.Rotate(Vector3.up, yaw);

        pitch -= lookInput.y * lookSpeed * Time.deltaTime;
        pitch = Mathf.Clamp(pitch, -80f, 80f);

        if (cameraPivot != null)
        {
            cameraPivot.localRotation = Quaternion.Euler(pitch, 0f, 0f);
        }
    }
}
