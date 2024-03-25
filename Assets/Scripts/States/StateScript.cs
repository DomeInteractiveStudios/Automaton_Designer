using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;   // Required when Using UI elements.

public class StateScript : MonoBehaviour
{
    public enum StateType 
    {
        regular, 
        final
    };

    [System.Serializable] public struct State {
        public StateType type;
        public string name;
    };

    public State state; 

    private TransitionGenerator transitionGenerator; // Reference to the TransitionGenerator component.
    private Transform visuals; // Reference to the Transform component.
    private SpriteRenderer spriteRenderer; // Reference to the SpriteRenderer component.
    [SerializeField] private Sprite[] sprites; // Array of Sprites. Each sprite is a diffrent state
    private bool move = false; // Boolean to check if the state is moving or not.
    private Vector3 mousePosition; // The target position of the state. 
    [SerializeField] private Checks checks; // Reference to the Checks scriptable object.

    private void Awake()
    {
        visuals = transform.Find("Visuals"); // Find the child object named "Visuals".
        spriteRenderer = visuals.Find("StateTypeVisual").GetComponent<SpriteRenderer>(); // Get the SpriteRenderer component from the child object.
        transitionGenerator = GetComponent<TransitionGenerator>(); // Get the TransitionGenerator script
    }

    private void Update()
    {
        if(checks.canModify) // If the state can be modified
        {
            mousePosition = Camera.main.ScreenToWorldPoint(Input.mousePosition); // Get the mouse position in the world space.

            if(state.type == StateType.final)
            {
                spriteRenderer.sprite = sprites[1]; // Set the sprite to the final state sprite
            }
            if(state.type == StateType.regular)
            {
                spriteRenderer.sprite = sprites[0]; // Set the sprite to the regular state sprite
            }
        }
    }

    private void LateUpdate()
    {
        if(move && checks.canModify) transform.position = new Vector3(mousePosition.x, mousePosition.y, 0); // Move the state to the mouse position.
    }

    public void OnMouseDown()
    {
        if(checks.canModify)
        {
            if(Input.GetMouseButtonDown(2) || (Input.GetMouseButtonDown(0) && Input.GetKey(KeyCode.LeftControl))) // If the right mouse button is clicked or the left mouse button is clicked while holding the left control key.
            {
                if(!transitionGenerator.isDragging) move = true; // If the transition is not being dragged, set the move boolean to true.
                else transitionGenerator.end = transform; // If the transition is being dragged, set the end of the transition to this state.
            }
        }
        
    }

    public void OnMouseUp()
    {
        if(checks.canModify)
        {
            move = false; // Set the move boolean to false. (no check is needed because move will raemin false if it's already false, and will be set to false if it's true, either way it will be false)
            if(transitionGenerator.isDragging) transitionGenerator.EndDragging(); // If the transition is being dragged, end the dragging.
        }
    }
}
