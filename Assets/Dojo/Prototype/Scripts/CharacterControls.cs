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

        /// <summary>
        /// Movement speed is linearly multiplied by this value.
        /// </summary>
        [SerializeField] private float moveSpeedModifier = 3;
        private Animator anim;
        private Rigidbody2D rb2d;
        private SpriteRenderer spriteRenderer;
        private Transform groundCheck;
        private float prevPos;  

        public void FixedUpdate()
        {
            HandleInput();

            if (Input.GetKeyDown(KeyCode.LeftArrow))
            {
                spriteRenderer.flipX = true;

            }
            if (Input.GetKeyDown(KeyCode.RightArrow))
            {
                spriteRenderer.flipX = false;
            }

            if (prevPos == transform.position.x)
            {
                anim.SetBool("LeftRight", false);
                Debug.Log("LeftRight is false");
            }
            else
            {
                anim.SetBool("LeftRight", true);
                Debug.Log("LeftRight is true");
            }
            prevPos = transform.position.x;
        }

        private void HandleInput()
        {
            Vector2 moveDirection = GetInput();
            actor.transform.Translate(moveDirection * Time.deltaTime * moveSpeedModifier, Space.World);
        }

        private Vector2 GetInput()
        {
            Vector2 input = Vector2.zero;
            input.x = Input.GetAxisRaw("Horizontal");
            return input;
        }

        private void Awake()
        {
            anim = GetComponent<Animator>();
            rb2d = GetComponent<Rigidbody2D>();
            spriteRenderer = GetComponent<SpriteRenderer>();
            prevPos = rb2d.transform.position.x;
            //Check if the groundCheck variable is set
            //if (groundCheck == null)
            //{
            //    Debug.LogError("Ground Check missing from the MovementComponent, please set one.");
            //    Destroy(this);
            //}
        }

        private void Update()
        {
            //Check in which direction the sprite should face and flip accordingly

            if (Input.GetKeyDown(KeyCode.DownArrow))
            {
                anim.SetBool("Iscrouch", true);
                Debug.Log("Iscrouch is true");
            }

            if (Input.GetKeyUp(KeyCode.DownArrow))
            {
                anim.SetBool("Iscrouch", false);
            }




            //Set the Jump parameter in the Animation State Machine
        }
    }
}
