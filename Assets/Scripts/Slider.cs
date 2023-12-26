using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.EventSystems;// Required when using Event data.

public class Slider : MonoBehaviour, ISelectHandler, IDeselectHandler// required interface when using the OnSelect method.
{
    [SerializeField] private Button sliderBackground;
    [SerializeField] private Color defaultColor;
    [SerializeField] private Color selectColor;
    //Do this when the selectable UI object is selected.
    void OnStart()
    {
    }
    public void OnSelect(BaseEventData eventData)
    {
        var colors = sliderBackground.colors;
        colors.normalColor = selectColor;
        sliderBackground.colors = colors;
    }

    public void OnDeselect(BaseEventData eventData)
    {
        var colors = sliderBackground.colors;
        colors.normalColor = defaultColor;
        sliderBackground.colors = colors;
    }
}