using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.Events;

public class GameManager : MonoBehaviour
{
    public Text displayText;
    public Text crewmateListText;
    public int maxCrewmates = 10;
    public ReturnToMenu returnToMainMenu;
    public UnityEvent onCrewComplete;

    private List<Crewmate> hiredCrewmates = new List<Crewmate>();
    private List<Crewmate> crewmates = new List<Crewmate>();
    private List<string> availableHobbies = new List<string>
    {
        "Reading",
        "Cooking",
        "Gardening",
        "Painting",
        "Gaming",
    };

    private List<string> alienHobbies = new List<string>
    {
        "Technology",
        "Astronomy",
        "Astrology",
        "Learning",
        "Hunting",
    };

    private bool gameIsOver = false;

    private void Start()
    {
        NextCrewmate();
        crewmateListText.text = GetCrewmateListText();
    }

    private void UpdateCrewmateInfoText()
    {
        if (crewmates.Count > 0)
        {
            Crewmate currentCrewmate = crewmates[crewmates.Count - 1];
            displayText.text = "Crewmate Applicant: " + currentCrewmate.GetNameAndHobby();
        }
        else
        {
            displayText.text = ""; // Clear the text to show no message when there are no more crewmate W
        }

        // Create the crewmate list string
        string crewmateList = "Crewmates on the ship:\n";
        foreach (Crewmate crewmate in hiredCrewmates)
        {
            crewmateList += crewmate.GetNameAndHobby() + "\n";
        }

        if (crewmates.Count >= maxCrewmates)
        {
            if (onCrewComplete != null)
            {
                onCrewComplete.Invoke(); // Trigger the event when the crew is complete
            }

            crewmateListText.text = crewmateList; // Set the crewmateList text content
        }
    }

    private void NextCrewmate()
    {
        if (hiredCrewmates.Count < maxCrewmates)
        {
            Crewmate newCrewmate = GenerateRandomCrewmate();
            crewmates.Add(newCrewmate);
            displayText.text = "Crewmate Applicant: " + newCrewmate.GetNameAndHobby();
            UpdateCrewmateInfoText();
        }
        else
        {
            EndGame(); // Call the EndGame function if the maximum crewmate limit is reached
        }
    }

    private Crewmate GenerateRandomCrewmate()
    {
        string name = GenerateRandomName();
        string hobby = "";
        bool isAlien = Random.Range(0, 2) == 1;

        if (isAlien)
        {
            hobby = alienHobbies[Random.Range(0, alienHobbies.Count)];
        }
        else
        {
            hobby = availableHobbies[Random.Range(0, availableHobbies.Count)];
        }

        return new Crewmate(name, hobby, isAlien);
    }

    private string GenerateRandomName()
    {
        string[] names = { "Alice", "Bob", "Charlie", "David", "Eve", "Frank", "Anna", "Felix", "Ada", "Gregory", "Charlotte", "Skyler", "Rory", "Xavier", "Yuri", "Kai", "Riley", "Ronald", "Camille", "Trent", "Raven", "Matt" };
        return names[Random.Range(0, names.Length)];
    }

    public void HireCrewmate()
    {
        if (!gameIsOver && crewmates.Count > 0)
        {
            Crewmate currentCrewmate = crewmates[crewmates.Count - 1];

            if (currentCrewmate.IsAlien())
            {
                displayText.text = "You hired: " + currentCrewmate.GetNameAndHobby();
                StartCoroutine(DisplayKilledMessageAndKillCrewmates(currentCrewmate));
            }
            else
            {
                displayText.text = "You hired: " + currentCrewmate.GetNameAndHobby() + "\nWelcome aboard!";
                StartCoroutine(HireCrewmateAfterDelay(currentCrewmate));
            }
        }
    }

    public void RefuseCrewmate()
    {
        if (crewmates.Count > 0)
        {
            Crewmate currentCrewmate = crewmates[crewmates.Count - 1];
            displayText.text = "You refused: " + currentCrewmate.GetNameAndHobby();
            crewmates.Remove(currentCrewmate);
            NextCrewmate();
        }
    }

    private List<Crewmate> FindCrewmatesWithHobby(string hobby)
    {
        List<Crewmate> crewmatesWithHobby = new List<Crewmate>();

        foreach (Crewmate crewmate in hiredCrewmates)
        {
            if (!crewmate.IsAlien() && crewmate.GetHobby() == hobby)
            {
                crewmatesWithHobby.Add(crewmate);
            }
        }

        return crewmatesWithHobby;
    }

    private IEnumerator DisplayNextCrewmateAfterDelay()
    {
        yield return new WaitForSeconds(1f);
        NextCrewmate();
    }

    private IEnumerator HireCrewmateAfterDelay(Crewmate crewmate)
    {
        yield return new WaitForSeconds(1f);
        if (displayText != null)
        {
            displayText.text = ""; // Clear the text to avoid any issues
        }

        crewmates.Remove(crewmate);
        hiredCrewmates.Add(crewmate); // Add the hired crewmate to the list of hired crewmates

        // Update the crewmate list text after hiring is complete
        crewmateListText.text = GetCrewmateListText();

        // Move to the next crewmate after the delay
        NextCrewmate();
    }

    private IEnumerator DisplayKilledMessageAndKillCrewmates(Crewmate alienCrewmate)
    {
        string alienHobby = alienCrewmate.GetHobby();
        displayText.text = "You hired: " + alienCrewmate.GetNameAndHobby() + "\nThey killed some crewmates and escaped off the ship!";
        yield return new WaitForSeconds(1f);

        KillCrewmatesWithSameHobby(alienCrewmate);

        // Update the crewmate info after killing crewmates
        UpdateCrewmateInfoText();

        yield return new WaitForSeconds(1f);
        NextCrewmate();
    }

    private void KillCrewmatesWithSameHobby(Crewmate alienCrewmate)
    {
        if (!alienCrewmate.IsAlien())
        {
            // Alien crewmate must be an actual alien, so nothing to kill
            return;
        }

        // Alien 'escapes'
        crewmates.Remove(alienCrewmate);
        hiredCrewmates.Remove(alienCrewmate);

        crewmateListText.text = GetCrewmateListText(); // Update the crewmate list text

        // Choose a random hobby from the availableHobbies list
        string randomHobby = availableHobbies[Random.Range(0, availableHobbies.Count)];

        // Find and eliminate all crewmates with the chosen hobby
        List<Crewmate> crewmatesToKill = FindCrewmatesWithHobby(randomHobby);
        foreach (Crewmate crewmate in crewmatesToKill)
        {
            crewmates.Remove(crewmate);
            hiredCrewmates.Remove(crewmate);
        }

        crewmateListText.text = GetCrewmateListText();
        UpdateCrewmateInfoText(); // Update the crewmate info after killing crewmates
    }

    public void EndGame()
    {
        gameIsOver = true;
        displayText.text = "Congratulations! Your crew is complete!\n\n";

        // Call a function from the ReturnToMainMenu script to disable/hide the buttons
        if (returnToMainMenu != null)
        {
            returnToMainMenu.DisableButtons();
        }
    }

    private string GetCrewmateListText()
    {
        // Create the crewmate list string
        string crewmateList = "Crewmates on the ship:\n";
        foreach (Crewmate crewmate in hiredCrewmates)
        {
            crewmateList += crewmate.GetNameAndHobby() + "\n";
        }
        return crewmateList;
    }
}
