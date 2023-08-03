using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using YamlDotNet.RepresentationModel;
using UnityEngine.UI;

namespace UI
{
    public class SelectLevel : MonoBehaviour
    {
        public Transform content;
        private Image _image;
        private TextMeshProUGUI _text;
                
        private string _title;
        private string _font;
        private string _path;
        private Stack<YamlMappingNode> _stackNodes = new Stack<YamlMappingNode>();
        private GameObject _prefabGameObject;

       private void Awake()
        {
            _prefabGameObject = Resources.Load("Prefab/UI/LevelItem") as GameObject;
            _image = transform.Find("Flag").GetComponent<Image>();
            _text = transform.Find("Text").GetComponent<TextMeshProUGUI>();

        }


       public void SetInformation(string title,string font, string path)
        {
            _title = title;
            _font = font;
            _path = path;
            
            _image = transform.Find("Flag").GetComponent<Image>();
            _text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
            
            _image.sprite = UIHelper.LoadSprite(_path + "/flag.png");
            _text.SetText(_title);
        }

        public void SetNode(YamlMappingNode node)
        {
            UIHelper.ClearContent(content);
            _stackNodes.Push(node);
            _prefabGameObject = Resources.Load("Prefab/UI/LevelItem") as GameObject;
            foreach (var e in node.Children)
            {
                var n = Instantiate(_prefabGameObject, content);
                var l = n.GetComponent<LevelItem>();
                l.SetInformation(_title,_font,_path);
                l.SetNode(e);
            }
        }

        public void Back()
        {
            if (_stackNodes.Count > 1)
            {
                _stackNodes.Pop();
                SetNode(_stackNodes.Pop());
            }
            else
            {
                _stackNodes.Clear();
                UIHelper.GetGameState(transform).SetState(GameState.State.SelectLanguage);
            }
        }


    }
}