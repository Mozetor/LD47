using UnityEngine;

namespace Utils {

    public static class ColorUtils {
        public static Color ToNiceColor(Color c) {
            return new Color(ToNiceFloat(c.r), ToNiceFloat(c.g), ToNiceFloat(c.b), ToNiceFloat(c.a));
        }

        public static float ToNiceFloat(float f) {
            return Mathf.Floor(f * 100) / 100;
        }
    }
}