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
        private bool grounded = false;
        private bool bc2dFlipped = false;


        public void FixedUpdate()
        {
            //is the player moving horizontal?
            float h = Input.GetAxisRaw("Horizontal");
            //freeze vertical axis unless jumping
            float v = Input.GetAxisRaw("Vertical");

            grounded = Physics2D.Linecast(transform.position, groundCheck.position, 1 << LayerMask.NameToLayer("Ground"));
             
            anim.SetBool("ground", grounded);
            
            anim.SetFloat("vSpeed", rb2d.velocity.y);
            anim.SetFloat("Speed", Mathf.Abs(h));

            if (anim.GetBool("Iscrouch") == true) return;


            rb2d.velocity = new Vector2(h * speed, rb2d.velocity.y);

            if (spriteRenderer.flipX && !bc2dFlipped )
            {
                //Debug.LogError("bc2dFlipped is:" + bc2dFlipped);
                bc2d.offset = new Vector2(bc2d.offset.x * -1, bc2d.offset.y);
                bc2dFlipped = true;
            }
            else if(!spriteRenderer.flipX && bc2dFlipped)
            {
               // Debug.LogError("bc2dFlipped is:" + bc2dFlipped);
                bc2d.offset = new Vector2(bc2d.offset.x * -1, bc2d.offset.y);
                bc2dFlipped = false;
            }

            //check to execute run anim
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
            //ONLY SPRITE RENDERER STUFF SHOULD GO HERE.

            if ((Input.GetKeyDown(KeyCode.UpArrow)) && grounded) {
                anim.SetBool("ground", false);
                rb2d.AddForce(new Vector2(0, jumpForce));
            }

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                spriteRenderer.flipX = true;
                anim.SetBool("Ismoving", true);

            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                spriteRenderer.flipX = false;
                anim.SetBool("Ismoving", true);
            }
            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                anim.SetBool("Iscrouch", true);
                Debug.Log("Iscrouch is true");
            }
            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                anim.SetBool("Iscrouch", false);
                Debug.Log("Iscrouch is false");
            }
        }
       
    }
}
