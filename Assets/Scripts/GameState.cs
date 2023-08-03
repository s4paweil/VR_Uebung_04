using System;
using System.Collections;
using System.Collections.Generic;
using UI;
using UnityEngine;
using YamlDotNet.RepresentationModel;

public class GameState : MonoBehaviour
{
    public enum State {Splash, SelectLanguage, SelectLevel, Game};
    // Start is called before the first frame update
    
    private GameObject _Splash;
    private GameObject _SelectLanguage;
    private GameObject _SelectLevel;
    private GameObject _Game;
    

    
    private void Awake()
    {
        _Splash = transform.Find("Splash").gameObject;
        _SelectLanguage = transform.Find("SelectLanguage").gameObject;
        _SelectLevel = transform.Find("SelectLevel").gameObject;
        _Game = transform.Find("Game").gameObject;

    }

    void Start()
    {
        SetState(State.Splash);
    }

    public GameObject GetSelectLanguageGameObject()
    {
        return _SelectLanguage;
    }
    
    public GameObject GetSelectLevelGameObject()
    {
        return _SelectLevel;
    }

    public GameObject GetGame()
    {
        return _Game;
    }


    public void SetState(State state)
    {
        switch (state)
        {
            case State.Splash:
                HideChildren();
                _Splash.SetActive(true);
                break;
            case State.SelectLanguage:
                HideChildren();
                _SelectLanguage.SetActive(true);
                break;
            case State.SelectLevel:
                HideChildren();
                _SelectLevel.SetActive(true);
                break;
            case State.Game:
                HideChildren();
                _Game.SetActive(true);
                break;
            default:
                throw new ArgumentOutOfRangeException(nameof(state), state, null);
        }
    }

    void Update()
    {
        
    }

    void HideChildren()
    {
        foreach (Transform child in transform) {
            child.gameObject.SetActive(false);
        }
    }
}
