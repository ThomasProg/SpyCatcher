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
    City, // (Diplomatic letter)
    DiplomaticLetterCreationDate,
    WeaponLicenseCategory
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
    public ImmigrantRandomizer immigrantRandomizer;

    [SerializeField]
    Text scoreText;

    [SerializeField]
    int currentScore = 0;

    [SerializeField]
    News news;

    [SerializeField]
    Lasers lasers;

    public int CurrentScore
    {
        get { return currentScore; }
        set
        {
            currentScore = value;
            scoreText.text = currentScore.ToString();
        }
    }


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

    string CheckIncoherenceWithRule(Rule rule, InspectionItem otherItem)
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

    string CheckIncoherenceWithRace(InspectionItem item1, InspectionItem item2)
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

                case DataType.Birthday:
                    if (immigrantRandomizer.HasValidAge(raceItem.info.value, DataConversions.StringToDate(otherItem.info.value)))
                    {
                        return null;
                    }
                    else
                    {
                        return "L'âge est invalide.";
                    }

                case DataType.Photo:
                    if (immigrantRandomizer.HasValidPhoto(raceItem.info.value, otherItem.info.value))
                    {
                        return null;
                    }
                    else
                    {
                        return "La photo n'est pas valide.";
                    }
            }
        }
        return null;
    }

    string CheckIncoherenceWithPlanet(InspectionItem item1, InspectionItem item2)
    {
        InspectionItem planetItem;
        InspectionItem otherItem;

        GetDataTypeItem(DataType.Race, item1, item2, out planetItem, out otherItem);
        if (planetItem != null)
        {
            switch (otherItem.info.type)
            {
                case DataType.City:
                    if (immigrantRandomizer.IsCityOnPlanet(otherItem.info.value, planetItem.info.value))
                    {
                        return null;
                    }
                    else
                    {
                        return "La ville ne correspond pas avecc la planète.";
                    }
            }
        }
        return null;
    }

    string CheckIncoherenceWithoutRules(InspectionItem item1, InspectionItem item2)
    {
        if (item1.info.type == item2.info.type && item1.info.type != DataType.ExpirationDate)
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
            string s;

            s = CheckIncoherenceWithRace(item1, item2);
            if (s != null)
                return s;

            s = CheckIncoherenceWithPlanet(item1, item2);
            if (s != null)
                return s;
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
        inspectedItems[0] = null;
        inspectedItems[1] = null;
        currentImmigrant = Instantiate(characterPrefab, parentTransform).GetComponent<Immigrant>();
        immigrantRandomizer.SetRandomData(currentImmigrant);
        currentImmigrant.audioSource.clip = immigrantRandomizer.GetCharacterData(currentImmigrant.photo).enterSound;
        currentImmigrant.audioSource.Play();
    }

    private void Start()
    {
        news.dateRule.CurrentDate = immigrantRandomizer.currentDate;

        //if (currentImmigrant == null)
        //{
        //    GenerateNewImmigrant();
        //}
    }

    bool lockButtons = false;

    public void AllowAccess()
    {
        if (lockButtons || currentImmigrant == null)
            return;

        if (currentImmigrant.isSpy)
        {
            CurrentScore -= 1;
        }
        else
            CurrentScore += 1;

        RaceData raceData = immigrantRandomizer.GetRaceData(currentImmigrant.race);
        currentImmigrant.anim.SetInteger("decision", 2);

        currentImmigrant.audioSource.clip = immigrantRandomizer.GetCharacterData(currentImmigrant.photo).enterSound;

        StartCoroutine(OnImmigrantLeave());
    }

    public void DenyAccess()
    {
        if (lockButtons || currentImmigrant == null)
            return;

        lasers.ActivateLasers();

        if (currentImmigrant.isSpy)
        {
            CurrentScore += 1;
        }
        else
        {
            CurrentScore -= 1;
        }

        RaceData raceData = immigrantRandomizer.GetRaceData(currentImmigrant.race);
        currentImmigrant.anim.SetInteger("decision", 1);

        currentImmigrant.audioSource.clip = immigrantRandomizer.GetCharacterData(currentImmigrant.photo).deniedSound;

        StartCoroutine(OnImmigrantLeave());
    }

    IEnumerator OnImmigrantLeave()
    {
        lockButtons = true;
        currentImmigrant.RemoveDocuments();
        currentImmigrant.audioSource.Play();

        yield return new WaitForSeconds(3);

        currentImmigrant.RemoveDocuments();
        Destroy(currentImmigrant.gameObject);

        yield return new WaitForSeconds(0.5f);

        lasers.DeactivateLasers();

        yield return new WaitForSeconds(0.5f);

        GenerateNewImmigrant();
        lockButtons = false;
    }
}
