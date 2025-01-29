
using UnityEngine;
using Vector2Json.SaveSystem;
namespace AnRan
{
    public class PlayerControl : MonoBehaviour
    {
        [Header("移动速度")]
        public float moveSpeed = 5f;
        [Header("跳跃高度")]
        public float jumpForce = 10f;
        [Header("最大跳跃次数")]
        public int maxJumpCount = 2;

        [Header("冲刺系数")]
        public float dashMutiple = 2;

        [Header("冲刺时间")]
        public float  dashTimer = 0.2f;
        [Header("地面层")]
        public LayerMask groundLayer;

        #region Debug

        [SerializeField]
        [ReadOnly]
        [Header("当前速度")]
        float speed = 0;

        [SerializeField]
        [ReadOnly]
        [Header("跳跃次数")]
        int jumpCount = 0;


        [SerializeField]
        [ReadOnly]
        [Header("向下检测距离")]
        float raycastDistance;

        [SerializeField]
        [ReadOnly]
        [Header("地面检测")]
        bool isGrounded = false;

        [SerializeField]
        [ReadOnly]
        [Header("2D刚体")]
        Rigidbody2D rb;

        [SerializeField]
        [ReadOnly]
        [Header("2DSprite")]
        private SpriteRenderer spriteRenderer;

        #endregion

        private void Start()
        {
            rb = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            raycastDistance = spriteRenderer.bounds.size.y / 2 + 0.05f;
            speed = moveSpeed;
            AddSerializedJson.AddAllConverter();
        }

        private void Update()
        {
            isGrounded = IsGround();

            if (Input.GetMouseButtonDown(0))
            {
                UserData user = new UserData("GameJam存档",1,new Vector2(transform.position.x,transform.position.y));

                GameJamSaveSystem.SaveData(user);
            }

            Move();

            Jump();

        }

        bool IsGround()
        {
            return Physics2D.Raycast(spriteRenderer.bounds.center, Vector2.down, raycastDistance, groundLayer);
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
            {
                spriteRenderer.flipX = horizontalInput > 0 ? false : true;
            }

            Dash(horizontalInput);
        }

        private void Dash(float hor)
        {
            if(Input.GetKeyDown(KeyCode.LeftShift)&& hor != 0)
            {

                speed = moveSpeed * dashMutiple;
                Invoke("DashFinished", dashTimer);
            }

        }

        void DashFinished()
        {
            speed = moveSpeed;
        }

        private void OnCollisionEnter2D(Collision2D collision)
        {

            if (1<<collision.gameObject.layer == groundLayer)
            {
                jumpCount = 0; 
            }
        }


    }

}

