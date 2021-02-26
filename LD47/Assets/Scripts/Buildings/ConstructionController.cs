using City;
using System;
using UnityEngine;
using UnityEngine.EventSystems;
using Worldgeneration;

namespace Buildings {

    public class ConstructionController : MonoBehaviour {

        public enum ConstructionState {
            None,
            Placing,
            Selling
        }

        private BuildingsData<Building> buildingsData;
        private CityController city;
        private Camera cam;
        private WorldData worldData;
        [SerializeField]
        private Transform parent;
        [SerializeField]
        private Color placeableColor;
        [SerializeField]
        private Color cantBuyColor;
        [SerializeField]
        private Color notPlaceableColor;


        private ConstructionState state;
        private IPlaceable<Building> placeable;
        private GameObject instance;
        private SpriteRenderer[] spriteRenderers;

        public ConstructionState State => state;
        public Action onStateChanged;

        private void Start() {
            worldData = FindObjectOfType<TileMapGenerator>().worldData;
            city = FindObjectOfType<CityController>();
            buildingsData = new BuildingsData<Building>(worldData, () => GenerateTerrainPlaceability(worldData));
            state = ConstructionState.None;
            cam = Camera.main;
        }

        // Seems to cause crashes when pausing ?!
        //private void OnDrawGizmos() {
        //    buildingsData?.DrawGizmos();
        //}

        private void Update() {

            switch (state) {
                case ConstructionState.Placing:
                    UpdatePlacing();
                    ListenCancel();
                    break;
                case ConstructionState.Selling:
                    UpdateSelling();
                    ListenCancel();
                    break;
            }
        }

        private void ListenCancel() {
            if (Input.GetKeyDown(KeyCode.Escape) || Input.GetMouseButtonDown(1)) {
                CleanUp();
                onStateChanged?.Invoke();
            }
        }
        /// <summary>
        /// Updates when currently in placement mode.
        /// </summary>
        private void UpdatePlacing() {
            var pos = CoordinateConverter.GetCurrentMouseTilePosition(cam);
            instance.transform.position = CoordinateConverter.ToVector3Int(pos);

            // Update color of instance.
            var color = buildingsData.InMap(CoordinateConverter.WorldToArray(pos, worldData.worldSize))
                && buildingsData.IsPlaceable(placeable, pos) ? placeableColor : notPlaceableColor;
            color = city.CanBuyByCost(placeable.GetCost()) ? color : cantBuyColor;
            for (int i = 0; i < spriteRenderers.Length; i++) {
                spriteRenderers[i].color = color;
            }

            // Place building if possible.
            if (Input.GetMouseButtonDown(0) && !EventSystem.current.IsPointerOverGameObject()
                && buildingsData.InMap(CoordinateConverter.WorldToArray(pos, worldData.worldSize))
                && buildingsData.IsPlaceable(placeable, pos) && city.Buy(placeable.GetCost())) {
                GameObject gameObject = Instantiate(placeable.GetObject(), instance.transform.position, Quaternion.identity, parent);
                if (!buildingsData.TrySet(gameObject.GetComponent<IPlaceable<Building>>(), pos)) {
                    throw new InvalidOperationException("This shouldn't be possible!");
                }
            }
        }
        /// <summary>
        /// Updates when currently in selling mode.
        /// </summary>
        private void UpdateSelling() {
            var pos = CoordinateConverter.GetCurrentMouseTilePosition(cam);

            if (!buildingsData.InMap(CoordinateConverter.WorldToArray(pos, worldData.worldSize))) {
                return;
            }

            var b = buildingsData.GetBuilding(CoordinateConverter.WorldToArray(pos, worldData.worldSize));
            if (Input.GetMouseButtonDown(0) && b != null) {
                RemoveBuilding(b);
            }
        }
        /// <summary>
        /// Initiates the placement of placeables.
        /// </summary>
        /// <param name="placeable"> Placeable to use for placement </param>
        public void StartPlacement(IPlaceable<Building> placeable) {
            CleanUp();
            this.placeable = placeable;
            state = ConstructionState.Placing;

            var pos = CoordinateConverter.ToVector3Int(CoordinateConverter.GetCurrentMouseTilePosition(cam));
            instance = Instantiate(placeable.GetObject(), pos, Quaternion.identity, transform);
            instance.GetComponent<Building>().enabled = false;

            spriteRenderers = instance.GetComponentsInChildren<SpriteRenderer>();
            // Make instance be in the foreground.
            for (int i = 0; i < spriteRenderers.Length; i++) {
                spriteRenderers[i].sortingOrder += 20;
            }
            onStateChanged?.Invoke();
        }
        /// <summary>
        /// Start selling mode.
        /// </summary>
        public void StartSelling() {
            CleanUp();
            state = ConstructionState.Selling;
            onStateChanged?.Invoke();
        }
        /// <summary>
        /// Stops current mode and resets.
        /// </summary>
        public void Stop() {
            CleanUp();
            onStateChanged?.Invoke();
        }
        /// <summary>
        /// Removes a placeable from the map.
        /// </summary>
        /// <param name="placeable"> Placeable to remove </param>
        public void RemoveBuilding(IPlaceable<Building> placeable) {
            buildingsData.Remove(placeable);
            placeable.PrepareRemoval();
            Destroy(placeable.GetObject());
        }
        /// <summary>
        /// Resets the current state and deletes all instances of old state.
        /// </summary>
        private void CleanUp() {
            if (instance != null) {
                Destroy(instance);
            }
            state = ConstructionState.None;
            placeable = null;
        }
        /// <summary>
        /// Generate the terrain placeability array.
        /// </summary>
        /// <param name="data"> Current world data </param>
        /// <returns> Terrain placeability array of given world data </returns>
        private bool[,] GenerateTerrainPlaceability(WorldData data) {
            bool[,] result = new bool[data.worldSize.x, data.worldSize.y];

            for (int x = 0; x < data.worldSize.x; x++) {
                for (int y = 0; y < data.worldSize.y; y++) {
                    result[x, y] = true;
                }
            }

            var layers = data.WorldPixelTypes;
            for (int i = 0; i < data.Layers; i++) {
                // Ignore all layers that are 'Walkable'.
                if (data.worldLayers[i].NavMeshSelector == 0) {
                    break;
                }
                var layer = layers[i];
                for (int x = 0; x < data.worldSize.x; x++) {
                    for (int y = 0; y < data.worldSize.y; y++) {
                        if (layer[x, y] != "") {
                            result[x, y] = false;
                        }
                    }
                }
            }
            return result;
        }
    }
}