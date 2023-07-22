using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Crewmate : MonoBehaviour
{
    private string name;
    private string hobby;
    private bool isAlien;

    public Crewmate(string name, string hobby, bool isAlien)
    {
        this.name = name;
        this.hobby = hobby;
        this.isAlien = isAlien;
    }

    public string GetNameAndHobby()
    {
        return name + " (" + hobby + ")";
    }

    public string GetHobby()
    {
        return hobby;
    }

    public bool IsAlien()
    {
        return isAlien;
    }

    public void RemoveHobby(string hobbyToRemove)
    {
        if (hobby == hobbyToRemove)
        {
            hobby = ""; // Set hobby to an empty string to remove
        }
    }

    public bool HasHobby(string hobbyToCheck)
    {
        return hobby == hobbyToCheck;
    }
}
