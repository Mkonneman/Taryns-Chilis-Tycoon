using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

// Drives the visual Grill station UI.
// Attach this to StationPanel_Grill.
// Wire up the four GrillSlot UI references in the Inspector.
public class GrillStationUI : MonoBehaviour
{
    [System.Serializable]
    public class GrillSlotUI
    {
        public TextMeshProUGUI itemNameText;
        public Slider timerBar;
        public Button pullButton;
        public Image slotBackground;

        [HideInInspector] public StationSlot linkedSlot;
        [HideInInspector] public float targetTime; // perfect pull time in seconds
        [HideInInspector] public float maxTime;    // burn time in seconds
    }

    [Header("Grill Station Data")]
    public Station grillStation;

    [Header("Slot UI References (match order to GrillSlot_1 through 4)")]
    public List<GrillSlotUI> slotUIs = new List<GrillSlotUI>();

    [Header("Timing Settings")]
    public float perfectCookTime = 5f;   // seconds to cook perfectly
    public float burnTime = 8f;          // seconds until burnt

    [Header("Colors")]
    public Color emptyColor = new Color(0.7f, 0.5f, 0.5f, 1f);
    public Color cookingColor = new Color(1f, 0.8f, 0.3f, 1f);
    public Color perfectColor = new Color(0.3f, 1f, 0.3f, 1f);
    public Color burntColor = new Color(1f, 0.2f, 0.2f, 1f);

    void Start()
    {
        // Initialize all slots
        List<StationSlot> slots = grillStation.GetAllSlots();
        for (int i = 0; i < slotUIs.Count; i++)
        {
            int index = i; // capture for lambda
            slotUIs[i].linkedSlot = slots[i];
            slotUIs[i].targetTime = perfectCookTime;
            slotUIs[i].maxTime = burnTime;

            // Wire up the pull button click
            slotUIs[i].pullButton.onClick.AddListener(() => OnPullClicked(index));

            // Initialize visual state
            RefreshSlotUI(slotUIs[i]);
        }
    }

    void Update()
    {
        foreach (GrillSlotUI slotUI in slotUIs)
        {
            if (!slotUI.linkedSlot.IsEmpty())
            {
                RefreshSlotUI(slotUI);
            }
        }
    }

    private void RefreshSlotUI(GrillSlotUI slotUI)
    {
        StationSlot slot = slotUI.linkedSlot;

        if (slot.IsEmpty())
        {
            slotUI.itemNameText.text = "Empty";
            slotUI.timerBar.value = 0f;
            if (slotUI.slotBackground != null)
                slotUI.slotBackground.color = emptyColor;
            return;
        }

        // Update item name
        slotUI.itemNameText.text = slot.occupant.menuItem.itemName;

        // Update timer bar (0 to 1 based on elapsed vs burn time)
        float progress = Mathf.Clamp01(slot.elapsedTime / slotUI.maxTime);
        slotUI.timerBar.value = progress;

        // Update color based on cooking stage
        if (slot.elapsedTime < slotUI.targetTime)
        {
            // Still cooking
            if (slotUI.slotBackground != null)
                slotUI.slotBackground.color = cookingColor;
        }
        else if (slot.elapsedTime < slotUI.maxTime)
        {
            // In the perfect window
            if (slotUI.slotBackground != null)
                slotUI.slotBackground.color = perfectColor;
        }
        else
        {
            // Burnt
            if (slotUI.slotBackground != null)
                slotUI.slotBackground.color = burntColor;
        }
    }

    private void OnPullClicked(int slotIndex)
    {
        GrillSlotUI slotUI = slotUIs[slotIndex];
        StationSlot slot = slotUI.linkedSlot;

        if (slot.IsEmpty())
        {
            Debug.Log($"Slot {slotIndex + 1} is empty - nothing to pull.");
            return;
        }

        // Calculate score based on timing
        float elapsed = slot.elapsedTime;
        float score = 0f;

        if (elapsed < slotUI.targetTime)
        {
            // Undercooked - partial score based on how close to perfect
            score = elapsed / slotUI.targetTime * 0.5f;
        }
        else if (elapsed < slotUI.maxTime)
        {
            // Perfect window - full score
            score = 1f;
        }
        else
        {
            // Burnt - score drops off after burn time
            float overTime = elapsed - slotUI.maxTime;
            score = Mathf.Max(0f, 1f - overTime);
        }

        string itemName = slot.occupant.menuItem.itemName;
        grillStation.CompleteSlot(slot, score);
        RefreshSlotUI(slotUI);

        Debug.Log($"Pulled {itemName} from slot {slotIndex + 1} - Score: {score:F2}");
    }

    // Call this to place an OrderItem into the first available slot.
    public bool TryAddItem(OrderItem item)
    {
        return grillStation.TryAssignItem(item);
    }
}