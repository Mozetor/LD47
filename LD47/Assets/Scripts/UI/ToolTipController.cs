using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using UnityEngine.EventSystems;

namespace Assets.ToolTip {

    /// <summary>
    /// This controller checks, whether the mouse is over a <see cref="GameObject"/>, 
    /// which should display a tooltip on mouse over.
    /// </summary>
    public class ToolTipController : MonoBehaviour {

        [Header("Tooltip UI")]
        /// <summary> The background image, which will be rescaled and colored to fit accordingly. </summary>
        [SerializeField]
        [Tooltip("The background image behind the tooltip, which will be colored according to the displayed tooltip.")]
        private Image backgroundImage;

        /// <summary> The text component containing the tooltip. </summary>
        [SerializeField]
        [Tooltip("The text component containing the tooltip")]
        private TextMeshProUGUI toolTipText;

        [Header("Settings")]
        /// <summary> The margin left around the tooltip text. </summary>
        [SerializeField]
        [Tooltip("The margin left around the tooltip text.")]
        private Vector4 margin;

        private void Start() {
            if (backgroundImage.sprite && backgroundImage.type != Image.Type.Sliced) {
                Debug.LogWarning("Consider using a sliced sprite as a background image as it will be rescaled.");
            }
        }

        private void Update() {
            backgroundImage.gameObject.SetActive(false);

            PointerEventData eventData = new PointerEventData(EventSystem.current) {
                position = Input.mousePosition
            };

            List<RaycastResult> results = new List<RaycastResult>();
            EventSystem.current.RaycastAll(eventData, results);

            if (results.Count == 0) {
                
                Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
                { // 2D
                    Vector3 positionOnZ0 = ray.origin - Camera.main.transform.position.z * ray.direction / ray.direction.z;
                    RaycastHit2D hit = Physics2D.Linecast(positionOnZ0, positionOnZ0);

                    if (hit.collider != null) {
                        results.Add(
                            new RaycastResult() {
                                gameObject = hit.collider.gameObject,
                                screenPosition = Input.mousePosition
                            });
                    }
                }

                { // 3D
                    if (Physics.Raycast(ray, out RaycastHit hit1)) {
                        results.Add(
                            new RaycastResult() {
                                gameObject = hit1.collider.gameObject,
                                screenPosition = Input.mousePosition
                            });
                    }
                }
            }


            (ToolTipInfo toolTipToDisplay, Vector2 screenPosition) = results
                .Select(r => (toolTip: r.gameObject.GetComponent<ToolTipInfo>(), r.screenPosition))
                .Where(t => t.toolTip && t.toolTip.ShowToolTip())
                .FirstOrDefault();



            if (toolTipToDisplay) {
                string text = toolTipToDisplay.GetToolTipText();
                (float width, float height) = toolTipToDisplay.CalculateSize(text, (margin.x, margin.y, margin.z, margin.w));
                float pivotX = screenPosition.x < Camera.main.pixelWidth / 2 ? 0f : 1f;
                float pivotY = screenPosition.y < Camera.main.pixelHeight / 2 ? 0f : 1f;

                toolTipText.text = text;
                backgroundImage.rectTransform.pivot = new Vector2(pivotX, pivotY);
                backgroundImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Horizontal, width);
                backgroundImage.rectTransform.SetSizeWithCurrentAnchors(RectTransform.Axis.Vertical, height);
                backgroundImage.rectTransform.SetPositionAndRotation(Input.mousePosition, Quaternion.identity);
                backgroundImage.color = toolTipToDisplay.GetColor();
                backgroundImage.gameObject.SetActive(true);
            }
        }
    }
}
