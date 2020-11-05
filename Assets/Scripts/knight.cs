using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class knight : MonoBehaviour {
	Connect_Characters connect = new Connect_Characters();
	Spells_Connect sc = new Spells_Connect();
	public Rigidbody2D rb_knight;
	public SpriteRenderer sp_knight;
	public Animator anim;
	private Transform player;
	private bool cooldown;
	private Transform wind;

	private List<GameObject> pp = new List<GameObject>();

	bool CheckPP2 = false;
	bool spy = false;
	void Awake() {
		rb_knight = GetComponent<Rigidbody2D>();
		sp_knight = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
	}
	void Start()
	{
		pp.AddRange(GameObject.FindGameObjectsWithTag("pp"));
		player = GameObject.FindGameObjectWithTag("Player").transform;
	}

	void Update() {
		if (spy == false) Patrol();
		if (connect.knight.Health < 0)
		{
			respawn.knights.Remove(gameObject);
			Destroy(gameObject);
		}
		if (Hero.WindCollections.Count != 0) wind = GameObject.FindGameObjectWithTag("wind").transform;
	}
	void FixedUpdate()
	{
		if (Hero.Health > 0)
		{
			Run();
			Attack();
		}
		else spy = false;
		Flip();

		anim.SetFloat("speed", Mathf.Abs(rb_knight.velocity.x));
	}
	void Run()
	{
		Vector2 directive = (player.position - transform.position).normalized;
		float range = (player.position - transform.position).magnitude;
		if (range > sc.punch.radius[connect.knight.id] + 1f && range < 100)
		{
			rb_knight.velocity = new Vector2(directive.x * connect.knight.Speed, rb_knight.velocity.y);
			spy = true;
		}
		else if (range >= 100) spy = false;
	}
	void Flip()
	{
		if (rb_knight.velocity.x > 0) sp_knight.flipX = false;
		else if (rb_knight.velocity.x < 0) sp_knight.flipX = true;
	}
	void Attack()
	{
		if ((player.position - transform.position).magnitude <= sc.punch.radius[connect.knight.id] && !cooldown)
		{
			StartCoroutine(AnimAttack());
			StartCoroutine(Cooldown());
			Hero.Health -= sc.punch.damage[connect.knight.id];
		}
	}
	IEnumerator Cooldown()
	{
		cooldown = true;
		yield return new WaitForSeconds(sc.punch.speed[connect.knight.id] - 2);
		cooldown = false;
	}
	IEnumerator AnimAttack()
	{
		anim.SetBool("Attack", true);
		yield return new WaitForSeconds(0.5f);
		anim.SetBool("Attack", false);
	}
	void OnCollisionEnter2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "wind") connect.knight.Health -= sc.wind.damage[connect.hero.id];
	}
	void OnCollisionStay2D(Collision2D coll)
	{
		if (coll.gameObject.tag == "wind") rb_knight.transform.position = Vector2.Lerp(rb_knight.position, new Vector2(wind.position.x, wind.position.y + 5), 0.3f);
	}
	void Patrol()
	{
		if (!CheckPP2)
		{
			rb_knight.velocity = new Vector2(-connect.knight.Speed, rb_knight.velocity.y);
			if ((pp[1].transform.position -rb_knight.transform.position).normalized.x > 0) CheckPP2 = true;
		}
		else if (CheckPP2)
		{
			rb_knight.velocity = new Vector2(connect.knight.Speed, rb_knight.velocity.y);
			if ((pp[0].transform.position - rb_knight.transform.position).normalized.x < 0) CheckPP2 = false;
		}
	}
}
