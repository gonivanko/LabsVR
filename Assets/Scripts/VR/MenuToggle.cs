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
        //toggleMenuAction.action.canceled += ButtonWasReleased;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void ButtonWasPressed(InputAction.CallbackContext context)
    {
        Debug.Log("Button Pressed " + gameObject.name);
        gameObject.SetActive(!gameObject.activeSelf);
    }

    //private void ButtonWasReleased(InputAction.CallbackContext context)
    //{
    //    Debug.Log("Button Released");
    //    gameObject.SetActive(!gameObject.activeSelf);
    //}
}
