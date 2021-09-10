using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateRule : Rule
{
    [SerializeField] Date currentDate;

    public Date CurrentDate
    {
        get
        {
            return currentDate;
        }
        set
        {
            currentDate = value;
            Text = DataConversions.ToString(currentDate);
        }
    }

    public override string CheckIncoherence(InspectionItem item)
    {
        switch (item.info.type)
        {
            case DataType.ExpirationDate:
            {
                Date expDate = DataConversions.StringToDate(item.info.value);
                if (expDate.IsLessThan(currentDate))
                {
                    return "La date d'expiration n'est plus valide.";
                }
                else
                    return null;
            }

            case DataType.DiplomaticLetterCreationDate:
                {
                    Date creationDate = DataConversions.StringToDate(item.info.value);
                    if (currentDate.IsLessThan(creationDate))
                    {
                        return "La date de création n'est pas valide.";
                    }
                    else
                        return null;
                }
        }
        return null;
    }
}