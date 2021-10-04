using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HeadBob : MonoBehaviour
{
	[SerializeField]
	private float bobSpeed = 0.1f;
	[SerializeField]
	private float bobDistance = 0.1f;
	[SerializeField]
	private Transform cam;

	private float horizontal, vertical, timer, waveSlice;
	private Vector3 midPoint;

	private float baseSpeed;

	Player_Controller controller;
	public bool canBob = true;

	void Start()
	{
		midPoint = cam.localPosition;
		baseSpeed = bobSpeed;
		controller = GameObject.Find("Fps Character").GetComponent<Player_Controller>();
	}

	void Update()
	{
		horizontal = Input.GetAxis("Horizontal");
		vertical = Input.GetAxis("Vertical");

		Vector3 localPosition = cam.localPosition;

		if (Input.GetKey(KeyCode.LeftShift))
		{
			bobSpeed = (controller.sprintSpeed / controller.baseSpeed) * baseSpeed;
		}
		else
		{
			bobSpeed = baseSpeed;
		}

        if (canBob)
        {
			if (Mathf.Abs(horizontal) == 0 && Mathf.Abs(vertical) == 0)
			{
				timer = 0.0f;
			}
			else
			{
				waveSlice = Mathf.Sin(timer);
				timer = timer + bobSpeed;
				if (timer > Mathf.PI * 2)
				{
					timer = timer - (Mathf.PI * 2);
				}
			}
			if (waveSlice != 0)
			{
				float translateChange = waveSlice * bobDistance;
				float totalAxes = Mathf.Abs(horizontal) + Mathf.Abs(vertical);
				totalAxes = Mathf.Clamp(totalAxes, 0.0f, 1.0f);
				translateChange = totalAxes * translateChange;
				localPosition.y = midPoint.y + translateChange;
			}
			else
			{
				localPosition.y = midPoint.y;
			}

			cam.localPosition = localPosition;
		}

		
	}
}
