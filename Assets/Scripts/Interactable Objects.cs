using UnityEngine;

public class InteractableObjects : MonoBehaviour
{
    private LayerMask layerMaskGround;
    private Vector3 gravity = new Vector3(0f, 9.81f, 0f);
    private float rotationZ = 0f;
    private Rigidbody rb;

    private void Start()
    {
        rb = GetComponent<Rigidbody>();
        layerMaskGround = LayerMask.GetMask("Ground");
    }

    private void FixedUpdate()
    {
        if (!rb.useGravity)
        {
            rb.AddForce(gravity * (rb.mass * rb.mass));
        }
    }

    public void InverseGravity()
    {
        rb.useGravity = !rb.useGravity;
        if (rotationZ == 0f)
        {
            rotationZ = 180f;
        }
        else
        {
            rotationZ = 0f;
        }

        transform.rotation = Quaternion.Euler(0f, 0f, rotationZ);
    }
}
