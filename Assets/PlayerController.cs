using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour {
    MovementComponent movementComponent;

    private void Awake()
    {
        movementComponent = GetComponent<MovementComponent>();
        if (movementComponent == null){
            Debug.LogError("there is no movement component" + gameObject.name);
            Destroy(this);
        }
    }
    // Use this for initialization
    void Start () {
		
	}

    // Update is called once per frame
    void Update() { 
    
    }

    private void FixedUpdate()
    {
        if (Input.GetKeyUp(KeyCode.LeftArrow))
        {

            //    rb2d.velocity.Set(0.0f, rb2d.velocity.y);
            movementComponent.MoveCharacter(0.0f);
            Debug.LogError("trying to set velocity to 0.");
        }

        //send input to the movement component in order to move character
        movementComponent.MoveCharacter(Input.GetAxis("Horizontal"));
        Debug.LogError("moving character");

        //check if the up arrow has been pressed and if so make the character jump.
        if (Input.GetKeyDown(KeyCode.UpArrow))
        {
            Debug.LogError("up arrow works");
            movementComponent.Jump();
        }

        


    }
}
