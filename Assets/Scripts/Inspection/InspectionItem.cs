using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[System.Serializable]
public struct InspectionItemInfo
{
    [SerializeField]
    public DataType type;
    [SerializeField]
    public string value;
};

public class InspectionItem : MonoBehaviour
{
    Text text = null;
    string inspectionManagerName = "InspectionManager";
    protected InspectionManager inspectionManager = null;

    [SerializeField]
    public InspectionItemInfo info;

    bool isSelected = false;

    public bool IsSelected
    {
        get
        {
            return isSelected;
        }
        set
        { 
            if (value)
            {
                isSelected = true;
                if (text != null)
                {
                    text.color = Color.yellow;
                }
            }
            else
            {
                isSelected = false;
                if (text != null)
                {
                    text.color = Color.black;
                }
            }
        }
    }


    void AddCallback(EventTrigger eventTrigger, EventTriggerType triggerType, UnityAction<BaseEventData> action)
    {
        List<EventTrigger.Entry> triggers = eventTrigger.triggers;
        EventTrigger.Entry clickEventHandler = triggers.Find(
            t => t.eventID == triggerType
        );
        if (clickEventHandler == null)
        {
            clickEventHandler = new EventTrigger.Entry();
            clickEventHandler.eventID = triggerType;
            triggers.Add(clickEventHandler);
            eventTrigger.triggers = triggers;
        }

        clickEventHandler.callback.AddListener(action);
    }


    // Start is called before the first frame update
    void Start()
    {
        text = GetComponent<Text>();
        inspectionManager = GameObject.Find(inspectionManagerName).GetComponent<InspectionManager>();
        EventTrigger eventTrigger = GetComponent<EventTrigger>();

        UnityAction<BaseEventData> action = null;
        action += OnMouseDown; 
        AddCallback(eventTrigger, EventTriggerType.PointerDown, OnMouseDown);
        AddCallback(eventTrigger, EventTriggerType.PointerUp, OnMouseUp);
        AddCallback(eventTrigger, EventTriggerType.PointerEnter, OnPointerEnter);
        AddCallback(eventTrigger, EventTriggerType.PointerExit, OnPointerExit);
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void OnPointerEnter(BaseEventData data)
    {
        if (text != null && !IsSelected)
        {
            text.color = Color.red;
        }
    }

    public void OnPointerExit(BaseEventData data)
    {
        if (text != null && !IsSelected)
        {
            text.color = Color.black;
        }
    }

    public void OnMouseDown(BaseEventData data)
    {
        inspectionManager.InspectItem(this);
    }

    public void OnMouseUp(BaseEventData data)
    {


    }


    //public static bool ShouldRaceBe(InspectionItem item, string race)
    //{
    //    if ()
    //    return item.info.type == 
    //}
}
