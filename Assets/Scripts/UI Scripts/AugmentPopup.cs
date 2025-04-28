using System;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.UI;

public class AugmentPopup : MonoBehaviour
{
    [SerializeField] private GameObject[] availableAugmentButtons;
    [SerializeField] private GameObject[] allAugmentButtons;
    private GameObject[] currentSelection = new GameObject[3];

    // Start is called once before the first execution of Update after the MonoBehaviour is created
    void Start()
    {
        availableAugmentButtons = (GameObject[])allAugmentButtons.Clone();
    }

    public void ShowAugments()
    {
        foreach (var button in allAugmentButtons)
        {
            button.SetActive(false);
        }

        if (availableAugmentButtons.Length < 3)
        {
            Debug.Log("Less than 3 augments");
        }
        
        for (int i = 0; i < 3; i++)
        {
            int randomIndex = UnityEngine.Random.Range(0, availableAugmentButtons.Length);
            currentSelection[i] = availableAugmentButtons[randomIndex];
            availableAugmentButtons = Array.FindAll(availableAugmentButtons, a => a != currentSelection[i]);
            currentSelection[i].SetActive(true);
        }
    }

    public void SelectAugment(GameObject chosenAugmentButton)
    {
        chosenAugmentButton.SetActive(false);
        RestoreUnusedAugments();
        availableAugmentButtons = Array.FindAll(availableAugmentButtons, a => a != chosenAugmentButton);
    }

    private void RestoreUnusedAugments()
    {
        if (currentSelection[0] != null)
        {
            foreach (var augment in currentSelection)
            {
                if (augment != null && augment.activeSelf)
                {
                    Array.Resize(ref availableAugmentButtons, availableAugmentButtons.Length + 1);
                    availableAugmentButtons[^1] = augment;
                }
            }
        }
    }
}
