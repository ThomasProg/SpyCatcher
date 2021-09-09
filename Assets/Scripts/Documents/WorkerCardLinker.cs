using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WorkerCardLinker : MonoBehaviour
{
    // Texts
    [SerializeField] InspectionItem pName;
    [SerializeField] InspectionItem planet;
    [SerializeField] InspectionItem sex;
    [SerializeField] InspectionItem birthdate;
    [SerializeField] InspectionItem expirationDate;

    // Images
    [SerializeField] InspectionItem photo;
    [SerializeField] InspectionItem agencySeal;

    private void Awake()
    {
        GetComponent<MyDragScript>().dragArea = GameObject.Find(DragList.dragListName).GetComponent<DragList>().dragArea;
    }

    public void SetText(WorkerCard workerCard)
    {
        pName.Text = workerCard.name;
        planet.Text = DataConversions.ToString(workerCard.planet);
        sex.Text = workerCard.sex;
        birthdate.Text = DataConversions.ToString(workerCard.birthdate);
        expirationDate.Text = DataConversions.ToString(workerCard.expirationDate);

        photo.Image = workerCard.photo;
        agencySeal.Image = workerCard.agencySeal;
    }

    public void SetTypes()
    {
        pName.info.type = DataType.Name;
        planet.info.type = DataType.Planet;
        sex.info.type = DataType.Sex;
        birthdate.info.type = DataType.Birthday;
        expirationDate.info.type = DataType.ExpirationDate;

        photo.info.type = DataType.Photo;
        agencySeal.info.type = DataType.Agency;
    }

    public void SetValues(WorkerCard workerCard)
    {
        pName.info.value = workerCard.name;
        planet.info.value = DataConversions.ToString(workerCard.planet);
        sex.info.value = workerCard.sex;
        birthdate.info.value = DataConversions.ToString(workerCard.birthdate);
        expirationDate.info.value = DataConversions.ToString(workerCard.expirationDate);

        photo.info.value = DataConversions.ToString(workerCard.photo);
        agencySeal.info.value = DataConversions.ToString(workerCard.agencySeal);
    }

    public void SetData(WorkerCard workerCard)
    {
        SetText(workerCard);
        SetTypes();
        SetValues(workerCard);
    }
}
