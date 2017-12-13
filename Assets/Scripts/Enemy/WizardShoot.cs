﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class WizardShoot : MonoBehaviour {

	private GameObject player;
	private Rigidbody rigidBody;
	private float speed = 20;

	private void Start()
	{
		player = GameObject.Find ("Player");
		rigidBody = GetComponent<Rigidbody> ();
		transform.LookAt (player.transform);
		Destroy (gameObject, 2.5f);
	}

	private void Update()
	{
		rigidBody.AddForce (transform.forward * speed);
	}

	private void OnCollisionEnter(Collision other)
	{
		if (other.gameObject.tag == "StopShoot")
			Destroy (gameObject);
	}
}
