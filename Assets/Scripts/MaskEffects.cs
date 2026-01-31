using UnityEngine;

public class MaskEffects : MonoBehaviour
{
    private GameManager gameManager;
    private PlayerController playerController;

    public void Start()
    {
        gameManager = GameObject.FindGameObjectWithTag("GameManager").GetComponent<GameManager>();
        playerController = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerController>();
    }

    public void ActivateMask(Masks mask)
    {
        switch (mask)
        {
            case Masks.OWL:
                OwlEffect(true);
                break;
            case Masks.GRAVITY:
                GravityEffect(true);
                break;
            case Masks.MOUSE:
                MouseEffect(true);
                break;
            case Masks.SLOWMO:
                SlowMoEffect(true);
                break;
        }
    }

    public void DeactivateMask(Masks mask)
    {
        switch (mask)
        {
            case Masks.OWL:
                OwlEffect(false);
                break;
            case Masks.GRAVITY:
                GravityEffect(false);
                break;
            case Masks.MOUSE:
                MouseEffect(false);
                break;
            case Masks.SLOWMO:
                SlowMoEffect(false);
                break;
        }
    }

    private void OwlEffect(bool enableIt)
    {

    }

    private void MouseEffect(bool enableIt)
    {
        Debug.Log("Mouse effect active: " +enableIt);
        playerController.ChangeScale(enableIt);
    }

    private void SlowMoEffect(bool enableIt)
    {
        Debug.Log("SlowMo effect active: " + enableIt);
        float animationSpeed;
        if(enableIt)
        {
            animationSpeed = 0.5f;
        }
        else
        {
            animationSpeed = 1.0f;
        }
        gameManager.ChangeAnimationSpeed(animationSpeed);
    }

    private void GravityEffect(bool enableIt)
    {
        Debug.Log("Gravity effect active: " + enableIt);
        float rotationValue;
        if(enableIt)
        {
            rotationValue = 180f;
        }
        else
        {
            rotationValue = 0f;
        }
        playerController.ChangePlayerGravity(rotationValue);
        gameManager.ChangeObjectGravity();
    }
}
