using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PassportLinker : MonoBehaviour
{
    // Texts
    [SerializeField] InspectionItem pName;
    [SerializeField] InspectionItem planet;
    [SerializeField] InspectionItem race;
    [SerializeField] InspectionItem weight;
    [SerializeField] InspectionItem height;
    [SerializeField] InspectionItem sex;
    [SerializeField] InspectionItem birthdate;
    [SerializeField] InspectionItem birthplace;
    [SerializeField] InspectionItem expirationDate;

    // Images
    [SerializeField] InspectionItem planetIcon;
    [SerializeField] InspectionItem photo;
    [SerializeField] InspectionItem planetSeal;

    private void Awake()
    {
        GetComponent<MyDragScript>().dragArea = GameObject.Find(DragList.dragListName).GetComponent<DragList>().dragArea;
    }

    public void SetText(Passport passport)
    {
        pName.Text = passport.name;
        planet.Text = DataConversions.ToString(passport.planet);
        race.Text = passport.race;
        weight.Text = DataConversions.ToString(passport.weight);
        height.Text = DataConversions.ToString(passport.height);
        sex.Text = passport.sex;
        birthdate.Text = DataConversions.ToString(passport.birthdate);
        birthplace.Text = passport.birthPlace;
        expirationDate.Text = DataConversions.ToString(passport.expirationDate);

        //planetIcon.Image = DataConversions.ToSprite(passport.planetIcon);
        photo.Image = passport.photo;
        planetSeal.Image = passport.planetSeal;
    }

    public void SetTypes()
    {
        pName.info.type = DataType.Name;
        planet.info.type = DataType.Planet;
        race.info.type = DataType.Race;
        weight.info.type = DataType.Weight;
        height.info.type = DataType.Height;
        sex.info.type = DataType.Sex;
        birthdate.info.type = DataType.Birthday;
        birthplace.info.type = DataType.Birthplace;
        expirationDate.info.type = DataType.ExpirationDate;

        planetIcon.info.type = DataType.Planet;
        photo.info.type = DataType.Photo;
        planetSeal.info.type = DataType.Planet;
    }

    public void SetValues(Passport passport)
    {
        pName.info.value = passport.name;
        planet.info.value = DataConversions.ToString(passport.planet);
        race.info.value = passport.race;
        weight.info.value = DataConversions.ToString(passport.weight);
        height.info.value = DataConversions.ToString(passport.height);
        sex.info.value = passport.sex;
        birthdate.info.value = DataConversions.ToString(passport.birthdate);
        birthplace.info.value = passport.birthPlace;
        expirationDate.info.value = DataConversions.ToString(passport.expirationDate);

        planetIcon.info.value = DataConversions.ToString(passport.planetIcon);
        photo.info.value = DataConversions.ToString(passport.photo);
        planetSeal.info.value = DataConversions.ToString(passport.planetSeal);
    }

    public void SetData(Passport passport)
    {
        SetText(passport);
        SetTypes();
        SetValues(passport);
    }

}
