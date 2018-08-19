using UnityEngine;

namespace MyCompany.RogueSmash.Prototype
{
    public class CharacterControls : MonoBehaviour
    {
        /// <summary>
        /// The actor is the game object in the game world
        /// representing this character/entity.
        /// </summary>
        [SerializeField] private GameObject actor;
        [SerializeField] private Transform groundCheck;

        /// <summary>
        /// Movement speed is linearly multiplied by this value.
        /// </summary>
        [SerializeField] private float speed = 3f;
        [SerializeField] private float jumpForce = 700f;
        private Animator anim;
        private Rigidbody2D rb2d;
        private SpriteRenderer spriteRenderer;
        private BoxCollider2D bc2d;
        private Vector2 movement;
        private bool grounded = true;
        private bool bc2dFlipped = false;
       [SerializeField] private float h;        
       [SerializeField] private float v;
        private bool idlePlay = false;

        public void FixedUpdate()
        {
            h = Input.GetAxisRaw("Horizontal");
            v = Input.GetAxisRaw("Vertical");

            //if (h > 0)
            //{
            //    spriteRenderer.flipX = false;
            //}
            //else if (h < 0)
            //{
            //    spriteRenderer.flipX = true;
            //}

            grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
             
            anim.SetBool("ground", grounded);
            
            anim.SetFloat("vSpeed", rb2d.velocity.y);
            anim.SetFloat("Speed", Mathf.Abs(h));

            if (anim.GetBool("Iscrouch") == true) return;


            rb2d.velocity = new Vector2(h * speed, rb2d.velocity.y);

            if ((v > 0) && grounded)
            {
                anim.SetBool("ground", false);
                rb2d.AddForce(new Vector2(0, jumpForce));
            }

            Ismoving(h);
 
        }

        void Ismoving (float h)
        {
            bool moving = h != 0f;
            anim.SetBool("Ismoving", moving);
        }

        private void Awake()
        {
            anim = GetComponent<Animator>();
            rb2d = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            bc2d = GetComponent<BoxCollider2D>();

            if (groundCheck == null)
            {
                Debug.LogError("Ground Check missing from the MovementComponent, please set one.");
                Destroy(this);
            }
        }

        private void Update()
        {
            //Check in which direction the sprite should face and flip accordingly

            if (h > 0)
            {
                idlePlay = false;
                spriteRenderer.flipX = false;
            }
            else if (h < 0)
            {
                idlePlay = false;
                spriteRenderer.flipX = true;
            }
            else if (h == 0 && !idlePlay)
            {
                idlePlay = true;
                for (int count = 1; count <= 3; count++)
                {
                    Debug.Log("idle animation played" + count);
                    anim.Play("Idle");
                }
            }

            if (spriteRenderer.flipX && !bc2dFlipped)
            {
                //Debug.LogError("bc2dFlipped is:" + bc2dFlipped);
                bc2d.offset = new Vector2(bc2d.offset.x * -1, bc2d.offset.y);
                bc2dFlipped = true;
            }
            else if (!spriteRenderer.flipX && bc2dFlipped)
            {
                // Debug.LogError("bc2dFlipped is:" + bc2dFlipped);
                bc2d.offset = new Vector2(bc2d.offset.x * -1, bc2d.offset.y);
                bc2dFlipped = false;
            }

            if (v < 0)
            {
                anim.SetBool("Iscrouch", true);
            }
            if (v == 0)
            {
                anim.SetBool("Iscrouch", false);
            }
        }
       
    }
}
