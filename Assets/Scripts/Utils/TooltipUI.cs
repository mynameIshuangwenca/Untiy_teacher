using UnityEngine;
using UnityEngine.UI;
using System.Collections;

public class TooltipUI : MonoBehaviour
{
    public Text OutlineText;
    public Text ContentText;

    public void UpdateTooltip(string text)
    {
        OutlineText.text = text;
        ContentText.text = text;
    }

    public void Show(Vector3 position, string text)
    {
        OutlineText.text = text;
        ContentText.text = text;
        gameObject.SetActive(true);
        transform.position = position;
    }

    public void Hide()
    {
        gameObject.SetActive(false);
    }

   
}
