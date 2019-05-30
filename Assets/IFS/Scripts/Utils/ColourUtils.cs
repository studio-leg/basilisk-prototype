using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ColourUtils
{
    #region Harmonious

    public static List<Color> GetHarmonious(
        int colorCount,
        float offsetAngle1,
        float offsetAngle2,
        float rangeAngle0,
        float rangeAngle1,
        float rangeAngle2,
        float saturation,
        float luminance)
    {
        return ColourUtils.GetHarmonious(colorCount, offsetAngle1, offsetAngle2, rangeAngle0, rangeAngle1, rangeAngle2, saturation, luminance, Random.value * 360f);
    }

    public static List<Color> GetHarmonious(
       int colorCount,
       float offsetAngle1,
       float offsetAngle2,
       float rangeAngle0,
       float rangeAngle1,
       float rangeAngle2,
       float saturation,
       float luminance,
       float referenceAngle)
    {
        List<Color> colors = new List<Color>();
        for (int i = 0; i < colorCount; i++)
        {
            float randomAngle = Random.value * (rangeAngle0 + rangeAngle1 + rangeAngle2);
            if (randomAngle > rangeAngle0)
            {
                if (randomAngle < rangeAngle0 + rangeAngle1)
                {
                    randomAngle += offsetAngle1;
                }
                else
                {
                    randomAngle += offsetAngle2;
                }
            }
            var hslColor = Color.HSVToRGB(
                ((referenceAngle + randomAngle) / 360.0f) % 1.0f,
                saturation,
                luminance);
            colors.Add(hslColor);
        }
        return colors;
    }
    #endregion

    public static Color GetRandomHue(float s = 1f, float v = 1f)
    {
        float h = Random.value;
        var colour = Color.HSVToRGB(h, s, v, false);
        return colour;
    }

    public static Color GetRandomFromMix(Color mix)
    {
        float red = Random.value;
        float green = Random.value;
        float blue = Random.value;
        red = (red + mix.r) / 2;
        green = (green + mix.g) / 2;
        blue = (blue + mix.b) / 2;
        Color color = new Color(red, green, blue);
        return color;
    }
}