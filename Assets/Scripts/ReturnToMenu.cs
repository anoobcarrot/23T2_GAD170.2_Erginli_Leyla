using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.Events;

public class ReturnToMenu : MonoBehaviour
{
    public Button hireButton;
    public Button refuseButton;
    public Button returnToMainMenuButton;

    private GameManager gameManager;

    private void Start()
    {
        // Get a reference to the GameManager in the scene
        gameManager = FindObjectOfType<GameManager>();

        // Add listener to the onCrewComplete event of the GameManager
        if (gameManager != null)
        {
            gameManager.onCrewComplete.AddListener(OnCrewComplete);
        }

        if (returnToMainMenuButton != null)
        {
            returnToMainMenuButton.onClick.AddListener(ReturnToMainMenu);
        }
    }

    public void DisableButtons()
    {
        // Disable or hide the buttons as needed
        if (hireButton != null)
        {
            hireButton.interactable = false; // Disable the "Hire" button
        }

        if (refuseButton != null)
        {
            refuseButton.interactable = false; // Disable the "Refuse" button
        }
    }

private void OnCrewComplete()
    {
        // Hide the hire and refuse buttons
        hireButton.gameObject.SetActive(false);
        refuseButton.gameObject.SetActive(false);

        // Show the "Return to Main Menu" button
        returnToMainMenuButton.gameObject.SetActive(true);
    }

    public void ReturnToMainMenu()
    {
        // Load Menu scene
        UnityEngine.SceneManagement.SceneManager.LoadScene("MenuUI");
    }
}
