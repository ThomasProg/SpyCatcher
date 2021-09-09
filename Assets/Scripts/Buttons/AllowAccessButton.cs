using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;


public class AllowAccessButton : MonoBehaviour
{
	[SerializeField] bool isAllowingNotDenying = true;
	private Button button;
	private InspectionManager inspectionManager;

	void Start()
	{
		button = GetComponent<Button>();
		button.onClick.AddListener(TaskOnClick);

		inspectionManager = GameObject.Find("InspectionManager").GetComponent<InspectionManager>();
		Debug.Assert(inspectionManager != null);
	}

	void TaskOnClick()
	{
		if (isAllowingNotDenying)
			inspectionManager.AllowAccess();
		else
			inspectionManager.DenyAccess();
	}
}
