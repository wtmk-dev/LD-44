using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
public class Main : MonoBehaviour 
{
	[SerializeField]
	private List<AdjustableText> UiTexts;
	private Dictionary<string,AdjustableText> adjustableTextDict;
	[SerializeField]
	private List<PlayerInteraction> PlayerInteractions;
	[SerializeField]
	private GameObject particleSystemPrefab, gameScreenGo;
	private ParticleSystem particleSystem, endgameParticles, gambleParticles;
	[SerializeField]
	private MoneySpawner moneySpawner, endGameSpawner;
	[SerializeField]
	private GameObject loveGo;

	[Range (0f, 86400f)]
	[SerializeField]
	private float triggerTime;
	[Range (0f, 86400f)]
	[SerializeField]
	private int costToUpgrade, costToUpgradePayPeriod, unlockGambleCost, costToQuit, costToUnlockLove;
	private SomeTimer someTimer;
	private bool isTimerRunning = false;
	private Player player;

	private System.Random rand;

	void Start()
	{
		foreach(PlayerInteraction playerInteraction in PlayerInteractions )
		{
			playerInteraction.OnPlayerInteracting += ResolveInteraction;
		}
	}

	private void Init()
	{
		rand = new System.Random();
		adjustableTextDict = SetAdjustableTextDict(UiTexts);
		GameObject particleSystemGo = Instantiate(particleSystemPrefab);
		particleSystem = particleSystemGo.GetComponent<ParticleSystem>();
		someTimer = new SomeTimer(triggerTime);
		player = new Player(costToUpgrade,costToUpgradePayPeriod,unlockGambleCost, costToQuit, costToUnlockLove);

		adjustableTextDict["UpgradeM"].SetText("$"+player.CostToNextUpgrade+"\n Increase money level");
		adjustableTextDict["UpgradeR"].SetText("$"+player.CostToUpgradePayPeriod+"\n Decreese time between payment");
		adjustableTextDict["Gamble"].SetText("$"+player.UnlockGambleCost+"\n Unlock gamble!");
		adjustableTextDict["Love"].SetText("$"+player.CostToUnlockLove+"\n Unlock Love!");

		UpdateStats();
		UpdateCostToQuit();
		gameScreenGo.SetActive(true);
		isTimerRunning = true;

	}

	private Dictionary<string,AdjustableText> SetAdjustableTextDict(List<AdjustableText> adjTxts)
	{
		Dictionary<string,AdjustableText> dict = new Dictionary<string,AdjustableText>();

		foreach (var adj in adjTxts)
		{
			dict.Add(adj.GetName(),adj);	
		}

		return dict;
	}

	void Update()
	{
		if( someTimer != null && isTimerRunning )
		{
			someTimer.RecordTime(Time.fixedDeltaTime);

			if( someTimer.HasTimeElapsedTrigger() )
			{
				player.IncreaseScore();

				if(player.LoveUnlocked)
				{
					particleSystem.Play();
				}
				
				SpawnMoney(player.Score);
				UpdatePlayerScore(player.Score);
			}
		}
	}

	private void UpdatePlayerScore(int score)
	{
		var payoutString = string.Format("$$$: {0}", score);
		adjustableTextDict["Score"].SetText(payoutString);
		// Debug.Log(payoutString);
	}

	private void UpdateGambleCost()
	{
		player.GambleCost = rand.Next(player.GambleLvl, player.GambleLvl * 10);
		var gambleString = string.Format("$"+player.GambleCost+"\nDouble of nothing?");
		adjustableTextDict["Gamble"].SetText(gambleString);
	}

	private void UpdateCostToQuit()
	{
		adjustableTextDict["Quit"].SetText("$"+player.CostToQuit+"\n Quit?");
	}

	private void UpdateStats()
	{
		adjustableTextDict["PayTimer"].SetText("You get paid "+player.MoneyLevel+"$ every " +someTimer.triggerTime +" seconds");
	}

	private void UpdateUpgradeText()
	{

	}

	private void ResolveInteraction(int iName)
	{
		switch(iName)
		{
			case 0:
			AtemptToUpgradeMoneyLevel();
			break;
			case 1:
			AttemptToUpgradePayPeriod();
			break;
			case 2:
			AtemptToGamble();
			break;
			case 3:
			Init();
			break;
			case 5:
			AtemptToQuit();
			break;
			case 7:
			AtemptToUnlockLove();
			break;
			default:
			break;
		}
	}

	private void AtemptToUpgradeMoneyLevel()
	{
		if(player.Score >= player.CostToNextUpgrade)
		{
			player.Score -= player.CostToNextUpgrade;
			moneySpawner.Kill(player.CostToNextUpgrade);
			player.CostToNextUpgrade *= 2;
			player.MoneyLevel++;

			adjustableTextDict["UpgradeM"].SetText("$"+player.CostToNextUpgrade+"\n Increase money level");
			UpdatePlayerScore(player.Score);
			UpdateStats();
		}
	}

	private void AttemptToUpgradePayPeriod()
	{
		if(player.Score >= player.CostToUpgradePayPeriod)
		{
			player.Score -= player.CostToUpgradePayPeriod;
			moneySpawner.Kill(player.CostToUpgradePayPeriod);
			player.CostToUpgradePayPeriod *= 2;
			player.PayPeriodLevel++;
			someTimer.LowerTriggerTime(player.PayPeriodLevel);

			adjustableTextDict["UpgradeR"].SetText("$"+player.CostToUpgradePayPeriod+"\n Decreese time between payment");
			UpdatePlayerScore(player.Score);
			UpdateStats();
		}
	}

	private void AtemptToGamble()
	{
		if(player.GambleLvl > 0 && player.Score >= player.UnlockGambleCost)
		{
			MakePayment(player.GambleCost);
			//playeffect and sounds
			int roll = rand.Next(1,100);
			int check = 60 - player.GambleLvl;
			if( roll > check )
			{
				player.GambleLvl++;
				UpdatePlayerScore(player.GambleCost*2);
				SpawnMoney(player.Score);
				//show someting like winner!
			}
			UpdateGambleCost();
		}
		else if(player.Score >= player.UnlockGambleCost)
		{
			MakePayment(player.UnlockGambleCost);
			player.GambleLvl++;
			UpdateGambleCost();
		}
	}

	private void AtemptToQuit()
	{
		if(player.Score >= player.CostToQuit)
		{
			MakePayment(player.CostToQuit);
			isTimerRunning = false;
			//endgameParticles.Play();
			//endGameSpawner
			ScreenSystem exit = GetComponent<ScreenSystem>();
			exit.EndGame(player.Score);

		}
	}

	private void AtemptToUnlockLove()
	{
		if(player.Score >= player.CostToUnlockLove)
		{
			MakePayment(player.CostToUnlockLove);
			player.LoveUnlocked = true;
			loveGo.SetActive(false);
		}
	}

	private void MakePayment(int payment)
	{
		player.Score -= payment;
		moneySpawner.Kill(payment);
		UpdatePlayerScore(player.Score);
	}

	private void SpawnMoney(int score)
	{
		moneySpawner.Spawn(score);
	}
}

