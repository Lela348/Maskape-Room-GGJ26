using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour
{

    [SerializeField] private float speed = 1.0f;
    [SerializeField] private Vector2 sensitivity;
    [SerializeField] private float maxVerticalAngle;
    [SerializeField] private Camera cam;

    private CharacterController characterController;
    private Vector2 moveVec2 = Vector2.zero;
    private Vector2 rotation = Vector2.zero;
    private Vector2 mouseInputVec2 = Vector2.zero;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 movement = transform.right * moveVec2.x + transform.forward * moveVec2.y;

        characterController.Move(movement * speed * Time.deltaTime);

        Vector2 velocity = mouseInputVec2 * sensitivity;


        rotation += velocity * Time.deltaTime;
        rotation.y = Mathf.Clamp(rotation.y, -maxVerticalAngle, maxVerticalAngle);

        transform.rotation = Quaternion.Euler(0f, rotation.x, 0f);
        cam.transform.rotation = Quaternion.Euler(-rotation.y, rotation.x, 0f);
    }

    private void FixedUpdate()
    {
        Debug.DrawRay(transform.position, cam.transform.TransformDirection(Vector3.forward) * 2, Color.white);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveVec2 = context.ReadValue<Vector2>();
        Debug.Log($"Move Vec3: {moveVec2}");
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseInputVec2 = context.ReadValue<Vector2>();
        Debug.Log($"Look Vec3: {mouseInputVec2}");
    }
}
