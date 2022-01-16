using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using UnityEngine.Events;

public class AllowAccessButton : MonoBehaviour
{
	[SerializeField] bool isAllowingNotDenying = true;
	private Button button;
	private InspectionManager inspectionManager;
	private ImmigrantManager immigrantManager;

	[SerializeField]
	private AudioClip onPressSound;
	[SerializeField]
	private AudioClip onReleaseSound;

	AudioSource audioSource;

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

	void Start()
	{
		audioSource = GetComponent<AudioSource>();
		button = GetComponent<Button>();
		button.onClick.AddListener(OnClick);
		EventTrigger eventTrigger = GetComponent<EventTrigger>();
		AddCallback(eventTrigger, EventTriggerType.PointerDown, OnMouseDown);
		AddCallback(eventTrigger, EventTriggerType.PointerDown, OnMouseUp);

		//inspectionManager = GameObject.Find("InspectionManager").GetComponent<InspectionManager>();
		//Debug.Assert(inspectionManager != null);

		immigrantManager = GameObject.Find("ImmigrantManager").GetComponent<ImmigrantManager>();
	}

	void OnClick()
	{
		if (isAllowingNotDenying)
			immigrantManager.AllowAccess();
		else
			immigrantManager.DenyAccess();
	}

	void OnMouseDown(BaseEventData e)
	{
		audioSource.clip = onPressSound;
		audioSource.Play();
	}

	void OnMouseUp(BaseEventData e)
	{
		audioSource.clip = onReleaseSound;
		audioSource.Play();
	}
}
