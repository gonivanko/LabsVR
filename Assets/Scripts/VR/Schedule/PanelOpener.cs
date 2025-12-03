using UnityEngine;
using UnityEngine.InputSystem;

public class PanelOpener : MonoBehaviour
{
    [SerializeField] private InputActionReference inputActionReference;
    [SerializeField] private GameObject panel;

    private bool buttonIsPressed;
    private bool isHovering;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        panel.SetActive(false);
        inputActionReference.action.started += ButtonWasPressed;
    }

    // Update is called once per frame
    private void Update()
    {
        //panel.SetActive(buttonIsPressed && isHovering);
    }

    private void ButtonWasPressed(InputAction.CallbackContext context)
    {
        buttonIsPressed = !buttonIsPressed;

        panel.SetActive(buttonIsPressed && isHovering);
    }

    public void OnHoverEnter()
    {
        isHovering = true;

        panel.SetActive(buttonIsPressed && isHovering);
    }

    public void OnHoverExit()
    {
        isHovering = false;
    }
}