using System;
using UnityEngine;
using TMPro;

[RequireComponent(typeof(TMP_Text))]
public class TMPTextFiller : MonoBehaviour
{
    private TMP_Text textComp;
    
    [SerializeField, Tooltip("Set what the text should start with.")]
    protected string startSentence = String.Empty;
    
    protected virtual void Awake()
    {
        textComp = GetComponent<TMP_Text>();
    }

    protected void SetText(string pText)
    {
        startSentence = startSentence.Trim();
        
        if (textComp != null)
            textComp.SetText($"{startSentence} {pText}");
    }
    
    protected void SetNumber(int pNumber)
    {
        SetText(pNumber.ToString());
    }
    
    protected void SetNumber(float pNumber)
    {
        SetText(MathF.Round(pNumber).ToString());
    }

    protected void SetTextEmpty()
    {
        if (textComp != null)
            textComp.SetText(String.Empty);
    }
}