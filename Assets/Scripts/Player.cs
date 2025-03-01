using UnityEngine;
using Unity.Cinemachine;

public class Player : MonoBehaviour
{
    [SerializeField] private InputManager inputManager;
    [SerializeField] private float speed;
    [SerializeField] private float jumpForce = 5f;
    [SerializeField] private float dashForce = 10f;
    [SerializeField] private float dashCooldown = 1f;
    [SerializeField] private ArrowIndicator arrowIndicator;

    private Rigidbody rb;
    private bool isGrounded;
    private bool canDoubleJump;
    private bool canDash = true;

    private void Start()
    {
        inputManager.OnMove.AddListener(MovePlayer);
        inputManager.OnSpacePressed.AddListener(JumpPlayer);
        rb = GetComponent<Rigidbody>();
        rb.constraints = RigidbodyConstraints.FreezeRotationX | RigidbodyConstraints.FreezeRotationZ;
    }

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && canDash)
        {
            DashPlayer();
        }
    }

    private void MovePlayer(Vector2 direction)
    {
        // 获取瞄准器的前向和右向向量
        Vector3 arrowForward = arrowIndicator.transform.forward;
        Vector3 arrowRight = arrowIndicator.transform.right;

        // 将向量平面化到XZ平面
        arrowForward.y = 0;
        arrowRight.y = 0;
        arrowForward.Normalize();
        arrowRight.Normalize();

        // 根据瞄准器方向计算移动方向
        Vector3 moveDirection = arrowForward * direction.y + arrowRight * direction.x;
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