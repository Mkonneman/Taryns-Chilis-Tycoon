using System.Collections.Generic;

// A runtime instance of a MenuItem within one specific Order.
// Not a ScriptableObject - this is created fresh each time a customer
// orders something, and holds the actual chosen customizations plus
// the grading data collected as it passes through each cooking step.
[System.Serializable]
public class OrderItem
{
    // Which dish this is (reference to the MenuItem asset, e.g. Big Smasher Burger)
    public MenuItem menuItem;

    // The specific customizations THIS customer requested for THIS order
    // (e.g. this customer wants "No Onions" but not "Extra Cheese")
    public List<string> requestedCustomizations = new List<string>();

    // Tracks which step in menuItem.requiredProcesses we're currently on.
    // Starts at 0, increments each time a station finishes grading this item.
    public int currentProcessIndex = 0;

    // Per-process accuracy scores, one entry added per completed step.
    // e.g. BuildOnly might score 0.9, GrillSear might score 1.0, etc.
    public List<float> processScores = new List<float>();

    public OrderItem(MenuItem item)
    {
        menuItem = item;
    }

    // True once every required process for this item has been completed.
    public bool IsComplete()
    {
        return menuItem != null && currentProcessIndex >= menuItem.requiredProcesses.Count;
    }

    // The ProcessType this item currently needs (null/none if already complete).
    public ProcessType? GetCurrentProcess()
    {
        if (IsComplete()) return null;
        return menuItem.requiredProcesses[currentProcessIndex];
    }

    // Call this when a station finishes grading this item's current step.
    public void CompleteCurrentProcess(float score)
    {
        processScores.Add(score);
        currentProcessIndex++;
    }

    // Average of all recorded process scores - this item's overall accuracy.
    public float GetAverageScore()
    {
        if (processScores.Count == 0) return 0f;
        float total = 0f;
        foreach (float s in processScores) total += s;
        return total / processScores.Count;
    }
}