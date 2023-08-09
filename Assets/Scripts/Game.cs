using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using TMPro;
using UI;
using UnityEngine;
using YamlDotNet;
using YamlDotNet.Core.Tokens;
using Random = UnityEngine.Random;

public class Game : MonoBehaviour
{
    
    private KeyCode[] gameInput = new KeyCode[4]
    {
        KeyCode.LeftArrow,
        KeyCode.UpArrow,
        KeyCode.RightArrow,
        KeyCode.DownArrow
    };
    
    //GameStateInternal
    private enum PlayerState {Alive, Dead};
    private enum InstructionState {Start, Introduce,Show, Input, Check, EndPhase};
    
    
    private PlayerState _playerState;
    private InstructionState _instructionState;
    
    // Gametechnicals
    public Transform content;
    private GameObject _prefabGameObject;
    private AudioSource _audioSource;
    private AudioClip _katsuAudioCLip;
    private AudioClip _donAudioClip;
    private AudioClip _AudioClipHelper;

    //GameModel
    private List<Phrase> _phraseList = new List<Phrase>();
    private int _phraseTop;

    //GameLogic
    private float _elapsed;
    private int _count;
    private int _maxCount;

    private KeyCode _currentKey;

  
    private string _path;
    private string _title;
    private string _font;
    private TMP_FontAsset _fontAsset;

    private System.Random _rnd = new System.Random();
    private void Start()
    {
        _elapsed = 1f;
        _count = 0;
        _maxCount = 0;
        _phraseTop = 0;
        _audioSource = gameObject.AddComponent <AudioSource>() ;
        _katsuAudioCLip = Resources.Load<AudioClip>("Music/Effects/Katsu");
        _donAudioClip = Resources.Load<AudioClip>("Music/Effects/Don");
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
        _phraseList = list;
        _playerState = PlayerState.Alive;
        ClearContent();
        _count = 0;
        _maxCount = 0;
        _phraseTop = 0;
        _instructionState = InstructionState.Start;
    }
    

    private void Push()
    {
        if (_phraseTop < _phraseList.Count)
        {
            _phraseTop++;
            _instructionState = InstructionState.Introduce;
        }
        else
        {
            _instructionState = InstructionState.Show;
            
        }
    }
    
    private Phrase GetTop()
    {
        return _phraseList[_phraseTop-1];
    }
    
    private Phrase GetAt(int i)
    {
        return _phraseList[i];
    }

    
    private Phrase GetRand()
    {
        return _phraseList[_rnd.Next(_phraseTop)];
    }

    

    
    private void Update()
    {
        if (_playerState == PlayerState.Alive)
        {
            _elapsed += Time.deltaTime;
            if (_instructionState == InstructionState.Input ||
                (_instructionState == InstructionState.Check && _elapsed < 0.25))
            {
                AssignInput();
            }

            if (_elapsed >= 1f) {
                _elapsed = _elapsed % 1f;
                Metronome();
            }

            if (Input.GetKeyDown(KeyCode.Escape))
            {
                UIHelper.GetGameState(transform).SetState(GameState.State.SelectLevel);
            }


        }
    }

    private void AssignInput()
    {
        if (!Input.anyKeyDown)
            return;

        
        foreach (KeyCode key in gameInput)
            if (Input.GetKeyDown(key))
            {
                
                _currentKey = key;
            }

        if (_elapsed < 0.25)
        {
            if (_instructionState == InstructionState.Check)
            {
                content.transform.GetChild(content.transform.childCount-1).GetComponent<InstructionInput>().SetGuess(_currentKey);
                
            }else{

                content.transform.GetChild(Math.Max(_count-1,0)).GetComponent<InstructionInput>().SetGuess(_currentKey);

            }
        }
        if(_elapsed > 0.75 && _instructionState == InstructionState.Input) content.transform.GetChild(Math.Min(_count,content.transform.childCount-1)).GetComponent<InstructionInput>().SetGuess(_currentKey);


    }

    private void ClearInput()
    {
        _currentKey = KeyCode.Escape;
    }

    private void Metronome()
    {
 

        switch (_instructionState)
        {
            case InstructionState.Start:
                if (_maxCount < 2)
                {
                    Push();
                }
                else
                {
                    if (_maxCount % 4 == 0)
                    {
                        Push();
                    }    
                }
                Fill();
                break;
            case InstructionState.Introduce:
            case InstructionState.Show:
                HandleShowState();
                break;
            case InstructionState.Input:
                HandleInputState();
                ClearInput();
                break;
            case InstructionState.Check:
                HandleCheckState();
                break;
            case InstructionState.EndPhase:
                _maxCount++;
                _instructionState = InstructionState.Start;
                break;
            default:
                throw new ArgumentOutOfRangeException();
        }
    }


    void Permute(Phrase c, Phrase p2, Phrase p3, Phrase p4, TMP_FontAsset _fontAsset)
    { 
        var a = Instantiate(_prefabGameObject, content);
        var i = a.GetComponent<InstructionInput>();

        switch (_rnd.Next(4))
        {
            case 0:
                i.Set(c.characters, p2.characters, p3.characters, p4.characters, _path + c.sound,_fontAsset);
                i.SetSolution(KeyCode.LeftArrow);
                break;
            case 1:
                i.Set(p2.characters, c.characters,p3.characters, p4.characters, _path + c.sound,_fontAsset);
                i.SetSolution(KeyCode.UpArrow);
                break;
            case 2:
                i.Set(p2.characters, p3.characters,c.characters, p4.characters, _path + c.sound,_fontAsset);
                i.SetSolution(KeyCode.RightArrow);
                break;
            case 3:
                i.Set(p2.characters, p3.characters,p4.characters, c.characters, _path + c.sound,_fontAsset);
                i.SetSolution(KeyCode.DownArrow);
                break;
            default:
                break;
        }
        
    }

    private Phrase[] GetThreeRandExceptP(Phrase p)
    {
        Phrase[] phraseArray = new Phrase[4];
        int j = 0;
        if (_phraseTop <= 4)
        {
            for (int i = 0; i < _phraseTop; i++)
            {
                var p2 = GetAt(i);
                if (p != p2)
                {
                    phraseArray[j++] = p2;
                }
            }
        }
        else
        {
            while (j <= 3)
            {
                var p2 = GetAt(_rnd.Next(_phraseTop));
                if (p != p2)
                {
                    phraseArray[j++] = p2;
                }
            }
        }
        
        return phraseArray;

    }

    private Phrase[] GetThreeRandExceptPSplit(Phrase p)
    {
        Phrase[] phraseArray = new Phrase[4];
        int j = 0;
        while (j <= 3)
        {
            var p2 = GetAt(_rnd.Next(_phraseList.Count));
            p2 = p2.split[_rnd.Next(p2.split.Count)];
            if (p.characters != p2.characters)
            {
                    phraseArray[j++] = p2;
            }
        }

        return phraseArray;

    }
    
    void Fill()
    {
        ClearContent();
        Phrase correctPhrase = null;
        Phrase dummyPhrase = new Phrase();

        if (GetAt(0).split.Count > 0)
        {
            _phraseTop = _phraseList.Count - 1;
            correctPhrase = GetRand();

            _AudioClipHelper = UIHelper.LoadAudioClip(_path + correctPhrase.sound);
            _audioSource.PlayOneShot(_AudioClipHelper);
            for (int i = 0; i < correctPhrase.split.Count; i++)
            {
                Phrase[] incorrect = GetThreeRandExceptPSplit(correctPhrase.split[i]);
                Permute(correctPhrase.split[i],incorrect[0],incorrect[1],incorrect[2],_fontAsset);
            }
            
            
            
        }else{

            for (int i = 0; i < 4; i++)
            {
                if (_instructionState == InstructionState.Introduce)
                {
                    correctPhrase = GetTop();
                    Phrase[] incorrect = GetThreeRandExceptP(correctPhrase);
                    Permute(correctPhrase,dummyPhrase,dummyPhrase,dummyPhrase,_fontAsset);
                }
                else
                {
                    correctPhrase = GetRand();
                    Phrase[] incorrect = GetThreeRandExceptP(correctPhrase);
                    if (_phraseTop == 1) Permute(correctPhrase,dummyPhrase,dummyPhrase,dummyPhrase,_fontAsset);
                    if (_phraseTop == 2) Permute(correctPhrase,incorrect[0],dummyPhrase,dummyPhrase,_fontAsset);
                    if (_phraseTop == 3) Permute(correctPhrase,incorrect[0],incorrect[1],dummyPhrase,_fontAsset);
                    if (_phraseTop >= 4) Permute(correctPhrase,incorrect[0],incorrect[1],incorrect[2],_fontAsset);

                }
            
     
            }
            
        }
        _count = 0;
        _instructionState = InstructionState.Show;
    }
    


    void HandleShowState()
    {
        if (transform.childCount <= 0)
        {
            Fill();
        }

       content.transform.GetChild(_count).GetComponent<InstructionInput>().playSound();
       content.transform.GetChild(_count).GetComponent<InstructionInput>().pulse();
        _count++;
        if (_count >=content.transform.childCount)
        {
            _instructionState = InstructionState.Input;
            _count = 0;
        }

    }
    void HandleInputState()
    {
        content.transform.GetChild(_count).GetComponent<InstructionInput>().pulse();
        _count++;
        if (_count >= content.transform.childCount)
        {
            _audioSource.PlayOneShot(_donAudioClip);
            _count = 0;
            _instructionState = InstructionState.Check;
        }
        else
        {
            _audioSource.PlayOneShot(_katsuAudioCLip);
        }
    }
    void HandleCheckState()
    {
        if (content.transform.GetChild(_count).GetComponent<InstructionInput>().ValidInput())
        {
            content.transform.GetChild(_count).GetComponent<InstructionInput>().green();
            content.transform.GetChild(_count).GetComponent<InstructionInput>().playSound();
            content.transform.GetChild(_count).GetComponent<InstructionInput>().pulse();
        }
        else
        {
            content.transform.GetChild(_count).GetComponent<InstructionInput>().red();
        }

        _count++;
        if (_count >=content.transform.childCount)
        {
            _instructionState = InstructionState.EndPhase;
            _count = 0;
        }
    }


    private void ClearContent()
    {
        foreach (Transform child in content.transform) {
            Destroy(child.gameObject);
        }
    }

  
}
