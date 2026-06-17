using System.Collections.Generic;
using UnityEngine;

// The distinct cooking-step types a dish can require.
// This determines which Station an item visits, not its menu category.
public enum ProcessType
{
    BuildOnly,      // layering/placement, no timer (e.g. assembling a bun + raw patty)
    GrillSear,      // timed doneness - burger patties, steaks
    FryTimer,       // drop-in, pull-at-right-moment - Crispers, fries, wings
    BakeTimer,      // oven/skillet timed bake - desserts
    PourGarnish,    // liquid level + garnish placement - drinks
    PlateAssembly   // automatic step - no player interaction, just visual plating
}

// One customization option a player can add/remove on an item
// (e.g. "Extra Cheese", "No Onions", "Well Done")
[System.Serializable]
public class CustomizationOption
{
    public string optionName;
    public bool isDefault; // true if this is included unless the customer says otherwise
}

// Defines what a dish IS, independent of any specific order.
// One MenuItem asset per dish (e.g. "Big Smasher Burger", "Crispers", "Lemonade").
[CreateAssetMenu(fileName = "NewMenuItem", menuName = "ChilisTycoon/MenuItem")]
public class MenuItem : ScriptableObject
{
    [Header("Basic Info")]
    public string itemName;
    public Sprite itemIcon;

    [Header("Category")]
    public MenuCategory category;

    [Header("Pricing")]
    public float basePrice;

    [Header("Cooking Pipeline")]
    [Tooltip("Ordered list of cooking steps this item must pass through, e.g. BuildOnly -> GrillSear -> BuildOnly -> PlateAssembly")]
    public List<ProcessType> requiredProcesses = new List<ProcessType>();

    [Header("Customization")]
    [Tooltip("Toppings/options this item supports customers requesting changes to")]
    public List<CustomizationOption> customizationOptions = new List<CustomizationOption>();
}

// Broad category groupings, matching the Chili's menu reference doc.
// Used for organization/filtering, NOT for determining cooking steps
// (that's what requiredProcesses is for).
public enum MenuCategory
{
    Burger,
    Sandwich,
    FriedSide,
    PlatedEntree,
    Drink,
    Dessert
}
