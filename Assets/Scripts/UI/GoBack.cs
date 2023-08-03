using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class GoBack : MonoBehaviour , IPointerClickHandler 
    {
        public void OnPointerClick(PointerEventData eventData)
        {

            if (eventData.button == PointerEventData.InputButton.Left)
            {
                UIHelper.GetGameState(transform).GetSelectLevelGameObject().GetComponent<SelectLevel>().Back();
            }
        }
    }
}
