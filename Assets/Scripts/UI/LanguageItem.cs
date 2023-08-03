using System;
using System.IO;
using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
using YamlDotNet.RepresentationModel;

namespace UI
{
    public class LanguageItem: MonoBehaviour, IPointerClickHandler 
    {
        private Image _image;
        private TextMeshProUGUI _text;
        private string _path;


        private void Awake()
        {
            _image = transform.Find("Flag").GetComponent<Image>();
            _text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
            
        }

        public void Set(string text, string path)
        {
            _path = path;
            _text.SetText(text);
            _image.sprite = UIHelper.LoadSprite(path + "/flag.png");
        }

        private void OnDestroy()
        {
            _image.sprite = null;
            Resources.UnloadUnusedAssets();
        }
        

        public void OnPointerClick(PointerEventData eventData)
        {
            if (eventData.button == PointerEventData.InputButton.Left)
            {
                var reader = new StreamReader(_path+"/config.yaml");
                var yamlStream = new YamlStream();
                yamlStream.Load(reader);

                var frontMatter = (YamlMappingNode)yamlStream.Documents[0].RootNode;
                var title =  frontMatter["Title"].ToString();
                var font = frontMatter["Font"].ToString();
        
                var body = (YamlMappingNode)yamlStream.Documents[1].RootNode;
                var stages = (YamlMappingNode)body.Children[new YamlScalarNode("Stages")];

                var gameState = UIHelper.GetGameState(transform);
                var selectLevel = gameState.GetSelectLevelGameObject().GetComponent<SelectLevel>();
                selectLevel.SetInformation(title,font,_path);
                selectLevel.SetNode(stages);
                gameState.SetState(GameState.State.SelectLevel);

            }
        }
    }
}
