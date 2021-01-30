using System.Linq;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class ExtensionMethods
{
    public static string ToDisplay(this int value) {
    	return value == 0 ? "0" : value.ToString("##,#");
    }

    public static string ToDisplay(this float value) {
        return Mathf.Approximately(value, 0) ? "0" : value.ToString("##,#");
    }

    public static float Map(this float x, float x1, float x2, float y1,  float y2) {
		var m = (y2 - y1) / (x2 - x1);
		var c = y1 - m * x1;
		return m * x + c;
    }

    public static float RoundToInterval(this float f, float interval) {
        return Mathf.Round(f / interval) * interval;
    }

    public static int RoundToInterval(this int i, int interval) {
        return Mathf.RoundToInt(Mathf.Round(i / interval) * interval);
    }

    public static float CeilToInterval(this float f, float interval) {
        return Mathf.Ceil(f / interval) * interval;
    }

    public static float Min(this float f, float min) {
        return Mathf.Max(f, min);
    }

    public static T RandomItem<T>(this T[] array) where T: class {
        if (array.Length == 0) return null;
    	return array[Random.Range(0, array.Length)];
    }

    public static T RandomItem<T>(this List<T> list) where T: class {
        if (list.Count == 0) return null;
        return list.ElementAt(Random.Range(0, list.Count));
    }
}
