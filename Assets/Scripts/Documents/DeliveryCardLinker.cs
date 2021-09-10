using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DeliveryCardLinker : MonoBehaviour
{
    //[SerializeField] InspectionItem photo;
    [SerializeField] InspectionItem pName;
    [SerializeField] InspectionItem agency;
    [SerializeField] InspectionItem cargo;
    [SerializeField] InspectionItem originPlanet;
    [SerializeField] InspectionItem expirationDate;
    //[SerializeField] InspectionItem signature;


    private void Awake()
    {
        GetComponent<MyDragScript>().dragArea = GameObject.Find(DragList.dragListName).GetComponent<DragList>().dragArea;
    }

    public void SetText(DeliveryCard deliveryCard)
    {
        pName.Text = deliveryCard.name;
        //photo.Image = deliveryCard.photo;
        agency.Text = deliveryCard.agency;
        cargo.Text = deliveryCard.cargo;
        originPlanet.Text= DataConversions.ToString(deliveryCard.originPlanet);
        expirationDate.Text = DataConversions.ToString(deliveryCard.expirationDate);
        //signature.Text = deliveryCard.signature;
    }

    public void SetTypes()
    {
        pName.info.type = DataType.Name;
        //photo.info.type = DataType.Photo;
        agency.info.type = DataType.Agency;
        cargo.info.type = DataType.Cargo;
        originPlanet.info.type = DataType.Planet;
        expirationDate.info.type = DataType.ExpirationDate;
        //signature.info.type = DataType.Agency;
    }

    public void SetValues(DeliveryCard deliveryCard)
    {
        pName.info.value = deliveryCard.name;
        //photo.info.value = DataConversions.ToString(deliveryCard.photo);
        agency.info.value = deliveryCard.agency;
        cargo.info.value = deliveryCard.cargo;
        originPlanet.info.value = DataConversions.ToString(deliveryCard.originPlanet);
        expirationDate.info.value = DataConversions.ToString(deliveryCard.expirationDate);
        //signature.info.value = deliveryCard.signature;
    }

    public void SetData(DeliveryCard deliveryCard)
    {
        SetText(deliveryCard);
        SetTypes();
        SetValues(deliveryCard);
    }

}
