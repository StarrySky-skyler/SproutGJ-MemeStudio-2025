using UnityEngine;

public class PlayerControl : MonoBehaviour
{
    [Header("�ƶ��ٶ�")] public float moveSpeed = 5f;

    [Header("��Ծ�߶�")] public float jumpForce = 10f;

    [Header("�����Ծ����")] public int maxJumpCount = 2;

    [Header("���ϵ��")] public float dashMutiple = 2;

    [Header("���ʱ��")] public float dashTimer = 0.2f;

    [Header("�����")] public LayerMask groundLayer;

    private void Start()
    {
        rb = GetComponent<Rigidbody2D>();
        spriteRenderer = GetComponent<SpriteRenderer>();
        raycastDistance = spriteRenderer.bounds.size.y / 2 + 0.05f;
        speed = moveSpeed;
    }

    private void Update()
    {
        isGrounded = IsGround();

        Move();

        Jump();
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (1 << collision.gameObject.layer == groundLayer && isGrounded)
            jumpCount = 0;
    }

    private bool IsGround()
    {
        return Physics2D.Raycast(spriteRenderer.bounds.center, Vector2.down,
            raycastDistance, groundLayer);
    }

    private void Jump()
    {
        if (Input.GetButtonDown("Jump") && jumpCount < maxJumpCount)
        {
            rb.linearVelocity = new Vector2(rb.linearVelocity.x, jumpForce);
            jumpCount++;
        }
    }

    private void Move()
    {
        float horizontalInput = Input.GetAxis("Horizontal");
        float moveDirection = horizontalInput * speed;
        rb.linearVelocity = new Vector2(moveDirection, rb.linearVelocity.y);
        if (horizontalInput != 0)
            spriteRenderer.flipX = horizontalInput > 0 ? false : true;

        Dash(horizontalInput);
    }

    private void Dash(float hor)
    {
        if (Input.GetKeyDown(KeyCode.LeftShift) && hor != 0)
        {
            speed = moveSpeed * dashMutiple;
            Invoke("DashFinished", dashTimer);
        }
    }

    private void DashFinished()
    {
        speed = moveSpeed;
    }

    #region Debug

    [SerializeField] [ReadOnly] [Header("��ǰ�ٶ�")]
    private float speed;

    [SerializeField] [ReadOnly] [Header("��Ծ����")]
    private int jumpCount;


    [SerializeField] [ReadOnly] [Header("���¼�����")]
    private float raycastDistance;

    [SerializeField] [ReadOnly] [Header("������")]
    private bool isGrounded;

    [SerializeField] [ReadOnly] [Header("2D����")]
    private Rigidbody2D rb;

    [SerializeField] [ReadOnly] [Header("2DSprite")]
    private SpriteRenderer spriteRenderer;

    #endregion
}
