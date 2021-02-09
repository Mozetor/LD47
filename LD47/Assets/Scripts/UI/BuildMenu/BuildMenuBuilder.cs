using Assets.WaveSpawner.Implementation;
using Buildings;
using System;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

namespace UI.BuildMenu {

    public class BuildMenuBuilder : MonoBehaviour {

        [SerializeField]
        private BuildMenuData buildMenuData;

        private ConstructionController constructionController;
        private List<GameObject> rows;
        private Transform menuRows;
        private Transform menuBar;
        private Image menuBarImage;

        private void Start() {
            GenerateUI();
            var spawner = FindObjectOfType<BuildBattleSpawner>();
            spawner.AddOnBuildPhaseEnd(() => {
                menuBar.gameObject.SetActive(false);
                menuRows.gameObject.SetActive(false);
            });
            spawner.AddOnBattlePhaseEnd(() => {
                menuBar.gameObject.SetActive(true);
                menuRows.gameObject.SetActive(true);
            });
        }

        private void GenerateUI() {
            constructionController = FindObjectOfType<ConstructionController>();
            menuRows = transform.Find("MenuRows");
            menuBar = transform.Find("MenuBar");
            menuBarImage = menuBar.GetComponent<Image>();
            float size = menuBar.GetComponent<RectTransform>().position.y * 2 - 20;
            GenerateMenuBar(buildMenuData.buildMenu.rows, size);
            rows = new List<GameObject>();
            foreach (var row in buildMenuData.buildMenu.rows) {
                rows.Add(GenerateRow(row, menuRows, size));
            }
            menuRows.gameObject.SetActive(false);
        }

        private void GenerateMenuBar(List<BuildMenuRow> rows, float size) {
            for (int i = 0; i < rows.Count; i++) {
                var row = rows[i];
                int j = i;
                GenerateItem(""/*row.name*/, row.sprite, buildMenuData.menuRowSlotSprite, () => ActivateRow(j), menuBar, size, row.spriteColor);
            }
        }

        private GameObject GenerateRow(BuildMenuRow row, Transform parent, float size) {
            GameObject rowGO = Instantiate(buildMenuData.rowPrefab, parent);
            for (int i = 0; i < row.items.Count; i++) {
                var item = row.items[i];
                GenerateItem(""/*item.name*/, item.icon, buildMenuData.menuSlotSprite, () => constructionController.StartPlacement(item.prefab.GetComponent<IPlaceable<Building>>()), rowGO.transform, size);
            }
            rowGO.SetActive(false);
            return rowGO;
        }

        private GameObject GenerateItem(string name, Sprite icon, Sprite background, Action buttonPress, Transform parent, float size, Color color = default) {
            GameObject slot = Instantiate(buildMenuData.slotPrefab, parent);
            slot.transform.Find("background").GetComponent<Image>().sprite = background;
            var iconImage = slot.transform.Find("icon").GetComponent<Image>();
            iconImage.sprite = icon;
            iconImage.color = color == default ? Color.white : color;
            slot.GetComponentInChildren<Button>().onClick.AddListener(() => buttonPress());
            slot.GetComponentInChildren<TextMeshProUGUI>().text = name;
            slot.GetComponent<RectTransform>().sizeDelta = new Vector2(size, size);
            return slot;
        }

        public void ActivateRow(int index) {
            bool isExtended = false;

            for (int i = 0; i < rows.Count; i++) {
                bool rowEqual = i == index;
                isExtended = (rowEqual && !rows[i].activeSelf) || isExtended;
                if (rowEqual) {
                    rows[i].SetActive(!rows[i].activeSelf);
                }
                else {
                    rows[i].SetActive(false);
                }
            }
            menuRows.gameObject.SetActive(isExtended);
            menuBarImage.sprite = isExtended ? buildMenuData.menuSpriteConnected : buildMenuData.menuSpriteSimple;
        }
    }
}