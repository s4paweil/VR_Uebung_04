using System.Collections;
using System.Collections.Generic;
using TMPro;
using UI;
using UnityEngine;
using UnityEngine.Networking;
using UnityEngine.UI;

public class InstructionInput : MonoBehaviour
{
    private RectTransform _rectTransform;
    private Image _higlight;

    private TextMeshProUGUI _l;
    private TextMeshProUGUI _t;
    private TextMeshProUGUI _r;
    private TextMeshProUGUI _b;


    private AudioClip _audioClip;


    // Start is called before the first frame update
    void Awake()
    {
        _rectTransform = transform.GetComponent<RectTransform>();
        _higlight = transform.Find("fx").Find("inner").GetComponent<Image>();
        _l = transform.Find("l").GetComponent<TextMeshProUGUI>();
        _t = transform.Find("t").GetComponent<TextMeshProUGUI>();
        _r = transform.Find("r").GetComponent<TextMeshProUGUI>();
        _b = transform.Find("b").GetComponent<TextMeshProUGUI>();
    }

    // Update is called once per frame
    void Update()
    {
        if (Vector3.Distance(_rectTransform.localScale,new Vector3(1.2f, 1.2f, 1.0f))>0.1f)
        {
            _rectTransform.localScale =
                Vector3.Lerp(_rectTransform.localScale, new Vector3(1.0f, 1.0f, 1.0f), Time.deltaTime);
            _higlight.color = Color.Lerp(_higlight.color, Color.white, Time.deltaTime*2f);
        }

    }

    public void Set(string l, string t, string r, string b, string sound, TMP_FontAsset font)
    {

        _audioClip = UIHelper.LoadAudioClip(sound);
        _l.font = font;
        _t.font = font;
        _r.font = font;
        _b.font = font;
        
        _l.SetText(l);
        _t.SetText(t);
        _r.SetText(r);
        _b.SetText(b);
    }

    public void pulse()
    {
        _rectTransform.localScale = new Vector3(1.2f,1.2f,1.2f);
        _higlight.color = Color.black;
    }
    
    
    public void playSound()
    {
        var player = transform.GetComponent<AudioSource>() ;
        player.PlayOneShot(_audioClip);

    }
    

}
