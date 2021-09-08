using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.UI;
using System;

public enum DataType
{ 
    None,
    Name,
    Race,
    Planet,
    Sex,
    Birthday,
    Birthplace,
    Weight,
    Height,
    ExpirationDate,
    Agency,
    Cargo,
    Photo,
}

public class InspectionManager : MonoBehaviour
{
    InspectionItem[] inspectedItems = { null, null };
    int nextItemIndex = 0;

    [SerializeField]
    Text callbackText;

    [SerializeField]
    Dictionary<DataType, string> incoherenceMessages = new Dictionary<DataType, string>();

    Immigrant currentImmigrant = null;

    [SerializeField]
    GameObject characterPrefab;

    [SerializeField]
    Transform parentTransform;

    [SerializeField]
    ImmigrantRandomizer immigrantRandomizer;

    InspectionManager()
    {
        if (incoherenceMessages.Count != 0)
            return;

        incoherenceMessages.Add(DataType.Race, "La race indiquée n'est pas la même.");
        incoherenceMessages.Add(DataType.Weight, "Le poids ne correspond pas.");
        incoherenceMessages.Add(DataType.Planet, "La planète ne correspond pas.");
    }


    public void Reset()
    {
        inspectedItems[0] = null;
        inspectedItems[1] = null;
        callbackText.text = "";
    }

    void SetMessage(string msg)
    {
        Debug.Assert(callbackText != null);
        callbackText.text = msg;
    }

    String CheckIncoherenceWithRule(Rule rule, InspectionItem otherItem)
    {
        if (rule == null)
            return null;

        return rule.CheckIncoherence(otherItem);
    }

    void GetDataTypeItem(DataType toGet, InspectionItem item1, InspectionItem item2, out InspectionItem wanted, out InspectionItem other)
    {
        if (item1.info.type == toGet)
        {
            wanted = item1;
            other = item2;
        }
        else if (item2.info.type == toGet)
        {
            wanted = item2;
            other = item1;
        }
        else
        {
            wanted = null;
            other = null;
        }
    }

    string CheckIncoherenceWithoutRules(InspectionItem item1, InspectionItem item2)
    {
        if (item1.info.type == item2.info.type)
        {
            if (item1.info.value != item2.info.value)
            {
                //string errorMessage;
                //if (incoherenceMessages.TryGetValue(item1.info.type, out errorMessage))
                //{
                //    return errorMessage;
                //}
                //else
                //{
                //    Debug.LogError("Message not implemented");
                //}
                return "Les données ne correspondent pas.";
            }
        }
        else
        {
            InspectionItem raceItem;
            InspectionItem otherItem;
            GetDataTypeItem(DataType.Race, item1, item2, out raceItem, out otherItem);
            if (raceItem != null)
            {
                switch (otherItem.info.type)
                {
                    case DataType.Height:
                        if (immigrantRandomizer.IsValidHeight(raceItem.info.value, DataConversions.FromString(otherItem.info.value)))
                        {
                            return null;
                        }
                        else
                        {
                            return "La taille est invalide.";
                        }

                    case DataType.Weight:
                        if (immigrantRandomizer.IsValidWeight(raceItem.info.value, DataConversions.FromString(otherItem.info.value)))
                        {
                            return null;
                        }
                        else
                        {
                            return "Le poids est invalide.";
                        }
                }
            }
        }

        return null;
    }

    void CheckIncoherence()
    {
        if (inspectedItems[0] == null || inspectedItems[1] == null)
            return;

        Rule rule1 = inspectedItems[0] as Rule;
        Rule rule2 = inspectedItems[1] as Rule;
        if (rule1 != null && rule2 != null)
            return;

        String message = null;
        message = CheckIncoherenceWithRule(rule1, inspectedItems[1]);
        if (message ==  null)
            message = CheckIncoherenceWithRule(rule2, inspectedItems[0]);
        if (message == null)
            message = CheckIncoherenceWithoutRules(inspectedItems[0], inspectedItems[1]);
        if (message == null)
            message = "Aucune incohérence trouvée.";

        SetMessage(message);
    }

    public void InspectItem(InspectionItem inspectedItem)
    {
        if (inspectedItems[0] == inspectedItem)
        {
            inspectedItems[0].IsSelected = false;
            SetMessage("");
            inspectedItems[0] = null;
            return;
        }
        if (inspectedItems[1] == inspectedItem)
        {
            inspectedItems[1].IsSelected = false;
            SetMessage("");
            inspectedItems[1] = null;
            return;
        }

        if (inspectedItems[nextItemIndex] != null)
            inspectedItems[nextItemIndex].IsSelected = false;

        inspectedItems[nextItemIndex] = inspectedItem;
        inspectedItems[nextItemIndex].IsSelected = true;
        nextItemIndex = (nextItemIndex + 1) % 2;

        CheckIncoherence();
    }

    public void GenerateNewImmigrant()
    {
        currentImmigrant = Instantiate(characterPrefab, parentTransform).GetComponent<Immigrant>();
        immigrantRandomizer.SetRandomData(currentImmigrant);
    }

    private void Start()
    {
        if (currentImmigrant == null)
        {
            GenerateNewImmigrant();
        }
    }
}
