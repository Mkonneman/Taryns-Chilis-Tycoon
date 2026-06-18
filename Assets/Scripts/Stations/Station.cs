using System.Collections.Generic;
using UnityEngine;

// A piece of equipment, not a single-purpose tool.
// Has multiple slots that can each independently run any of this
// station's supported process types at the same time.
// Example: the Grill station supports both GrillSear (burger patties)
// and FryTimer (Crispers/fries) across separate slots simultaneously.
public class Station : MonoBehaviour
{
    [Tooltip("Which cooking-step types this station is capable of running")]
    public List<ProcessType> supportedProcesses = new List<ProcessType>();

    [Tooltip("How many slots this station has available")]
    public int slotCount = 4;

    // Runtime list of slots, built from slotCount when the station initializes.
    private List<StationSlot> slots = new List<StationSlot>();

    void Awake()
    {
        InitializeSlots();
    }

    private void InitializeSlots()
    {
        slots = new List<StationSlot>();
        for (int i = 0; i < slotCount; i++)
        {
            slots.Add(new StationSlot());
        }
    }

    void Update()
    {
        // Advance the timer on every occupied slot each frame.
        foreach (StationSlot slot in slots)
        {
            slot.Tick(Time.deltaTime);
        }
    }

    // True if this station is able to run the given process type at all
    // (regardless of whether a slot is currently free).
    public bool SupportsProcess(ProcessType processType)
    {
        return supportedProcesses.Contains(processType);
    }

    // Finds the first open (empty) slot, or null if the station is full.
    public StationSlot GetOpenSlot()
    {
        foreach (StationSlot slot in slots)
        {
            if (slot.IsEmpty()) return slot;
        }
        return null;
    }

    // Attempts to place an OrderItem into an open slot, if this station
    // supports the process that item currently needs.
    // Returns true if successfully assigned, false otherwise.
    public bool TryAssignItem(OrderItem item)
    {
        ProcessType? currentProcess = item.GetCurrentProcess();
        if (!currentProcess.HasValue) return false;
        if (!SupportsProcess(currentProcess.Value)) return false;

        StationSlot openSlot = GetOpenSlot();
        if (openSlot == null) return false;

        openSlot.Assign(item);
        return true;
    }

    // Call this once a slot's processing is judged complete (e.g. player pulled
    // the item off the grill, or a timer-based auto-completion fired).
    // score should be 0-1 representing how accurate/well-timed the result was.
    public void CompleteSlot(StationSlot slot, float score)
    {
        if (slot.IsEmpty()) return;

        slot.occupant.CompleteCurrentProcess(score);
        slot.Clear();
    }

    public List<StationSlot> GetAllSlots()
    {
        return slots;
    }
}