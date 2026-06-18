// One individual processing slot within a Station.
// A Station can have multiple slots running independently and simultaneously
// (e.g. the Grill station might have 4 slots - 2 running GrillSear for patties,
// 2 running FryTimer for Crispers - all ticking their own timers at once).
[System.Serializable]
public class StationSlot
{
    // The OrderItem currently occupying this slot (null if the slot is empty/idle)
    public OrderItem occupant;

    // Which process this slot is currently running for its occupant
    // (must be one of the Station's supportedProcesses)
    public ProcessType? activeProcess;

    // How long the current process has been running, in seconds.
    // Stations/process-specific logic will compare this against
    // a target time to determine doneness/accuracy.
    public float elapsedTime = 0f;

    public bool IsEmpty()
    {
        return occupant == null;
    }

    // Places an OrderItem into this slot and starts timing its current process.
    public void Assign(OrderItem item)
    {
        occupant = item;
        activeProcess = item.GetCurrentProcess();
        elapsedTime = 0f;
    }

    // Call every frame (or on a fixed tick) while this slot is occupied.
    public void Tick(float deltaTime)
    {
        if (!IsEmpty())
        {
            elapsedTime += deltaTime;
        }
    }

    // Clears the slot, making it available for a new OrderItem.
    public void Clear()
    {
        occupant = null;
        activeProcess = null;
        elapsedTime = 0f;
    }
}