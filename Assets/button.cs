using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class ButtonClickHandler : MonoBehaviour, IPointerClickHandler
{
    public Button button;

    public void OnPointerClick(PointerEventData eventData)
    {
        Debug.Log("Button clicked");
    }
}
