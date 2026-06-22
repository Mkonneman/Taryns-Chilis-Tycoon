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

    [Header("Drag the GrillStationUI here")]
    public GrillStationUI grillStationUI;

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

        // Display the order on the ticket
        if (orderTicketUI != null)
            orderTicketUI.DisplayOrder(testOrder);

        // Try putting the fries into the grill station
        // (fries need FryTimer which GrillStation supports)
        OrderItem friesItem = testOrder.items[1];
        if (grillStationUI != null)
        {
            bool assigned = grillStationUI.TryAddItem(friesItem);
            Debug.Log($"Assigned fries to grill station: {assigned}");
        }
    }
}