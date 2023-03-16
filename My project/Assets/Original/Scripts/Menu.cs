using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;
using UnityEngine.UI;

public class Menu : MonoBehaviour
{
   
    public GameObject firstSelected;
    private GameObject currentSelected;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        currentSelected = EventSystem.current.currentSelectedGameObject;
        if(currentSelected != null && currentSelected.transform.parent != null && currentSelected.transform.parent.name == "Lore images")
        {
            currentSelected.GetComponent<SelectedItem>().swapColour();
        }
    }

    private void OnEnable()
    {
        EventSystem.current.SetSelectedGameObject(null);
        EventSystem.current.SetSelectedGameObject(firstSelected);
    }

    public void loreMouseClick(InputAction.CallbackContext context)
    {
        if(context.canceled && currentSelected != null && currentSelected.transform.parent != null && currentSelected.transform.parent.name == "Lore images")
        {
            currentSelected.GetComponent<SelectedItem>().controllerSelected();
        }
    }

    public void click(InputAction.CallbackContext context)
    {
        // print("hi");
        if (context.canceled)
        {
            // EventSystem.current.gameObject.GetComponent<Button>().onClick.Invoke();
            // print(EventSystem.current.gameObject);
            var eventSystem = EventSystem.current;
            ExecuteEvents.Execute(currentSelected, new BaseEventData(eventSystem), ExecuteEvents.submitHandler);

            if (currentSelected != null && currentSelected.transform.parent != null && currentSelected.transform.parent.name == "Lore images")
            {
                currentSelected.GetComponent<SelectedItem>().controllerSelected();
            }
        }
    }
}
