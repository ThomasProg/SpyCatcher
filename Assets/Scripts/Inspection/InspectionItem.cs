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

[System.Serializable]
struct ColorInteraction
{
    public Color defaultColor;
    public Color onHover;
    public Color onSelection;
};


public class InspectionItem : MonoBehaviour
{
    Text text = null;
    Image image = null;
    string inspectionManagerName = "InspectionManager";
    protected InspectionManager inspectionManager = null;

    [SerializeField]
    ColorInteraction imageColor;

    [SerializeField]
    ColorInteraction textColor;

    [SerializeField]
    public InspectionItemInfo info;

    bool isSelected = false;

    public string Text
    {
        set
        {
            if (text != null)
                text.text = value;
        }
    }

    public Sprite Image
    {
        set
        {
            if (image != null)
                image.sprite = value;
        }
    }

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
                    text.color = textColor.onSelection;
                }
                if (image != null)
                    image.color = imageColor.onSelection;
            }
            else
            {
                isSelected = false;
                if (text != null)
                {
                    text.color = textColor.defaultColor;
                }
                if (image != null)
                    image.color = imageColor.defaultColor;
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
    void Awake()
    {
        text = GetComponent<Text>();
        image = GetComponent<Image>();
        inspectionManager = GameObject.Find(inspectionManagerName).GetComponent<InspectionManager>();

        EventTrigger eventTrigger = GetComponent<EventTrigger>();
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
            text.color = textColor.onHover;
        }

        if (image != null && !IsSelected)
        {
            image.color = imageColor.onHover;
        }
    }

    public void OnPointerExit(BaseEventData data)
    {
        if (text != null && !IsSelected)
        {
            text.color = textColor.defaultColor;
        }

        if (image != null && !IsSelected)
        {
            image.color = imageColor.defaultColor;
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
