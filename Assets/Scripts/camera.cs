using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class camera : MonoBehaviour {

	private int lastX;

	private bool FaceRight = true;

	public float dumping = 1.5f;
	float StartSize;

	public Vector2 offset = new Vector2(2f, 1f);
	private Transform player;
	private Camera cam;
	private Vector3 target;

	void Start () {
		cam = GetComponent<Camera>();
		offsetCamera();
		StartSize = cam.orthographicSize;
	}
	void offsetCamera()
	{
		player = GameObject.FindGameObjectWithTag("Player").transform;
		lastX = Mathf.RoundToInt(player.position.x);
		if (FaceRight) transform.position = new Vector3(player.position.x + offset.x, player.position.y + offset.y,transform.position.z);
		else transform.position = new Vector3(player.position.x - offset.x, player.position.y - offset.y, transform.position.z);
	}
	void Update () {
		float V = Mathf.Sqrt(Mathf.Pow(Hero.rb_Hero.velocity.x, 2) + Mathf.Pow(Hero.rb_Hero.velocity.y, 2));
		int currentX = Mathf.RoundToInt(player.position.x);
		if (currentX > lastX) FaceRight = true;
		else if (currentX < lastX) FaceRight = false;
		lastX = Mathf.RoundToInt(player.position.x);

		if (!FaceRight) target = new Vector3(player.position.x - offset.x, player.position.y - offset.y, transform.position.z);
		else target = new Vector3(player.position.x + offset.x, player.position.y + offset.y, transform.position.z);
		if (V >= 4 && cam.orthographicSize < 30) cam.orthographicSize += (100 + V) / 2000;
		else if (cam.orthographicSize > StartSize) cam.orthographicSize -= (100 + V) / 2000;
		transform.position = Vector3.Lerp(transform.position, target, dumping * (float)0.075);

		
	}
}
