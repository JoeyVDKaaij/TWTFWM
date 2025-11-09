using UnityEngine;

public class TextFillerGetter : MonoBehaviour
{
    [Header("Text Filler Getter Settings")]
    [SerializeField, Tooltip("Set the text filler to get.")]
    private TMPTextFiller textFiller;

    public TMPTextFiller TextFiller
    {
        get
        {
            return textFiller;
        }
    }
}
