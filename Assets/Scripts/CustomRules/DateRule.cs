using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DateRule : Rule
{
    public Date currentDate;

    public override string CheckIncoherence(InspectionItem item)
    {
        if (item.info.type == DataType.ExpirationDate)
        {
            Date expDate = DataConversions.StringToDate(item.info.value);
            if (expDate.IsLessThan(currentDate))
            {
                return "La date d'expiration n'est plus valide.";
            }
            else
                return null;
        }
        return null;
    }
}