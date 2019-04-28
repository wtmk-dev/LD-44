using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScreenSystem : MonoBehaviour {

	[SerializeField]
	private List<PlayerInteraction> playerInteractions;
	[SerializeField] 
	private List<GameObject> screens;
	[SerializeField]
	private List<AdjustableText> UiTexts;

	void Awake()
	{
		foreach(PlayerInteraction playerInteraction in playerInteractions )
		{
			playerInteraction.OnPlayerInteracting += ResolveInteraction;
		}

		screens[2].SetActive(false);
	}

	private void ResolveInteraction(int iName)
	{
		switch(iName)
		{
			case 3:
			StartGame();
			break;
			case 4:
			//AttemptToUpgradePayPeriod();
			break;
			default:
			break;
		}
	}

	private void StartGame()
	{
		screens[0].SetActive(false);
	}

	public void EndGame(int score)
	{
		Debug.Log("this got called");
		screens[1].SetActive(false);
		screens[2].SetActive(true);
		UiTexts[0].SetText("You walked awake with $: " +score);

	}

}
