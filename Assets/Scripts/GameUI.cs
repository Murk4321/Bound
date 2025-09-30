using System;
using TMPro;
using UnityEngine;

public class GameUI : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI itemText;
    [SerializeField] private TextMeshProUGUI inputActionsText;

    private void Start()
    {
        itemText.text = string.Empty;
        inputActionsText.text = string.Empty;
    }

    public void ShowItemText(string text)
    {
        if (itemText.text == text) return;
        
        itemText.text = text;
    }

    public void ShowInputActionsText(string text)
    {
        if (inputActionsText.text == text) return;
        
        inputActionsText.text = text;
    }
}
