using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

// Reads from an Order object and populates the ticket panel UI.
// Attach this to the OrderTicketPanel GameObject.
public class OrderTicketUI : MonoBehaviour
{
    [Header("UI References")]
    public TextMeshProUGUI customerNameText;
    public Transform orderItemsContainer; // The Content object inside the ScrollView

    [Header("Prefab")]
    public GameObject orderItemEntryPrefab; // A prefab with a TMP text component

    private List<GameObject> spawnedEntries = new List<GameObject>();

    // Call this whenever a new Order needs to be displayed on the ticket.
    public void DisplayOrder(Order order)
    {
        // Clear any previously displayed entries
        ClearTicket();

        if (order == null)
        {
            customerNameText.text = "No Order";
            return;
        }

        // Placeholder customer name until Customer class exists
        customerNameText.text = "Customer";

        // Spawn one entry per OrderItem
        foreach (OrderItem item in order.items)
        {
            GameObject entry = Instantiate(orderItemEntryPrefab, orderItemsContainer);
            TextMeshProUGUI entryText = entry.GetComponent<TextMeshProUGUI>();

            if (entryText != null)
            {
                string itemText = item.menuItem.itemName;

                // Add customizations below the item name
                if (item.requestedCustomizations.Count > 0)
                {
                    foreach (string customization in item.requestedCustomizations)
                    {
                        itemText += $"\n  - {customization}";
                    }
                }

                // Show current process step
                ProcessType? currentProcess = item.GetCurrentProcess();
                if (currentProcess.HasValue)
                {
                    itemText += $"\n  [{currentProcess.Value}]";
                }
                else
                {
                    itemText += "\n  [Done]";
                }

                entryText.text = itemText;
            }

            spawnedEntries.Add(entry);
        }
    }

    // Clears all spawned item entries from the ticket.
    public void ClearTicket()
    {
        foreach (GameObject entry in spawnedEntries)
        {
            Destroy(entry);
        }
        spawnedEntries.Clear();
    }
}