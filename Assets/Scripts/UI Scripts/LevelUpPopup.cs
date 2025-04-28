using UnityEngine;
using System;

public class LevelUpPopup : MonoBehaviour
{
    [SerializeField] private CanvasGroup popupGroup;
    [SerializeField] private CanvasGroup augmentGroup;
    GameManager gm;
    PlayerManager pm;

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
            popupGroup.alpha = 1f;
            popupGroup.interactable = true;
            popupGroup.blocksRaycasts = true;
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

    void Close()
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
