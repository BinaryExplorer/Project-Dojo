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
        private float tagSpeed = 0;

        public void FixedUpdate()
        {
            float h = Input.GetAxisRaw("Horizontal");
            float v = Input.GetAxisRaw("Vertical");
            

            
            //if (h > 0)
            //{
            //    spriteRenderer.flipX = false;
            //}
            //else if (h < 0)
            //{
            //    spriteRendere.r.flipX = true;
            //}

            grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
             
            anim.SetBool("ground", grounded);
            
            anim.SetFloat("vSpeed", rb2d.velocity.y);

            Debug.LogError("max vspeed is: " + anim.GetFloat("vSpeed"));

            anim.SetFloat("Speed", Mathf.Abs(h));

            if (anim.GetBool("Iscrouch")) return;

            rb2d.velocity = new Vector2(h * speed, rb2d.velocity.y);

            if (h > 0)
            {
                spriteRenderer.flipX = false;
            }
            else if (h < 0)
            {
                spriteRenderer.flipX = true;
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
            if ((Input.GetKeyDown(KeyCode.Space) && grounded))
            {
                anim.SetBool("ground", false);
                rb2d.AddForce(new Vector2(0, jumpForce));
            }
            //Check in which direction the sprite should face and flip accordingly
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

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                anim.SetBool("Iscrouch", true);
            }
            else
            {
                anim.SetBool("Iscrouch", false);
            }
        }
       
    }
}
