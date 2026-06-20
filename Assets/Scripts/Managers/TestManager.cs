using System.Collections.Generic;
using UnityEngine;

public class TestManager : MonoBehaviour
{
    [Header("Drag your three v1 MenuItem assets here")]
    public MenuItem burgerMenuItem;
    public MenuItem friesMenuItem;
    public MenuItem drinkMenuItem;

    [Header("Drag the GrillStation GameObject here")]
    public Station grillStation;

    [Header("Drag the OrderTicketUI here")]
    public OrderTicketUI orderTicketUI;

    private Order testOrder;

    void Start()
    {
        // Build a test order with all three items
        List<MenuItem> menuItems = new List<MenuItem> 
        { 
            burgerMenuItem, 
            friesMenuItem, 
            drinkMenuItem 
        };
        testOrder = new Order(menuItems);

        // Add some test customizations to the burger
        testOrder.items[0].requestedCustomizations.Add("No Onions");
        testOrder.items[0].requestedCustomizations.Add("Extra Cheese");

        Debug.Log($"Created test order with {testOrder.items.Count} items.");

        // Display the order on the ticket UI
        if (orderTicketUI != null)
        {
            orderTicketUI.DisplayOrder(testOrder);
        }
        else
        {
            Debug.LogWarning("OrderTicketUI not assigned in TestManager!");
        }

        // Try assigning items to the grill station
        foreach (OrderItem item in testOrder.items)
        {
            ProcessType? current = item.GetCurrentProcess();
            if (current.HasValue && grillStation.SupportsProcess(current.Value))
            {
                bool assigned = grillStation.TryAssignItem(item);
                Debug.Log($"Assigned {item.menuItem.itemName} ({current.Value}) to GrillStation: {assigned}");
            }
            else
            {
                Debug.Log($"{item.menuItem.itemName} needs {current} - not handled by GrillStation.");
            }
        }
    }
}