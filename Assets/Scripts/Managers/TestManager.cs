using System.Collections.Generic;
using UnityEngine;
 
// Temporary test script to prove the Order/Station pipeline works end to end.
// Drag the three v1 MenuItem assets (burger, fries, drink) into the Inspector
// slots below, drag the GrillStation into the grillStation slot, hit Play,
// and watch the Console.
public class TestManager : MonoBehaviour
{
    [Header("Drag your three v1 MenuItem assets here")]
    public MenuItem burgerMenuItem;
    public MenuItem friesMenuItem;
    public MenuItem drinkMenuItem;
 
    [Header("Drag the GrillStation GameObject here")]
    public Station grillStation;
 
    private Order testOrder;
 
    void Start()
    {
        // Build a test order containing all three items - this is a "combo"
        // simply by virtue of having more than one OrderItem in the list.
        List<MenuItem> menuItems = new List<MenuItem> { burgerMenuItem, friesMenuItem, drinkMenuItem };
        testOrder = new Order(menuItems);
 
        Debug.Log($"Created test order with {testOrder.items.Count} items.");
 
        foreach (OrderItem item in testOrder.items)
        {
            ProcessType? current = item.GetCurrentProcess();
            Debug.Log($"{item.menuItem.itemName} currently needs: {current}");
        }
 
        // Try assigning every item that currently needs a process the
        // Grill station supports (GrillSear or FryTimer).
        foreach (OrderItem item in testOrder.items)
        {
            ProcessType? current = item.GetCurrentProcess();
            if (current.HasValue && grillStation.SupportsProcess(current.Value))
            {
                bool assigned = grillStation.TryAssignItem(item);
                Debug.Log($"Tried assigning {item.menuItem.itemName} ({current.Value}) to GrillStation: {assigned}");
            }
            else
            {
                Debug.Log($"{item.menuItem.itemName} needs {current} which GrillStation does not support - skipping for this test.");
            }
        }
    }
}