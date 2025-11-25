using UnityEngine;
using UnityEngine.InputSystem;

public class MenuToggle : MonoBehaviour
{
    [SerializeField] InputActionReference toggleMenuAction;

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        gameObject.SetActive(false);
        toggleMenuAction.action.started += ButtonWasPressed;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ButtonWasPressed(InputAction.CallbackContext context)
    {
        gameObject.SetActive(!gameObject.activeSelf);
    }
}
