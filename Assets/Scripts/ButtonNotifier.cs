using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class ButtonNotifier : MonoBehaviour, IDeselectHandler, ISelectHandler
{
    public bool Selected
    {
        get
        {
            return selected;
        }
    }

    private bool selected;

    public void OnDeselect(BaseEventData evenData)
    {
        selected = false;
    }

    public void OnSelect(BaseEventData eventData)
    {
        selected = true;
    }
}