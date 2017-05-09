using System;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using System.Collections.Generic;

public class onclickInventory : MonoBehaviour, ISelectHandler
{

	public void onClickFunction()
	{
		FindObjectOfType<InventoryManager>().goToFocus(true);
	}

	public void OnSelect(BaseEventData eventData)
	{
		for(int i = 0; i < transform.parent.childCount;i++)
		{
			if(transform.parent.GetChild(i) == transform)
			{
			    FindObjectOfType<InventoryManager>().selected = i;
			}
		}
		FindObjectOfType<InventoryManager>().goToFocus(false);
	}
}
