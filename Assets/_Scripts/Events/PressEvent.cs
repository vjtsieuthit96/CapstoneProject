using System;
using UnityEngine;

public class PressEvent
{
    public event Action<PanelType> onOpitonButtonPress; // Event to notify when a panel is pressed
    public void OnOptionButtonPress(PanelType panelType)
    {
        onOpitonButtonPress?.Invoke(panelType); // Invoke the event if there are any subscribers
    }
}
