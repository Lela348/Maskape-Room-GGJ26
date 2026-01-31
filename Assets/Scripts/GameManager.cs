using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class GameManager : MonoBehaviour
{
    private float globalAnimationSpeed = 1.0f;

    private Animator[] animators;
    private GameObject[] objects;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animators = FindObjectsOfType<Animator>();
        objects = GameObject.FindGameObjectsWithTag("Interactable Object");
    }

    public void ChangeAnimationSpeed(InputAction.CallbackContext context)
    {
        if (context.started && context.interaction is PressInteraction)
        {
            if (globalAnimationSpeed == 1.0f)
            {
                globalAnimationSpeed = 0.5f;
            }
            else
            {
                globalAnimationSpeed = 1.0f;
            }

            foreach (Animator ani in animators)
            {
                ani.speed = globalAnimationSpeed;
            }
        }
    }

    public void ChangeObjectGravity(InputAction.CallbackContext context)
    {
        if (context.started && context.interaction is PressInteraction)
        {
            foreach (GameObject go in objects)
            {
                InteractableObjects interactable = go.GetComponent<InteractableObjects>();
                interactable.InverseGravity();
            }
        }
    }
}
