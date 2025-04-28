using UnityEngine;
using System;

public class LevelUpPopup : MonoBehaviour
{
    [SerializeField] private CanvasGroup popupGroup;
    [SerializeField] private CanvasGroup augmentGroup;
    GameManager gm;
    PlayerManager pm;

    [SerializeField] private GameObject[] allUpgradeButtons;
    private GameObject[] currentSelection = new GameObject[3];
    [SerializeField] private GameObject[] availableUpgradeButtons;

    void Awake()
    {
        // make sure the panel is invisible, but the GO stays active
        popupGroup.alpha = 0f;
        popupGroup.interactable = false;
        popupGroup.blocksRaycasts = false;
    }

    void Start()
    {
        // now the GameObject is definitely active, subscribe here
        gm = GameManager.Instance;
        pm = GameObject.FindGameObjectWithTag("Player").GetComponent<PlayerManager>();
        availableUpgradeButtons = (GameObject[])allUpgradeButtons.Clone();

        if (gm != null)
        {
            gm.OnLeveledUp += ShowPopup;
            Debug.Log("LevelUpPopup: Subscribed to OnLeveledUp");
        }
        else Debug.LogError("LevelUpPopup: GameManager.Instance was null in Start()");
    }

    void OnDestroy()
    {
        if (gm != null)
        {
            gm.OnLeveledUp -= ShowPopup;
            Debug.Log("LevelUpPopup: Unsubscribed from OnLeveledUp");
        }
    }

    void ShowPopup(int newLevel)
    {
        // Every 5 levels shows augment menu
        if (newLevel % 5 == 0)
        {
            Debug.Log($"LevelUpPopup: ShowPopup fired for level {newLevel}");
            Time.timeScale = 0f;
            GetComponent<AugmentPopup>().ShowAugments();
            augmentGroup.alpha = 1f;
            augmentGroup.interactable = true;
            augmentGroup.blocksRaycasts = true;
        }
        else
        {
            Debug.Log($"LevelUpPopup: ShowPopup fired for level {newLevel}");
            Time.timeScale = 0f;
            ShowUpgrades();
            popupGroup.alpha = 1f;
            popupGroup.interactable = true;
            popupGroup.blocksRaycasts = true;
        }
    }

    public void ShowUpgrades()
    {
        // Deactivates all buttons
        foreach (var button in allUpgradeButtons)
        {
            button.SetActive(false);
        }

        if (allUpgradeButtons.Length < 3)
        {
            Debug.Log("Less than 3 upgrades");
            availableUpgradeButtons = (GameObject[])allUpgradeButtons.Clone();
        }

        // Chooses 3 random augments from available augments to display
        for (int i = 0; i < availableUpgradeButtons.Length; i++)
        {
            int swapIndex = UnityEngine.Random.Range(i, availableUpgradeButtons.Length);
            GameObject temp = availableUpgradeButtons[i];
            availableUpgradeButtons[i] = availableUpgradeButtons[swapIndex];
            availableUpgradeButtons[swapIndex] = temp;
        }

        for (int i = 0; i < 3; i++)
        {
            currentSelection[i] = availableUpgradeButtons[i];
            currentSelection[i].SetActive(true);
        }
    }

    public void OnHealthUpgrade()
    {

        pm.MultiplyMaxHealth(1.2f);
        Close();

    }
    public void OnDamageUpgrade()
    {

        pm.SetDamageMultiplier(1.1f);
        pm.SetAttackDamage(pm.damageMultiplier, pm.baseAttackDamage);
        Close();

    }
    public void OnSpeedUpgrade()
    {
        var cc = pm.gameObject.GetComponent<PlayerMovement>();
        if (cc != null) cc.speed *= 1.2f;
        Close();
    }

    public void Close()
    {
        popupGroup.alpha = 0f;
        popupGroup.interactable = false;
        popupGroup.blocksRaycasts = false;
        Time.timeScale = 1f;
    }

    // Called on button click
    public void CloseAugmentPopup()
    {
        augmentGroup.alpha = 0f;
        augmentGroup.interactable = false;
        augmentGroup.blocksRaycasts = false;
        Time.timeScale = 1f;
    }
}
