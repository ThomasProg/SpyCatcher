using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RaceRule : Rule
{
    [SerializeField]
    string race;

    public override string CheckIncoherence(InspectionItem item)
    {
        switch (item.info.type)
        {
            case DataType.Race:
            {
                if (race == item.info.value)
                {
                    return "Anomalie détectée.";
                }
                else
                    return null;
            }
        }
        return null;
    }
}
