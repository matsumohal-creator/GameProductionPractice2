using UnityEngine;

public class UIManager : MonoBehaviour
{
    [SerializeField] SlidePanel optionPanel;

    public void ToggleOption()
    {
        optionPanel.Toggle();
    }
}