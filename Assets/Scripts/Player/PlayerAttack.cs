﻿using UnityEngine;

public class PlayerAttack : MonoBehaviour
{
	private Enemy enemyHit;
	private float effectsDisplayTime = .1f, timer;
	private int shootableMask;
	private Ray ray = new Ray();
	private RaycastHit hitInfo;
	public GameObject gunEnd;
	//Visualize line entries 0-4 as left to right (far left, near left, center, near right, far right)
	public LineRenderer[] lines = new LineRenderer[5];
	public static bool spread;
	public static int damagePerShot = 20;
	public static float fireRate = 2f, range = 25f;

	private void Start () 
	{
		shootableMask = LayerMask.GetMask ("Shootable");
	}

	private void Update ()
	{
		timer += Time.deltaTime;

		//Checks for the mouse button to be held down, cooldown has been reached, and that the game isn't paused.
		if (Input.GetButton ("Fire1") && timer >= (1 / fireRate) && !UIManager.pauseMenu.activeSelf) {
			timer = 0f;

			//Regardless of whether spread shot is active or not, the center line will always shoot.
			lines[2].enabled = true;
			lines[2].SetPosition (0, gunEnd.transform.position);
			ray.origin = gunEnd.transform.position;
			ray.direction = transform.forward;

			Shoot (ray, lines[2]);

			if (spread) {
				//If spread shot is active, fire the extra 4 shots as well. Angle() returns the ray adjusted by an angle.
				//Angle() also handles the setup for the line just like above.
				Shoot (Angle (ray, lines[0], -.25f), lines[0]);
				Shoot (Angle (ray, lines[1], -.5f), lines[1]);
				Shoot (Angle (ray, lines[3], .5f), lines[3]);
				Shoot (Angle (ray, lines[4], .25f), lines[4]);
			}
		}

		//Disable the gun line(s) when the cooldown (multiplied by the time set for how long to display the line) has been reached.
		if (timer >= (1 / fireRate) * effectsDisplayTime) {
			lines[0].enabled = false;
			lines[1].enabled = false;
			lines[2].enabled = false;
			lines[3].enabled = false;
			lines[4].enabled = false;
		}
	}

	private void Shoot(Ray ray, LineRenderer line)
	{
		//Physics.Raycast returns true if the ray hits a collider. Check if it hit an enemy (and deal damage if so), then draw the line accordingly.
		if (Physics.Raycast (ray, out hitInfo, range, shootableMask)) {
			enemyHit = hitInfo.collider.GetComponent<Enemy> ();
			if (enemyHit != null) {
				enemyHit.TakeDamage (damagePerShot);
				UIManager.UpdateEnemy (enemyHit);
			}
			line.SetPosition (1, hitInfo.point);
		}else{
			line.SetPosition (1, ray.origin + ray.direction * range);
		}
	}

	private Ray Angle(Ray ray, LineRenderer line, float angleAdjust)
	{
		line.enabled = true;
		line.SetPosition (0, gunEnd.transform.position);
		ray.direction = transform.forward + (transform.right * angleAdjust);
		return ray;
	}
}
