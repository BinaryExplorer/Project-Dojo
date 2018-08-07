using MyCompany.GameFramework.InputManagement;
using MyCompany.GameFramework.Physics.Interfaces;
using MyCompany.RogueSmash.Player;
using UnityEngine;

namespace MyCompany.RogueSmash.InputManagement
{
	public class SampleCharacterController : MonoBehaviour
	{
		[Header("Data Templates")]
		[SerializeField] private CharacterDataTemplate characterDataTemplate;
		
		[Header("Other")]
		private InputManager inputManager;
		private Rigidbody rigidbody;

		void Awake()
		{
			rigidbody = GetComponent<Rigidbody>();
			inputManager = new InputManager(new SampleBindings(), new RadialMouseInputHandler());
			//inputManager.AddActionToBinding("shoot", Shoot);
		}

		void FixedUpdate ()
		{
			CheckForInput();
		}
		
		private void OnCollisionEnter(Collision collision)
		{
			ICollisionEnterHandler[] handlers =
				collision.gameObject.GetComponents<ICollisionEnterHandler>();
			if (handlers != null)
			{
				foreach (var handler in handlers)
				{
					handler.Handle(this.gameObject, collision);
				}
			}
		}

		private void CheckForInput()
		{
			inputManager.CheckForInput();
			
			Vector2 input = Vector2.zero;
			input.x = inputManager.GetAxis("Horizontal");
			input.y = inputManager.GetAxis("Vertical");
			//transform.Translate(input* Time.deltaTime * characterDataTemplate.Data.MovementSpeed, Space.World);
			rigidbody.velocity = input * characterDataTemplate.Data.MovementSpeed;
		}
		}
	}
