using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

[RequireComponent(typeof(Rigidbody))]
public class PlayerMovement : MonoBehaviour
{
    [Header("Input")]
    public InputAction MoveAction;

    [Header("Settings")]
    public float walkSpeed = 2.0f;
    public float turnSpeed = 10f;

    private Rigidbody m_Rigidbody;
    private Vector3 m_Movement;
    private Quaternion m_Rotation = Quaternion.identity;

    void Awake()
    {
        m_Rigidbody = GetComponent<Rigidbody>();
        m_Rigidbody.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
        // Prevents falling over sideways
    }

    void OnEnable()
    {
        MoveAction.Enable();
    }

    void OnDisable()
    {
        MoveAction.Disable();
    }

    void FixedUpdate()
    {
        // Read 2D input
        Vector2 pos = MoveAction.ReadValue<Vector2>();
        float horizontal = pos.x;
        float vertical = pos.y;

        // Build movement vector
        m_Movement = new Vector3(horizontal, 0f, vertical);

        if (m_Movement.sqrMagnitude > 0.001f)
        {
            // Smooth rotation towards movement direction
            Vector3 desiredForward = Vector3.RotateTowards(transform.forward, m_Movement, turnSpeed * Time.fixedDeltaTime, 0f);
            m_Rotation = Quaternion.LookRotation(desiredForward);

            // Apply movement
            Vector3 move = m_Movement.normalized * walkSpeed * Time.fixedDeltaTime;
            m_Rigidbody.MovePosition(m_Rigidbody.position + move);
            m_Rigidbody.MoveRotation(m_Rotation);
        }
        else
        {
            // Stay idle → stop movement
            m_Rigidbody.velocity = Vector3.zero;
        }
    }
}
