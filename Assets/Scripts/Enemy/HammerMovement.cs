using UnityEngine;

public class HammerMovement : MonoBehaviour
{
    [SerializeField] float delayInSeconds = 2f;
    private Animator hammerAnimator;

    void Start()
    {
        hammerAnimator = GetComponent<Animator>();

        StartAnimationDelay();
    }

    void StartAnimationDelay()
    {
        Invoke("TriggerHitAnimation", delayInSeconds);
    }

    void TriggerHitAnimation()
    {
        if (hammerAnimator != null)
        {
            hammerAnimator.SetTrigger("Hit");
        }
    }
}