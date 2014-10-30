using UnityEngine;
using System.Collections;

public class GlobalVars : MonoBehaviour {
	public static float goalXPosition = 6100;
	public static int[] playerCharacters = new int[4];
	public static int[] playerClasses = new int[4];
	public static int numberOfPlayers = 0;
	public static bool[] playersJoined = new bool[4];
	public static Color blueColor = new Color(0, 0.357f, 0.82f);
	public static Color redColor = new Color(0.769f, 0, 0);
	public static Color yellowColor = new Color(0.91f, 0.91f, 0);
	public static Color greenColor = new Color(0.027f, 0.82f, 0);
	public static bool gameOver = false;
	public static Color IntToColor(int i) {
		if(i == 0) {
			return GlobalVars.redColor;
		} else if(i == 1) {
			return GlobalVars.blueColor;
		} else if(i == 2) {
			return GlobalVars.greenColor;
		}
		return GlobalVars.yellowColor;
	}
}
