using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerInteraction : MonoBehaviour, IPlayerInteraction
{
    public Action<int> OnPlayerInteracting;

    [SerializeField]
    public int iName;

    public void Execute()
    {
        if(OnPlayerInteracting != null)
        {
            OnPlayerInteracting(iName);
        }
    }

    public int GetName()
    {
        return iName;
    }
}