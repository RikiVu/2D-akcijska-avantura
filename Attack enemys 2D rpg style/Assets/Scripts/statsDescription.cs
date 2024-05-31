
using UnityEngine;
using UnityEngine.EventSystems;

public class statsDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    public toolTipScr tooltipSCR;
    [SerializeField]
    private string textToShowTitle = " ";
    [SerializeField]
    private string textToShow = " ";
    private Vector3 temp = new Vector3(0,100,0);

    void Start()
    {
        //Tooltip = GameObject.FindGameObjectWithTag("ToolTip");
        //tooltipSCR = Tooltip.GetComponent<toolTipScr>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
            tooltipSCR.ChangeText(textToShowTitle, textToShow);
            tooltipSCR.transform.position = transform.position + temp;
             tooltipSCR.gameObject.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        tooltipSCR.gameObject.SetActive(false);
    }
}
