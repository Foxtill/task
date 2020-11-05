using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Hero : MonoBehaviour {
	Connect_Characters connect = new Connect_Characters();
	Wind wind = new Wind();
	public static Rigidbody2D rb_Hero;
	public SpriteRenderer sp_Hero;
	private Animator anim;
	public GameObject Wind;
	private GameObject CloneWind;


	public static GameObject g;

	private int extraJump = 1;
	private bool WindRight;
	private bool isGround;
	private bool cooldown;

	private float cooldownTime = 2;
	const float JumpForce = 60;
	public static float Health;
	private float speed;

	List<Collider2D> GroundColliders = new List<Collider2D>();
	public static List<GameObject> WindCollections = new List<GameObject>();

	void Awake()
	{
		rb_Hero = GetComponent<Rigidbody2D>();
		sp_Hero = GetComponent<SpriteRenderer>();
		anim = GetComponent<Animator>();
		Health = connect.hero.Health;
	}
	void Start()
	{
		g = this.gameObject;
	}
	void Update() {
		if (Health <= 0) this.gameObject.SetActive(false);
		isGround = GroundColliders.Count > 0 ? true : false;
		if (isGround) extraJump = 1;
		Flip();
	}
	void FixedUpdate()
	{
		Run();
		Jump();
		Attack();
	}
	void Attack()
	{
		if (Input.GetKeyDown(KeyCode.Mouse0))
		{
			if (!cooldown)
			{
				if (!sp_Hero.flipX)
				{
					CloneWind = Instantiate(Wind, new Vector2(transform.position.x + 7, transform.position.y), Quaternion.identity);
					WindRight = true;
				}
				else
				{
					CloneWind = Instantiate(Wind, new Vector2(transform.position.x - 7, transform.position.y), Quaternion.identity);
					WindRight = false;
				}
				WindCollections.Add(Wind);
				StartCoroutine(Cooldown());
				StartCoroutine(AnimAttack());
			}
		}
		if (WindCollections.Count != 0) WindAttack();
	}
	private IEnumerator Cooldown()
	{
		cooldown = true;
		yield return new WaitForSeconds(cooldownTime);
		WindCollections.Clear();
		Destroy(CloneWind);
		cooldown = false;
	}
	void Run()
	{
		speed = Input.GetAxis("Horizontal");
		rb_Hero.velocity = new Vector2(connect.hero.Speed * speed, rb_Hero.velocity.y);
		anim.SetFloat("speed", Mathf.Abs(speed));
	}
	void Flip()
	{
		if		(speed < 0) sp_Hero.flipX = true;
		else if (speed > 0) sp_Hero.flipX = false;
	}
	void WindAttack()
	{
		if (WindRight) CloneWind.transform.position = Vector3.Lerp(CloneWind.transform.position, new Vector2(CloneWind.transform.position.x + wind.radius[connect.hero.id], CloneWind.transform.position.y), wind.speed[connect.hero.id]);
		else		   CloneWind.transform.position = Vector3.Lerp(CloneWind.transform.position, new Vector2(CloneWind.transform.position.x - wind.radius[connect.hero.id], CloneWind.transform.position.y), wind.speed[connect.hero.id]);	
	}
	void Jump()
	{
		if (Input.GetKeyDown(KeyCode.Space) && extraJump > 0)
		{
			rb_Hero.velocity = Vector2.up * JumpForce;
			extraJump--;
		}
		anim.SetFloat("Jump", Mathf.Abs(rb_Hero.velocity.y));
	}
	void OnCollisionStay2D(Collision2D coll)
	{
		if (!GroundColliders.Contains(coll.collider))
		{
			foreach (var p in coll.contacts)
			{
				if (p.point.y < GetComponent<Collider2D>().bounds.min.y)
				{
					GroundColliders.Add(coll.collider);
					break;
				}
			}
		}
	}

	void OnCollisionExit2D(Collision2D coll)
	{
		GroundColliders.Clear();
	}
	IEnumerator AnimAttack()
	{
		anim.SetBool("Attack", true);
		yield return new WaitForSeconds(0.5f);
		anim.SetBool("Attack", false);
	}
}	
