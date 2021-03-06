﻿using UnityEngine;
using System.Collections;
using System;
using System.Collections.Generic;

public class GameScriptController : MonoBehaviour {

	public enum CreatureStateEnum {
		appear,
		hello,
		quest,
		player,
		win,
		leave,
		totalWin
	}

	public GameObject[] Creatures;
	public int CurrentCreature;
	public CreatureStateEnum CreatureState;
	public int CreatureHealth;

	public UIPanel panel;

	public bool stepIsConfirmed = false;

	public float delayActions;
	public Action actionToDoAfterDelay;

	public Dictionary<CreatureStateEnum, string> messages;

	// Use this for initialization
	void Start () {

		messages = new Dictionary<CreatureStateEnum, string> {
			{ CreatureStateEnum.appear, "Привет!\nДавай поиграем." },
			{ CreatureStateEnum.hello, "Сегодня у нас звук \"Р\"!" },
		};


		if (Creatures == null) Creatures = new GameObject[0];
		CurrentCreature = 0;
		if (Creatures.Length > 0) {
			CreatureState = CreatureStateEnum.appear;
			CreatureHealth = 3;
		}

	}
	
	// Update is called once per frame
	void Update () {

		if (delayActions > 0) {
			delayActions -= Time.deltaTime;
			return;
		}

		//if (messages.ContainsKey (CreatureState) && !stepIsConfirmed) return;
		//stepIsConfirmed = false;

		if(actionToDoAfterDelay != null) actionToDoAfterDelay();

		var creature = Creatures [CurrentCreature];
		var animator = creature.GetComponent<Animator>();

		switch (CreatureState)
		{
		case CreatureStateEnum.appear:
			animator.SetInteger("state", 0);
			Delay(1.66F, () => CreatureState ++);
			break;
			
		case CreatureStateEnum.hello:
			animator.SetInteger("state", 1);
			Delay(1, () => CreatureState ++);
			break;

		case CreatureStateEnum.quest:
			animator.SetInteger("state", 2);
			Delay(5, () => CreatureState ++);
			break;

		case CreatureStateEnum.player:
			animator.SetInteger("state", 3);
			Delay(20, () => {
				if(CreatureHealth > 0) CreatureHealth -= 1;
				else {
					CreatureHealth = 3;
					CreatureState += 1;
				}
			});
			break;

		case CreatureStateEnum.win:
			animator.SetInteger("state", 4);
			Delay(2, () => CreatureState ++);
			break;

		case CreatureStateEnum.leave:
			animator.SetInteger("state", 5);
			Delay(2, () => {
				if(CurrentCreature < Creatures.Length - 1)
				{
					CurrentCreature += 1;
					CreatureState = 0;
				}
				else 
					CreatureState += 1;
			});
			break;

		case CreatureStateEnum.totalWin:
			// CHANGE SCENE HERE!!
			break;

		default:
			break;
		}
	}

	public void Delay(float secs, Action action)
	{
		delayActions = secs;
		actionToDoAfterDelay = action;
	}
}
