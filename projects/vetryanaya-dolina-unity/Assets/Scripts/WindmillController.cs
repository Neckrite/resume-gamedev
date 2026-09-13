using UnityEngine;

public class WindmillController : MonoBehaviour
{
    [Header("Components")]
    private Animator animator; 

    [Header("Speed Settings")]
    [SerializeField] private float normalSpeed = 1.5f; 
    [SerializeField] private float slowedSpeed = 0.5f;  
    [SerializeField] private float duration = 3f;        
    [SerializeField] private float transitionSpeed = 2f; 

    private float targetSpeed;
    private float timer = 0f;
    private bool isPlayerInside = false;

    private void Start()
    {
        animator = GetComponent<Animator>();
        if (animator != null)
        {
            animator.speed = normalSpeed;
            targetSpeed = normalSpeed; 
        }
    }

    private void Update()
    {
        if (animator == null) return;

        if (isPlayerInside && Input.GetKeyDown(KeyCode.E) && targetSpeed != slowedSpeed)
        {
            targetSpeed = slowedSpeed;
            timer = duration;          
        }

        if (timer > 0)
        {
            timer -= Time.deltaTime;
            if (timer <= 0)
            {
                targetSpeed = normalSpeed;
            }
        }

        animator.speed = Mathf.MoveTowards(animator.speed, targetSpeed, transitionSpeed * Time.deltaTime);
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerInside = true;
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player")) isPlayerInside = false;
    }
}
