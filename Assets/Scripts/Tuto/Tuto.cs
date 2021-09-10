using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

[RequireComponent(typeof(CanvasGroup))]
public class Tuto : MonoBehaviour
{
    CanvasGroup image;

    [SerializeField]
    float fadeDuration = 3;

    [SerializeField]
    float waitTime = 3;

    [SerializeField]
    InspectionManager inspectionManager;

    Coroutine coroutine;


    private void Awake()
    {
        image = GetComponent<CanvasGroup>();
    }

    private void Start()
    {
        coroutine = StartCoroutine(FadeOut());

        EventTrigger eventTrigger = GetComponent<EventTrigger>();
        AddCallback(eventTrigger, EventTriggerType.PointerDown, ForceStop);
    }

    void AddCallback(EventTrigger eventTrigger, EventTriggerType triggerType, UnityAction<BaseEventData> action)
    {
        if (eventTrigger == null)
        {
            Debug.LogWarning("Buttons should have an EventTrigger.");
            return;
        }

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

    IEnumerator FadeOut()
    {
        yield return new WaitForSeconds(waitTime);

        for (float i = 0; i < fadeDuration; i += Time.deltaTime)
        {
            image.alpha = 1f - i / fadeDuration;
            yield return null;
        }

        inspectionManager.GenerateNewImmigrant();
        Destroy(image.gameObject);
    }


    void ForceStop(BaseEventData e)
    {
        StopCoroutine(coroutine);
        inspectionManager.GenerateNewImmigrant();
        Destroy(image.gameObject);
    }
}
