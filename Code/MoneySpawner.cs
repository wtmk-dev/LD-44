using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoneySpawner : MonoBehaviour, ISpawner {

	[SerializeField]
	private GameObject prefab;
	[SerializeField]
	private int maxPrefab;
	[SerializeField]
	private List<GameObject> spawnPoints;
	private System.Random rand;
	private int currentSpawned = 0;
	private Queue<GameObject> prefabPool;
	private Queue<GameObject> spawnedPool;

	// Use this for initialization
	void Start () 
	{	
		rand = new System.Random();
		spawnedPool = new Queue<GameObject>();
		BuildPrefabPool();
	}
	
	// Update is called once per frame
	void Update () 
	{
		
	}

	public void Spawn(int amount)
	{
		int amountToSpawn = amount - spawnedPool.Count;
		// Debug.Log(amountToSpawn);
		ActivatePrefabFromPool(amountToSpawn);
	}

	public void Kill(int amount)
	{
		DeactivatePrefabFromPool(amount);
	}

	private bool DeactivatePrefabFromPool(int amount)
	{	
		if(spawnedPool.Count > 0 && amount > 0)
		{
			GameObject go = spawnedPool.Dequeue();
			go.SetActive(false);
			prefabPool.Enqueue(go);
			amount -= 1;
			return DeactivatePrefabFromPool(amount);
		}
		else
		{
			return false;
		}
		
	}

	private bool ActivatePrefabFromPool(int amount)
	{
		if(prefabPool.Count > 0 && amount > 0)
		{
			GameObject go = prefabPool.Dequeue();
			Rigidbody2D rb2d = go.GetComponent<Rigidbody2D>();
			rb2d.velocity = new Vector2(0f,0f);
	
			if(spawnPoints.Count > 0)
			{
				Vector3 spawnPos = spawnPoints[rand.Next(0,spawnPoints.Count-1)].transform.position;
				go.transform.position = spawnPos;
			}
			else
			{
				go.transform.position = transform.position;
			}
			
			go.SetActive(true);
			spawnedPool.Enqueue(go);
			amount -= 1;
			return ActivatePrefabFromPool(amount);
		}
		else
		{
			return false;
		}
	}

	private void BuildPrefabPool()
	{
		prefabPool = new Queue<GameObject>();		
		for(int i = 0; i < maxPrefab; i++)
		{	
			GameObject go = Instantiate(prefab, transform.position,Quaternion.identity);
			go.SetActive(false);
			prefabPool.Enqueue(go);
		}
	}
}
