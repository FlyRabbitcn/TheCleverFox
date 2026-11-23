using FlyRabbit.EventBus;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

[RequireComponent(typeof(Button))]
public class MapWorkshopItemSelector : MonoBehaviour
{

    public string ItemName;


    private Button m_Button;


    private static EventMessage.UI.OnMapWorkshopItemSelected m_Message = new EventMessage.UI.OnMapWorkshopItemSelected();

    private void Awake()
    {
        m_Button = GetComponent<Button>();

        m_Button.onClick.AddListener(OnButtonClick);
    }




    private void OnButtonClick()
    {
        m_Message.ItemSelector = this;
        EventBus.TriggerEvent<EventMessage.UI.OnMapWorkshopItemSelected>(m_Message);
    }
}
