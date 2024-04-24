
using UnityEngine;
using UnityEngine.EventSystems;

public class statsDescription : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    private GameObject Tooltip;
    toolTipScr tooltipSCR;
    [SerializeField]
    private string textToShowTitle = " ";
    [SerializeField]
    private string textToShow = " ";
    private Vector3 temp = new Vector3(0,100,0);

    void Start()
    {
        Tooltip = GameObject.FindGameObjectWithTag("ToolTip");
        tooltipSCR = Tooltip.GetComponent<toolTipScr>();
    }
    public void OnPointerEnter(PointerEventData eventData)
    {
            tooltipSCR.ChangeText(textToShowTitle, textToShow);
            Tooltip.transform.position = transform.position + temp;
            Tooltip.SetActive(true);
    }
    public void OnPointerExit(PointerEventData eventData)
    {
        Tooltip.SetActive(false);
    }
}
