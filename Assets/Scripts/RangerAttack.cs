﻿using UnityEngine;
using System.Collections;

public class RangerAttack : MonoBehaviour {

	[SerializeField] private float range;
	[SerializeField] private float timeBetweenAttacks = 1f;
	[SerializeField] Transform fireLocation;

	private Animator anim;
	private GameObject player;
	private bool playerInRange;
	private EnemyHealth enemyHealth;
	private GameObject arrow;

	// Use this for initialization
	void Start () {
		arrow = GameManager.instance.Arrow;
		enemyHealth = GetComponent<EnemyHealth>();
		player = GameManager.instance.Player;
		anim = GetComponent<Animator>();
		StartCoroutine(attack());

	
	}
	
	// Update is called once per frame
	void Update () {
		if(Vector3.Distance(transform.position, player.transform.position) < range && enemyHealth.IsAlive){
			playerInRange = true;
			anim.SetBool("PlayerInRange", true);
			RotateTowards(player.transform);
		} else {
			playerInRange = false;
			anim.SetBool("PlayerInRange", false);
			
		}
	}

	IEnumerator attack(){
		if(playerInRange && !GameManager.instance.GameOver && enemyHealth.IsAlive){
			anim.Play("Attack");
			yield return new WaitForSeconds(timeBetweenAttacks);
		}
		yield return null;
		StartCoroutine(attack());
	}

	private void RotateTowards(Transform player){
		Vector3 direction = (player.position - transform.position).normalized;
		Quaternion lookRotation = Quaternion.LookRotation(direction);
		transform.rotation = Quaternion.Slerp(transform.rotation, lookRotation, Time.deltaTime * 10f);

	}

	public void FireArrow(){
		GameObject newArrow = Instantiate(arrow);
		newArrow.transform.position = fireLocation.position;
		newArrow.transform.rotation = transform.rotation;
		newArrow.GetComponent<Rigidbody>().velocity = transform.forward * 25f;

	}
}
