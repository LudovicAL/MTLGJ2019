using UnityEngine;

[RequireComponent(typeof(Animator))]
public class AnimationManager : MonoBehaviour
{
    private Animator animator;
    private PlayerAnim playerAnim;
    [SerializeField] private Rigidbody2D rb;
    private void Start()
    {
        animator = GetComponent<Animator>();
        playerAnim = gameObject.transform.parent.GetComponent<PlayerAnim>();
    }

    private void LateUpdate()
    {
        if (animator.GetCurrentAnimatorStateInfo(0).IsName("Idle"))
        {
            ResetAnimatorState();
        }

        if (rb.velocity.y > 0.2f)
        {
            animator.SetBool("isJumping", true);
        }
        else if (rb.velocity.y < -0.2f)
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", true);
        }
        else 
        {
            animator.SetBool("isJumping", false);
            animator.SetBool("isFalling", false);
        }

        if (playerAnim.Crouching)
        {
            animator.SetBool("isCrouching", true);
        }

        if (playerAnim.Walking)
        {
            animator.SetBool("isWalking", true);
        }

        if (!playerAnim.Crouching)
        {
            animator.SetBool("isCrouching", false);
        }
    }

    private void ResetAnimatorState()
    {
        foreach (AnimatorControllerParameter parameter in animator.parameters)
        {
            animator.SetBool(parameter.name, false);
        }
    }
}
