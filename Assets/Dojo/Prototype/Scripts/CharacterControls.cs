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
        private Vector2 movement;
        private bool grounded = false;


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
           
            rb2d.velocity = new Vector2(h * speed, rb2d.velocity.y);

            Debug.LogError("velocity" + rb2d.velocity);

            //nextMovement(h, v);

            //check to execute run anim
            Ismoving(h);
 
        }
        
        //private void nextMovement(float h, float v)
        //{
           
        //    movement.Set(h, v);
        //    movement = movement * speed * Time.deltaTime;
        //    rb2d.MovePosition (rb2d.position + movement);
        //}

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

            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                spriteRenderer.flipX = false;
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
