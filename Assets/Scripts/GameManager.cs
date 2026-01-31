using System;
using System.Runtime.CompilerServices;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Interactions;

public class GameManager : MonoBehaviour
{
    private MaskEffects maskEffects;

    private float globalAnimationSpeed = 1.0f;

    private Animator[] animators;
    private GameObject[] objects;

    private Masks activeMask = Masks.NONE;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        animators = FindObjectsOfType<Animator>();
        objects = GameObject.FindGameObjectsWithTag("Interactable Object");
        maskEffects = GameObject.FindGameObjectWithTag("MaskEffects").GetComponent<MaskEffects>();
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

    public void ChangeObjectGravity()
    {
        foreach (GameObject go in objects)
        {
            InteractableObjects interactable = go.GetComponent<InteractableObjects>();
            interactable.InverseGravity();
        }
    }

    public void ChangeAnimationSpeed(float aniSpeed)
    {
        foreach (Animator ani in animators)
        {
            ani.speed = aniSpeed;
        }
    }

    public void ChangeMask(InputAction.CallbackContext context)
    {
        if (context.started && context.interaction is PressInteraction)
        {
            Debug.Log("Change Mask key pressed: " + context.control.name);
            int maskId = int.Parse(context.control.name) - 1;
            Masks[] maskArray = (Masks[])Enum.GetValues(typeof(Masks));
            Masks newMask = maskArray[maskId];

            if(newMask != activeMask && newMask != Masks.NONE)
            {
                maskEffects.DeactivateMask(activeMask);
                maskEffects.ActivateMask(newMask);
                activeMask = newMask;
            }
            else
            {
                maskEffects.DeactivateMask(activeMask);
                activeMask = Masks.NONE;
            }
        }
    }
}
