using UnityEngine;

public class HammerMovement : MonoBehaviour
{
    [SerializeField] float delayInSeconds = 2f; // Adjust this value as needed
    private Animator hammerAnimator;

    void Start()
    {
        // Get the Animator component attached to the same GameObject
        hammerAnimator = GetComponent<Animator>();

        // Call a method to start the delay before triggering the animation
        StartAnimationDelay();
    }

    void StartAnimationDelay()
    {
        // Invoke the method to trigger the animation after the specified delay
        Invoke("TriggerHitAnimation", delayInSeconds);
    }

    void TriggerHitAnimation()
    {
        // Set the "Hit" trigger for the animation
        if (hammerAnimator != null)
        {
            hammerAnimator.SetTrigger("Hit");
        }
    }
}