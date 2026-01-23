using System.Collections.Generic;
using UnityEngine;
using TMPro;

namespace Game
{
    [System.Serializable]
    public class UIData
    {
        [SerializeField] private GameObject uiObject;
        [SerializeField] private string prefix;
        [SerializeField] private UIType type;

        public GameObject Object => uiObject;
        public TextMeshProUGUI Text => uiObject.GetComponent<TextMeshProUGUI>();
        public string Prefix => prefix;
        public UIType Type => type;
    }

    public enum UIType
    {
        // TitleScene
        None,

        // PlayScene
        ReadyCount,
        TimeLimit,
        ItemCount,
        ClearItemCount,
        JumpCount,
        //Effect
        GameOver,

        // ResultScene
        Score,
        HitCount
    }
}