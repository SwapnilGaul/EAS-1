﻿using UnityEngine;
using System.Collections;
using UnityEngine.UI;
using UnityEngine.Assertions;

public class PlayerHealth : MonoBehaviour {

	[SerializeField] int startingHealth = 100;
	[SerializeField] float timeSinceLastHit = 2f;
	[SerializeField] Slider healthSlider;



	private float timer = 0f;
	private CharacterController characterController;
	private Animator anim;
	private int currentHealth;
	private AudioSource audioP;
	private ParticleSystem blood;

	public int CurrentHealth{
		get{
			return currentHealth;
			}
		set{
			if(value < 0)
				currentHealth = 0;
			else
				currentHealth = value;
		}


	}

	void Awake(){
		Assert.IsNotNull(healthSlider);
	}
	// Use this for initialization
	void Start () {
		anim = GetComponent<Animator>();
		characterController = GetComponent<CharacterController>();
		currentHealth = startingHealth;
		audioP = GetComponent<AudioSource>();
		blood = GetComponentInChildren<ParticleSystem>();
		
	}
	// Update is called once per frame
	void Update () {
		timer += Time.deltaTime;
		
	}

	void OnTriggerEnter (Collider other){
		if(timer >= timeSinceLastHit && !GameManager.instance.GameOver){
			if(other.tag == "Weapon"){
				takeHit();
				timer = 0;
			}
		}
	}
	void takeHit(){
		if(currentHealth > 0){
			GameManager.instance.PlayerHit(currentHealth);
			anim.Play("Hurt");
			currentHealth -= 10;
			healthSlider.value = currentHealth;
			audioP.PlayOneShot(audioP.clip);
			blood.Play();
		}

		if(currentHealth <= 0){
			killPlayer();
		}
	}

	void killPlayer(){
		GameManager.instance.PlayerHit(currentHealth);
		anim.SetTrigger("HeroDie");
		anim.Play("Die");
		characterController.enabled = false;
		audioP.PlayOneShot(audioP.clip);
		blood.Play();
	}

	public void PowerUpHealth(){
		if(currentHealth <= 70){
			currentHealth += 30;
		} else if(currentHealth < startingHealth){
			currentHealth = startingHealth;
		}
		healthSlider.value = currentHealth;
	}
}













