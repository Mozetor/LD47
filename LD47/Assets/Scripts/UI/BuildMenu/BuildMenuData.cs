using System.Collections.Generic;
using UnityEngine;

namespace UI.BuildMenu {

    [CreateAssetMenu(menuName = "Data/Buid Menu UI Data")]
    public class BuildMenuData : ScriptableObject {

        public BuildMenu buildMenu;
        public GameObject slotPrefab;
        public GameObject rowPrefab;
        public Sprite menuSlotSprite;
        public Sprite menuRowSlotSprite;

        public Sprite menuSpriteSimple;
        public Sprite menuSpriteConnected;
    }

    [System.Serializable]
    public class BuildMenu {

        public List<BuildMenuRow> rows;

    }

    [System.Serializable]
    public class BuildMenuRow {

        public string name;
        public Sprite sprite;
        public BuildMenuRowType type;
        public Color spriteColor;
        public List<BuildMenuItem> items;

    }

    [System.Serializable]
    public class BuildMenuItem {

        public string name;
        public Sprite icon;
        public GameObject prefab;

    }

    public enum BuildMenuRowType {
        Normal,
        Demolish
    }
}