using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WeaponLicenseLinker : MonoBehaviour
{
    [SerializeField] InspectionItem pName;
    [SerializeField] InspectionItem birthdate;
    [SerializeField] InspectionItem sex;
    [SerializeField] InspectionItem expirationDate;
    [SerializeField] InspectionItem birthplace;
    [SerializeField] InspectionItem photo;
    [SerializeField] InspectionItem planetIcon;

    private void Awake()
    {
        GetComponent<MyDragScript>().dragArea = GameObject.Find(DragList.dragListName).GetComponent<DragList>().dragArea;
    }

    public void SetText(WeaponLicense weaponLicense)
    {
        pName.Text = weaponLicense.name;
        sex.Text = weaponLicense.sex;
        birthdate.Text = DataConversions.ToString(weaponLicense.birthdate);
        expirationDate.Text = DataConversions.ToString(weaponLicense.expirationDate);
        birthplace.Text = weaponLicense.birthplace;

        photo.Image = weaponLicense.photo;
        planetIcon.Image = DataConversions.ToSprite(weaponLicense.originPlanet);
    }

    public void SetTypes()
    {
        pName.info.type = DataType.Name;
        sex.info.type = DataType.Sex;
        birthdate.info.type = DataType.Birthday;
        birthplace.info.type = DataType.Birthplace;
        expirationDate.info.type = DataType.ExpirationDate;

        photo.info.type = DataType.Photo;
        planetIcon.info.type = DataType.Planet;
    }

    public void SetValues(WeaponLicense weaponLicense)
    {
        pName.info.value = weaponLicense.name;
        sex.info.value = weaponLicense.sex;
        birthdate.info.value = DataConversions.ToString(weaponLicense.birthdate);
        birthplace.info.value = DataConversions.ToString(weaponLicense.birthplace);
        expirationDate.info.value = DataConversions.ToString(weaponLicense.expirationDate);

        photo.info.value = DataConversions.ToString(weaponLicense.photo);
        planetIcon.info.value = DataConversions.ToString(weaponLicense.originPlanet);
    }

    public void SetData(WeaponLicense weaponLicense)
    {
        SetText(weaponLicense);
        SetTypes();
        SetValues(weaponLicense);
    }
}
