using System.Linq;
using System.Text.RegularExpressions;
using UnityEngine;

namespace Assets.ToolTip {

    /// <summary>
    /// This is the base component attached to any object that should cause a tool tip to be shown.
    /// </summary>
    public abstract class ToolTipInfo : MonoBehaviour {

        /// <summary> The number of pixels per line in the tool tip text. </summary>
        private const int PIXELS_PER_LINE = 20;
        /// <summary> The pixels per char in the longest line.  </summary>
        private const int PIXELS_PER_CHAR = 12;

        /// <summary>
        /// <p> This function creates the text, that is shown in the tool tip.</p>
        /// <p> Can have rich text elements. </p>
        /// <p> This method will be called to get the tool tip content including style elements. <br/>
        /// Example return: <br/>
        /// <code>
        /// "This is text including a\n<br/>
        /// new line.\n<br/>
        /// &lt;b&gt;This is bolt text&lt;/b&gt;\n<br/>
        /// &lt;i&gt;This is italic text&lt;/i&gt;\n<br/>
        /// &lt;color=#FF0000&gt;This is red text.&lt;/color&gt;"
        /// </code>
        /// </p>
        /// </summary>
        /// <returns> The tool tip box content. </returns>
        public abstract string GetToolTipText();

        /// <summary>
        /// Is there any data, that is to be displayed in the tool tip.
        /// This will be called, when the mouse is over the <see cref="ToolTipInfo"/>.<br/>
        /// returns <b><see cref="true"/></b>, if the tool tip should be displayed and <b><see cref="false"/></b>, if not.
        /// </summary>
        /// <returns> true, if a tool tip is to be shown, otherwise false. </returns>
        public abstract bool ShowToolTip();

        /// <summary>
        /// Gives the color of the base dependent on what is to be shown.<br/>
        /// This color will be used to change the color of the background of the tooltip.<br/>
        /// If the background should be in the usual sprite color return <see cref="Color.white"/>
        /// </summary>
        /// <returns> The evaluated color.</returns>
        public abstract Color GetColor();

        /// <summary>
        /// Calculates the size of the given tool tip text.
        /// </summary>
        /// <param name="text"> The text, that is to be Calculated. </param>
        /// <param name="margin"> The margin left on either side of the text. </param>
        /// <returns> The tuple of the size (width, height) </returns>
        public (float width, float height) CalculateSize(string text, (float top, float right, float bottom, float left) margin) {
            string[] lines = text.Split('\n');

            float height = lines.Length * PIXELS_PER_LINE + margin.top + margin.bottom;
            float width = lines
                .Select(s => Regex.Replace(s, "</?.*?/?>", ""))
                .Max(s => s.Length) * PIXELS_PER_CHAR + margin.left + margin.right;
            return (width, height);
        }
    }
}
