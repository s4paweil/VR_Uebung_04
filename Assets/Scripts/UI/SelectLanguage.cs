using System.IO;
using Unity.VisualScripting;
using UnityEngine;

namespace UI
{
    public class SelectLanguage : MonoBehaviour
    {
        public Transform content;
        private void Awake()
        {
            UIHelper.ClearContent(content);
            var prefabGameObject = Resources.Load("Prefab/UI/LanguageItem") as GameObject;
            var directoryNames = Directory.GetDirectories(Application.streamingAssetsPath);
            foreach (var directoryPath in directoryNames)
            {
                var directoryName = Path.GetFileName(directoryPath);
                var langeItem = Instantiate(prefabGameObject, content);

                langeItem.GetComponent<LanguageItem>().Set(directoryName, directoryPath);
            }
        }
        
        

    }
}
