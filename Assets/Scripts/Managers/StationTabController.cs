using UnityEngine;

// Manages which station panel is currently visible.
// Attach this to the Canvas or a manager GameObject,
// then wire up the three panels and three buttons in the Inspector.
public class StationTabController : MonoBehaviour
{
    [Header("Station Panels")]
    public GameObject buildPanel;
    public GameObject grillPanel;
    public GameObject pourPanel;

    void Start()
    {
        // Default to Build station visible on game start
        ShowPanel(buildPanel);
    }

    public void ShowBuildStation()
    {
        ShowPanel(buildPanel);
    }

    public void ShowGrillStation()
    {
        ShowPanel(grillPanel);
    }

    public void ShowPourStation()
    {
        ShowPanel(pourPanel);
    }

    private void ShowPanel(GameObject panelToShow)
    {
        buildPanel.SetActive(false);
        grillPanel.SetActive(false);
        pourPanel.SetActive(false);
        panelToShow.SetActive(true);
    }
}