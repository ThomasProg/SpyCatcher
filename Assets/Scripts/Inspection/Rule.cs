using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class Rule : InspectionItem
{
    public virtual string CheckIncoherence(InspectionItem item) { return null; }
}