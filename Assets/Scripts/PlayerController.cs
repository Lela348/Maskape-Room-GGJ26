using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

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

    private Ray ray;
    private RaycastHit hitData;
    private LayerMask layerMask;
    private Vector3 offset;
    private Vector3 playerVelocity;

    private const float GRAVITY = -9.81f;
    private const float DISTANCE = 2.5f;
    private const float GRAB_POS_X = 0.5f;
    private const float GRAB_POS_Y = 0.25f;
    private const float SCALE_SMALL = 0.5f;
    private const float SCALE_POSITION_OFFSET = 0.5f;

    private GameObject holdObject;

    private bool isSmall = false;
    private bool groundedPlayer;

    private void Awake()
    {
        layerMask = LayerMask.GetMask("Interactable Object");
    }

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        characterController = GetComponent<CharacterController>();
    }

    // Update is called once per frame
    void Update()
    {
        groundedPlayer = characterController.isGrounded;
        if (groundedPlayer)
        {
            playerVelocity.y = 0f;
        }

        Vector3 movement = transform.right * moveVec2.x + transform.forward * moveVec2.y;
        movement = Vector3.ClampMagnitude(movement, 1f);
        playerVelocity.y += GRAVITY * Time.deltaTime;

        Vector3 finalMove = (movement * speed) + (playerVelocity.y * Vector3.up);
        characterController.Move(finalMove * Time.deltaTime);

        Vector2 mouseVelocity = mouseInputVec2 * sensitivity;

        rotation += mouseVelocity * Time.deltaTime;
        rotation.y = Mathf.Clamp(rotation.y, -maxVerticalAngle, maxVerticalAngle);

        transform.rotation = Quaternion.Euler(0f, rotation.x, 0f);
        cam.transform.rotation = Quaternion.Euler(-rotation.y, rotation.x, 0f);

        if (holdObject != null)
        {
            holdObject.transform.position = cam.ViewportToWorldPoint(new Vector3(GRAB_POS_X, GRAB_POS_Y, DISTANCE));
            holdObject.transform.rotation = Quaternion.Euler(-rotation.y, rotation.x, 0f);
            Debug.Log("HoldObject pos: " +  holdObject.transform.position);
        }
    }

    private void FixedUpdate()
    {
        ray = cam.ViewportPointToRay(new Vector3 (0.5f, 0.5f, 0f));
        Debug.DrawRay(ray.origin, ray.direction * 2, Color.white);
    }

    public void OnMove(InputAction.CallbackContext context)
    {
        moveVec2 = context.ReadValue<Vector2>();
    }

    public void OnLook(InputAction.CallbackContext context)
    {
        mouseInputVec2 = context.ReadValue<Vector2>();
    }

    public void OnGrab(InputAction.CallbackContext context)
    {
        if(context.started && context.interaction is PressInteraction)
        {
            Debug.Log("Grab function called");
            if (holdObject == null)
            {
                if (Physics.Raycast(ray.origin, ray.direction, out hitData, DISTANCE, layerMask))
                {
                    if (hitData.transform.gameObject)
                    {
                        Debug.Log("GO hit");
                        holdObject = hitData.transform.gameObject;
                        
                    }
                    Debug.Log("Hold object should not be null: " + holdObject);
                }

            }
            else
            {
                holdObject.transform.position = cam.ViewportToWorldPoint(new Vector3(GRAB_POS_X, GRAB_POS_Y, DISTANCE));
                holdObject = null;
                Debug.Log("Hold object should be null: " + holdObject);
            }
        }
    }

    public void OnDebugScale(InputAction.CallbackContext context)
    {
        if (context.started && context.interaction is PressInteraction)
        {
            Debug.Log("DEBUG SCALE function called");
            if(isSmall)
            {
                transform.localScale = Vector3.one;
                characterController.Move(new Vector3(0.0f, SCALE_POSITION_OFFSET, 0.0f));
                isSmall = false;
            }
            else
            {
                transform.localScale = new Vector3(SCALE_SMALL, SCALE_SMALL, SCALE_SMALL);
                characterController.Move(new Vector3(0.0f, -SCALE_POSITION_OFFSET, 0.0f));
                isSmall = true;
            }
            Debug.Log("Transform pos after scale change: " + transform.position);
        }

    }
}
