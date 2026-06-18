using System.Collections.Generic;

// One customer's full order. A combo is simply an Order with multiple
// items in the list below - no separate Combo class needed.
[System.Serializable]
public class Order
{
    // Which customer this order belongs to (name, patience, tip modifier, etc.)
    // Customer class doesn't exist yet - this is a placeholder reference
    // for now and will be wired up once Customer is designed.
    // public Customer customer;

    // The dish(es) in this order. One item = a simple order.
    // Multiple items = a combo, handled by the exact same pipeline.
    public List<OrderItem> items = new List<OrderItem>();

    // Calculated once every item in the order is fully complete.
    public float overallScore = 0f;
    public bool isServed = false;

    public Order()
    {
        items = new List<OrderItem>();
    }

    // Convenience constructor for quickly building an order from a list of MenuItems
    // (useful for early testing before a real order-generation system exists)
    public Order(List<MenuItem> menuItems)
    {
        items = new List<OrderItem>();
        foreach (MenuItem mi in menuItems)
        {
            items.Add(new OrderItem(mi));
        }
    }

    // True once every OrderItem in this order has finished its full process pipeline.
    public bool IsComplete()
    {
        if (items.Count == 0) return false;

        foreach (OrderItem item in items)
        {
            if (!item.IsComplete()) return false;
        }
        return true;
    }

    // Aggregates each OrderItem's average score into one overall order score.
    // Call this once IsComplete() is true, right before serving.
    public float CalculateOverallScore()
    {
        if (items.Count == 0)
        {
            overallScore = 0f;
            return overallScore;
        }

        float total = 0f;
        foreach (OrderItem item in items)
        {
            total += item.GetAverageScore();
        }
        overallScore = total / items.Count;
        return overallScore;
    }

    // Returns all OrderItems currently needing a specific ProcessType.
    // A Station will call this to find out what work is waiting for it.
    public List<OrderItem> GetItemsNeedingProcess(ProcessType processType)
    {
        List<OrderItem> result = new List<OrderItem>();
        foreach (OrderItem item in items)
        {
            ProcessType? current = item.GetCurrentProcess();
            if (current.HasValue && current.Value == processType)
            {
                result.Add(item);
            }
        }
        return result;
    }
}
