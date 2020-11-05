using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class respawn : MonoBehaviour {
	Vector2 placeForRespawn;
	Vector2 placeForSpawn;
	public static List<GameObject> knights = new List<GameObject>();
	public GameObject GO_knight;
	private GameObject GO_knight_clone;
	Connect_Characters connect = new Connect_Characters();
	void Start () {
		placeForRespawn = new Vector2(-86.6f, 18.8f);
		placeForSpawn = new Vector2(35.2f, 18.8f);
	}
	
	void FixedUpdate () {
		if (Hero.Health < 0) StartCoroutine(rs_hero());
		if (knights.Count < 2) StartCoroutine(rs_knight());
	}
	IEnumerator rs_knight()
	{
		yield return new WaitForSeconds(2);
		if (knights.Count < 2)
		{
			GO_knight_clone = Instantiate(GO_knight, new Vector2(Random.Range(placeForSpawn.x - 10, placeForSpawn.x + 30), placeForSpawn.y), Quaternion.identity);
			knights.Add(GO_knight_clone);
		}
	}
	IEnumerator rs_hero()
	{
		yield return new WaitForSeconds(2);
		if (Hero.Health < 0)
		{
			Hero.g.SetActive(true);
			Hero.rb_Hero.position = placeForRespawn;
			Hero.Health = connect.hero.Health;
		}
	}
}
