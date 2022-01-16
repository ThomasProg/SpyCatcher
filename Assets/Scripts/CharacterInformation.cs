using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Date
{
    public int day;
    public int month;
    public int year;

    public void RemoveYears(int nbYears)
    {
        year -= nbYears;
    }

    public void RemoveMonths(int nbMonths)
    {
        int nbMaxMonths = 12;
        int nbYears = nbMonths / nbMaxMonths;
        month = month - nbMonths % nbMaxMonths;
        if (month <= 0)
        {
            month += nbMaxMonths;
            nbYears++;
        }
        RemoveYears(nbYears);
    }

    public void RemoveDays(int nbDays)
    {
        int nbMaxDays = 28;
        int nbMonths = nbDays / nbMaxDays;
        day = day -  nbDays % nbMaxDays;
        if (day <= 0)
        {
            day += nbMaxDays;
            nbMonths++;
        }
        RemoveMonths(nbMonths);

    }

    public bool IsLessThan(Date another)
    {
        if (year < another.year)
            return true;
        if (year > another.year)
            return false;

        if (month < another.month)
            return true;
        if (month > another.month)
            return false;

        if (day < another.day)
            return true;
        //if (day > another.day)
        return false;
    }

    static public Date GetRandomDate(Date min, Date max)
    {
        Date randDate;
        randDate.year = Random.Range(min.year, max.year + 1);

        int minMonth = 1;
        if (randDate.year == min.year)
            minMonth = min.month;
        int maxMonth = 12;
        if (randDate.year == max.year)
            maxMonth = max.month;
        randDate.month = Random.Range(minMonth, maxMonth);

        int minDay = 1;
        if (randDate.year == min.year && randDate.month == min.month)
            minDay = min.day;
        int maxDay = 28;
        if (randDate.year == max.year && randDate.month == max.month)
            maxDay = max.day;
        randDate.day = Random.Range(minDay, maxDay);

        return randDate;
    }
};

[System.Serializable]
public enum Planet
{
    Kyla,
    Lorn,
    Juvenus,
    Bzzz,
    K52
};

//[System.Serializable]
//public enum Race
//{ 
//    Human,
//    Antropomorph,
//    Slime,
//    Insect,
//    Invisible
//}

[System.Serializable]
public enum City
{
    Ville1,
    Vill2,
    Ville3,
    Ville4,
    Ville5,
    Ville6,
    Ville7,
    Ville8,
    Ville9,
    Ville10,
    Ville11,
    Ville12,
    Nyonis,
    Shrouxe,

    Nyx,
    Slip,
    Kroa,
    Vioni,
    Muna,
    Nobboria,
    Raugawa,
    Chahines,
    Ziawi,
    Venarvis,
    Ulluela,

    Papapapa,
    Krrkrr,
    Toubos,
    Zaturn,
    Brawlala,

    K50
}

public class DataConversions
{
    public static string ToString<T>(T p)
    {
        return p.ToString();
    }

    public static string ToString(Planet p)
    {
        return p.ToString();
    }

    public static string ToString(Date date)
    {
        return date.day + " / " + date.month + " / " + date.year;
    }


    public static T FromString<T>(string str)
    {
        return (T)System.Enum.Parse(typeof(T), str, true);
    }

    public static int FromString(string str)
    {
        return int.Parse(str);
    }


    public static Date StringToDate(string str)
    {
        string[] s = str.Split('/');
        Date newDate;
        newDate.day = int.Parse(s[0]);
        newDate.month = int.Parse(s[1]);
        newDate.year = int.Parse(s[2]);
        return newDate;
    }

    public static Sprite ToSprite(Planet p)
    {
        InspectionManager inspectionManager = GameObject.Find("InspectionManager").GetComponent<InspectionManager>();
        return inspectionManager.immigrantRandomizer.GetPlanetData(p).icon;
    }

    public static Sprite PlanetToSeal(Planet p)
    {
        InspectionManager inspectionManager = GameObject.Find("InspectionManager").GetComponent<InspectionManager>();
        return inspectionManager.immigrantRandomizer.GetPlanetData(p).seal;
    }
};