using UnityEngine;
using UnityEngine.InputSystem;

public class InfoViewer : MonoBehaviour
{
    [SerializeField] InputActionReference inputActionReference;
    [SerializeField] GameObject infoPanel;
    
    private bool buttonIsPressed = false;
    private bool isHovering = false;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        //gameObject.SetActive(false);
        inputActionReference.action.started += ButtonWasPressed;
        inputActionReference.action.canceled += ButtonWasReleased;
    }

    // Update is called once per frame
    private void Update()
    {
        Debug.Log("Button State: " + buttonIsPressed);
        infoPanel.SetActive(buttonIsPressed && isHovering);
    }

    private void ButtonWasPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Button Pressed " + gameObject.name);
        buttonIsPressed = true;
        
    }
    private void ButtonWasReleased(InputAction.CallbackContext context)
    {
        Debug.Log("Button Released " + gameObject.name);
        buttonIsPressed = false;
        //gameObject.SetActive(false);
    }

    public void OnHoverEnter()
    {
        Debug.Log("Hover Entered " + gameObject.name);
        isHovering = true;
    }
    public void OnHoverExit()
    {
        Debug.Log("Hover Exited " + gameObject.name);
        isHovering = false;
    }
}
