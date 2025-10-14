using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

public class UI_DifficultyButton : MonoBehaviour, IPointerEnterHandler, IPointerExitHandler
{
    [SerializeField] private TextMeshProUGUI infoTextMesh;

    [TextArea]
    [SerializeField] private string infoText;

    public void OnPointerEnter(PointerEventData eventData)
    {
        infoTextMesh.text = infoText;
    }

    public void OnPointerExit(PointerEventData eventData)
    {
        infoTextMesh.text = "";
    }
}
