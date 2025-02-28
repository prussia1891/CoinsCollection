using UnityEngine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private Transform launchIndicator;
    [SerializeField] private Camera playerCamera;

    private Rigidbody rb;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool canDash = true;

    private void Start()
    {
        inputManager.OnMove.AddListener(MovePlayer);
        rb = GetComponent<Rigidbody>();
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
        {
            JumpPlayer();
        }

        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            DashPlayer();
        }

        UpdateLaunchIndicator();
    }

    private void MovePlayer(Vector2 direction)
    {
        Vector3 moveDirection = new Vector3(direction.x, 0f, direction.y);
        rb.AddForce(speed * moveDirection);
    }

    private void JumpPlayer()
    {
        if (isGrounded)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            isGrounded = false;
            canDoubleJump = true;
        }
        else if (canDoubleJump)
        {
            rb.AddForce(Vector3.up * jumpForce, ForceMode.Impulse);
            canDoubleJump = false;
        }
    }

    private void DashPlayer()
    {
        Vector3 dashDirection = rb.linearVelocity.normalized;
        rb.AddForce(dashDirection * dashForce, ForceMode.Impulse);
        canDash = false;
        Invoke(nameof(ResetDash), dashCooldown);
    }

    private void ResetDash()
    {
        canDash = true;
    }

    private void UpdateLaunchIndicator()
    {
        if (launchIndicator != null && playerCamera != null)
        {
            Ray cameraRay = playerCamera.ScreenPointToRay(Input.mousePosition);
            if (Physics.Raycast(cameraRay, out RaycastHit hit))
            {
                launchIndicator.position = hit.point;
                Vector3 aimDirection = (hit.point - transform.position).normalized;
                transform.forward = new Vector3(aimDirection.x, 0, aimDirection.z);
            }
        }
    }

    private void OnCollisionStay(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = true;
        }
    }

    private void OnCollisionExit(Collision collision)
    {
        if (collision.gameObject.CompareTag("Ground"))
        {
            isGrounded = false;
        }
    }
}