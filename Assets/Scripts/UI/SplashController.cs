using System;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;

namespace UI
{
    public class SplashController : MonoBehaviour, IPointerClickHandler 
    {
        public void OnPointerClick(PointerEventData eventData)
        {
            UIHelper.GetGameState(transform).SetState(GameState.State.SelectLanguage);
        }
    }
}
