using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DiplomaticLetterLinker : MonoBehaviour
{
    [SerializeField] InspectionItem pName;
    [SerializeField] InspectionItem seal;
    [SerializeField] InspectionItem creationDate; // The date when the card was made
    [SerializeField] InspectionItem creationCity; // The city the card was made in

    private void Awake()
    {
        GetComponent<MyDragScript>().dragArea = GameObject.Find(DragList.dragListName).GetComponent<DragList>().dragArea;
    }

    public void SetText(DiplomaticLetter diplomaticLetter)
    {
        pName.Text = diplomaticLetter.name;
        seal.Text = DataConversions.ToString(diplomaticLetter.seal);
        creationDate.Text = DataConversions.ToString(diplomaticLetter.creationDate);
        creationCity.Text = diplomaticLetter.creationCity;
    }

    public void SetTypes()
    {
        pName.info.type = DataType.Name;
        seal.info.type = DataType.Planet;
        creationDate.info.type = DataType.DiplomaticLetterCreationDate;
        creationCity.info.type = DataType.City;
    }

    public void SetValues(DiplomaticLetter diplomaticLetter)
    {
        pName.info.value = diplomaticLetter.name;
        seal.info.value = DataConversions.ToString(diplomaticLetter.seal);
        creationDate.info.value = DataConversions.ToString(diplomaticLetter.creationDate);
        creationCity.info.value = diplomaticLetter.creationCity;
    }

    public void SetData(DiplomaticLetter diplomaticLetter)
    {
        SetText(diplomaticLetter);
        SetTypes();
        SetValues(diplomaticLetter);
    }
}
