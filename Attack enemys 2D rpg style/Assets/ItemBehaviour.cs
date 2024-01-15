
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemBehaviour : MonoBehaviour, IPointerEnterHandler, IPointerClickHandler, IPointerExitHandler
{
    public item itemScr;

    public void OnPointerEnter(PointerEventData eventData)
    {
      
        itemScr.PointerEnter();
    }
    public void OnPointerClick(PointerEventData eventData)
    {
        itemScr.PointerClick();
    }
    public void OnPointerExit(PointerEventData eventData)
    {
       itemScr.PointerExit();
    }
}
