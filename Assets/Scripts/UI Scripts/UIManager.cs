using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class UIManager : MonoBehaviour
{
    [Header("Health UI")]
    [SerializeField] private Slider healthSlider;

    [Header("XP & Level UI")]
    [SerializeField] private Slider xpSlider;
    [SerializeField] private TextMeshProUGUI levelText;

    private PlayerManager player;
    private GameManager gm;

    void Awake()
    {
        player = FindObjectOfType<PlayerManager>();
        gm = FindObjectOfType<GameManager>();
    }

    void Start()
    {
        // initialize health bar
        healthSlider.maxValue = player.currentMaxHealth;
        healthSlider.value = player.currentHealth;

        // initialize xp bar & level
        xpSlider.maxValue = gm.nextLevelXP;
        xpSlider.value = gm.xp;
        if (levelText != null)
            levelText.text = $"Level: {gm.playerLevel}";
    }

    void OnEnable()
    {
        player.OnPlayerDamaged += HandlePlayerDamaged;
    }

    void OnDisable()
    {
        player.OnPlayerDamaged -= HandlePlayerDamaged;
    }

    private void HandlePlayerDamaged(float damageTaken, float cur, float max)
    {
        healthSlider.maxValue = max;
        healthSlider.value = cur;
    }

    void Update()
    {
        // refresh XP bar & level display each frame
        xpSlider.maxValue = gm.nextLevelXP;
        xpSlider.value = gm.xp;
        if (levelText != null)
            levelText.text = $"Lv {gm.playerLevel}";
    }
}
