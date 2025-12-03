using UnityEngine;
using UnityEngine.InputSystem;

public class MenuToggle : MonoBehaviour
{
    [SerializeField] private InputActionReference toggleMenuAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    private void Start()
    {
        gameObject.SetActive(false);
        toggleMenuAction.action.started += ButtonWasPressed;
    }

    // Update is called once per frame
    private void Update()
    {
    }

    private void ButtonWasPressed(InputAction.CallbackContext context)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}