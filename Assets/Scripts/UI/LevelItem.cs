using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UI;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.EventSystems;
using YamlDotNet;
using YamlDotNet.RepresentationModel;

public class LevelItem : MonoBehaviour, IPointerClickHandler 
{
    private TextMeshProUGUI _text;
    private KeyValuePair<YamlNode,YamlNode> _currentNode;
    private string _title;
    private string _font;
    private string _path;
    private void Awake()
    {
        _text = transform.Find("Text").GetComponent<TextMeshProUGUI>();
    }
    
    public void SetNode(KeyValuePair<YamlNode,YamlNode> node)
    {
        _currentNode = node;
        _text = transform.Find("Text").GetComponent<TextMeshProUGUI>();

        _text.SetText(((YamlScalarNode)_currentNode.Key).Value);
            
    }
    
    public void SetInformation(string title,string font, string path)
    {
        _title = title;
        _font = font;
        _path = path;
    }

    private string ReadValue(YamlMappingNode yamlMappingNode, string key)
    {
        var yamlScalarNode = new YamlScalarNode(key);
        return yamlMappingNode.Children.TryGetValue(yamlScalarNode, out var c) ? c.ToString() : "";
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if(_currentNode.Value.NodeType == YamlNodeType.Sequence)
        {
            var phraseList = new List<Phrase>();
            foreach (var n in (YamlSequenceNode)_currentNode.Value)
            {
                var v = (YamlMappingNode)n;
                var p = new Phrase
                {
                    characters = ReadValue(v, "char"),
                    combination = ReadValue(v, "comb"),
                    roman = ReadValue(v, "roman"),
                    eqphonem = ReadValue(v, "eqphonem"),
                    sound = ReadValue(v, "sound")
                };
                phraseList.Add(p);
            }

            var gamestate = UIHelper.GetGameState(transform);
            var g = gamestate.GetGame().GetComponent<Game>();
            g.SetPhraseList(phraseList);
            g.SetInformation(_title,_font,_path);
                
            gamestate.SetState(GameState.State.Game);
        }else{
            UIHelper.GetGameState(transform).GetSelectLevelGameObject().GetComponent<SelectLevel>().SetNode((YamlMappingNode)_currentNode.Value);
        }
    }
}
