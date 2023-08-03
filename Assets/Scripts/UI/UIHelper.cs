using System;
using TMPro;
using UnityEngine;
using UnityEngine.Networking;
using Object = UnityEngine.Object;

namespace UI
{
    public static class UIHelper
    {
        
        public static Sprite LoadSprite(string path) 
        {
            var bytes = System.IO.File.ReadAllBytes(path);
            var texture = new Texture2D(2, 2);
            texture.LoadImage(bytes);
            var sprite = Sprite.Create(texture, new Rect(0.0f, 0.0f, texture.width, texture.height), new Vector2(1f, 1f), 100.0f);
            return sprite;
        }
        
        public static Font LoadFont(string path) 
        {
            Font customFont = new Font(path);
            return customFont;

        }

        public static TMP_FontAsset LoadFontAsset(string path) 
        {
            Font customFont = LoadFont(path);
            TMP_FontAsset asset = TMP_FontAsset.CreateFontAsset(customFont);
            return asset;

        }
        
        public static AudioClip LoadAudioClip(string path) 
        {
            UnityWebRequest www = UnityWebRequestMultimedia.GetAudioClip(path, AudioType.UNKNOWN);
            www.SendWebRequest();
            while (!www.isDone)
            {
            }

            if (www.result == UnityWebRequest.Result.Success)
            {
                return DownloadHandlerAudioClip.GetContent(www);
            }
            
            return null;

        }

        public static GameState GetGameState(Transform transform)
        {
            GameState gameState = null;
            GameObject node = transform.gameObject;
            while (!gameState)
            {
                gameState = node.GetComponent<GameState>();
                node = node.transform.parent.gameObject;
            }

            return gameState;
        }

        public static void ClearContent(Transform content)
        {
            foreach (Transform child in content.transform) {
                Object.Destroy(child.gameObject);
            }
        }
    }
}