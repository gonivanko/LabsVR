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
        inputActionReference.action.started += ButtonWasPressed;
        inputActionReference.action.canceled += ButtonWasReleased;
    }

    // Update is called once per frame
    private void Update()
    {
        infoPanel.SetActive(buttonIsPressed && isHovering);
    }

    private void ButtonWasPressed(InputAction.CallbackContext context)
    {
        buttonIsPressed = true;
        
    }
    private void ButtonWasReleased(InputAction.CallbackContext context)
    {
        buttonIsPressed = false;
    }

    public void OnHoverEnter()
    {
        isHovering = true;
    }
    public void OnHoverExit()
    {
        isHovering = false;
    }
}
