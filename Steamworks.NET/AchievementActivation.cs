using System.Collections;
using UnityEngine;
using Steamworks;

public class AchievementActivation : MonoBehaviour {

	void Start() {
		if(SteamManager.Initialized) {
			string name = SteamFriends.GetPersonaName();
			Debug.Log(name);
		}
	}

	public static void UnlockAchievement (string ID) {
		if (!SteamManager.Initialized)
			return;
		
		bool unlocked;
		SteamUserStats.GetAchievement (ID, out unlocked);
		if (!unlocked) {
			SteamUserStats.SetAchievement (ID);
			SteamUserStats.StoreStats ();
		}
	}
}
