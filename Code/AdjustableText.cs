using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class AdjustableText : MonoBehaviour, IAdjustableText
{
    private TextMeshProUGUI Text;

    [SerializeField]
    private string iName;

    void Awake()
    {
        Text = GetComponent<TextMeshProUGUI>();    
    }

    public void SetText(string text)
    {
        Text.text = text;
    }

    public string GetName()
    {
        return iName;
    }
}