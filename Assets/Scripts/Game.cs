using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using TMPro;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Networking;
using YamlDotNet;
using YamlDotNet.RepresentationModel;

public class Game : MonoBehaviour
{
    public enum InstructionState {Show, Input, Check};
    public enum PlayerState {Alive, Dead};

    public Transform content;
    private GameObject _prefabGameObject;

    private AudioSource audioSource;

    private bool introduce;
    
    
    float elapsed;
    private AudioSource player; 
    private AudioClip Katsu;
    private AudioClip Don;

    private int count;


    private List<Phrase> _phraseList = new List<Phrase>();
    private List<Phrase> _introduced = new List<Phrase>();
    private PlayerState _playerState;
    private InstructionState _instructionState;
    private int _sequence_count;
    private int _charIndex;
    private string _path;
    private string _title;
    private string _font;
    private TMP_FontAsset _fontAsset;
    private void Start()
    {
        
        elapsed = 1f;
        count = 0;
        player = gameObject.AddComponent <AudioSource>() ;
        Katsu = Resources.Load<AudioClip>("Music/Effects/Katsu");
        Don = Resources.Load<AudioClip>("Music/Effects/Don");
        _prefabGameObject = Resources.Load("Prefab/UI/InputDiamond") as GameObject;
   
    }

 
    public void SetInformation(string title,string font, string path)
    {
        _title = title;
        _font = font;
        _path = path;
        _fontAsset = UIHelper.LoadFontAsset(_path + _font);
    }
    
    public void SetPhraseList(List<Phrase> list)
    {
        _introduced.Clear();
        _phraseList = list;
        _sequence_count = 0;
        _playerState = PlayerState.Alive;
        Pop();
    }
    

    private void Pop()
    {
        if (_charIndex < _phraseList.Count)
        {
            _introduced.Add(_phraseList[_charIndex++]);
            _instructionState = InstructionState.Show;
        }
    }
    
    private void Update()
    {
        if (_playerState == PlayerState.Alive)
        {
            elapsed += Time.deltaTime;
            if (elapsed >= 1f) {
                elapsed = elapsed % 1f;
                Metronome();
            }

            if (elapsed < 0.3f || elapsed > 0.7f)
            {
                //GetInput();
            }
        }
    }
    
    private void Metronome()
    {
        switch (_instructionState)
        {
            case InstructionState.Show:
                HandleShow();
                break;
            case InstructionState.Input:
                HandleInput();
                break;
            case InstructionState.Check:
                HandleCheck();
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }

    List<GameObject> _currentInstructionList = new List<GameObject>();
    void Fill()
    {
        ClearContent();
        for (int i = 0; i < 4; i++)
        {
            var c = _introduced[_introduced.Count - 1];
            var a = Instantiate(_prefabGameObject, content);
            a.GetComponent<InstructionInput>().Set(c.characters, c.characters, c.characters, c.characters, _path + c.sound,_fontAsset);
            _currentInstructionList.Add(a);
        }

        count = 0;
    }

    void HandleShow()
    {
        if (_currentInstructionList.Count <= 0)
        {
            Fill();
        }

        _currentInstructionList[count].GetComponent<InstructionInput>().playSound();
        _currentInstructionList[count].GetComponent<InstructionInput>().pulse();
        count++;
        if (count >= _currentInstructionList.Count)
        {
            _instructionState = InstructionState.Input;
            count = 0;
        }

    }
    void HandleInput()
    {
        count++;
        if (count >= _currentInstructionList.Count)
        {
            player.PlayOneShot(Don);
            count = 0;
            _instructionState = InstructionState.Check;
        }
        else
        {
            player.PlayOneShot(Katsu);
        }
    }
    void HandleCheck()
    {
        _currentInstructionList[count].GetComponent<InstructionInput>().playSound();
        _currentInstructionList[count].GetComponent<InstructionInput>().pulse();
        count++;
        if (count >= _currentInstructionList.Count)
        {
            _instructionState = InstructionState.Show;
            count = 0;
        }
    }


    private void ClearContent()
    {
        foreach (Transform child in content.transform) {
            Destroy(child.gameObject);
        }
    }

  
}
