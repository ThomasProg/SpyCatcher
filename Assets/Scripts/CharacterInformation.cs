using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public struct Date
{
    public int day;
    public int month;
    public int year;
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

    public static T FromString<T>(string str)
    {
        return (T) System.Enum.Parse(typeof(T), str, true); ;
    }
};