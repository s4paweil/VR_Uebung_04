using TMPro;
using UnityEngine;

namespace UI
{
    public class FadeLoopTextMeshPro : MonoBehaviour
    {
        private TextMeshProUGUI _textMeshProUGUI;
        private float _time;
        private void Awake()
        {
            _textMeshProUGUI = GetComponent<TextMeshProUGUI>();
            _time = 0;
        }
        private void Update()
        {
            _textMeshProUGUI.alpha = RampUpDown(1f, 2f, 1f, .5f, ref _time);
        }
    
        private static float RampUpDown(float rampUpTime, float upTime, float rampDownTime, float downTime, ref float time)
        {
            var total = rampUpTime + upTime + rampDownTime + downTime;
            time = Mathf.Repeat(time + Time.deltaTime, total);
            var t = Mathf.Clamp01(time / rampUpTime) * Mathf.Clamp01((total - downTime - time) / rampDownTime);
            return t;
        }
    }
}
