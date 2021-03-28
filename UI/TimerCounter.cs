using UnityEngine;
using UnityEngine.UI;
using System;
using System.Collections;
using TMPro;

public class TimerCounter : MonoBehaviour {
	public bool isOn;
	public bool isDead;
	public int level;

	public float timeGoing;
	public float overallTimeGoing;

	private string[] stringsFrom00To99 = {
		"00", "01", "02", "03", "04", "05", "06", "07", "08", "09",
		"10", "11", "12", "13", "14", "15", "16", "17", "18", "19",
		"20", "21", "22", "23", "24", "25", "26", "27", "28", "29",
		"30", "31", "32", "33", "34", "35", "36", "37", "38", "39",
		"40", "41", "42", "43", "44", "45", "46", "47", "48", "49",
		"50", "51", "52", "53", "54", "55", "56", "57", "58", "59",
		"60", "61", "62", "63", "64", "65", "66", "67", "68", "69",
		"70", "71", "72", "73", "74", "75", "76", "77", "78", "79",
		"80", "81", "82", "83", "84", "85", "86", "87", "88", "89",
		"90", "91", "92", "93", "94", "95", "96", "97", "98", "99"
	};

	public TMP_Text CounterMinutes;
	public TMP_Text CounterSeconds;
	public TMP_Text CounterHundredths;
	public GameObject CounterGameObject;

	void Update () {
		if (level != 3 || isDead)
			return;
		
		timeGoing += Time.deltaTime;
		if (timeGoing > 180.0f)
			AchievementActivation.UnlockAchievement ("NeedSomeCoffee");
		overallTimeGoing += Time.deltaTime;
		if (overallTimeGoing > 300.0f)
			Level.fiveMinsOn = true;
		
		if (isOn && level == 3)
			ConvertTime (timeGoing);
	}

	void ConvertTime (float unsortedTime) {
		var sortedTime = TimeSpan.FromSeconds(unsortedTime);

		CounterHundredths.text = stringsFrom00To99[sortedTime.Milliseconds / 10];
		CounterSeconds.text = stringsFrom00To99[sortedTime.Seconds];
		CounterMinutes.text = stringsFrom00To99[sortedTime.Minutes];
	}

	public void UpdateLevel(int newLevel) {
		level = newLevel;

		overallTimeGoing = 0;
		timeGoing = 0;

		if (level != 3)
			CounterGameObject.SetActive (false);
		else
			CounterGameObject.SetActive (isOn);
	}

	public void ResetTime() {
		timeGoing = 0;
	}

	public void SwitchDead(bool newValue) {
		if (!newValue)
			ResetTime ();

		isDead = newValue;
	}
}