using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class FindEnemy : MonoBehaviour
{

	// Use this for initialization
	private Vector3 posHero;
	public Animator anim;
	public Text text;
	const float R = 49;
	const float G = 77;
	const float B = 121;
	float r = R;
	float g = G;
	float b = B;

	float r_Enemy = 31;
	float g_Enemy = 42;
	float b_Enemy = 58;

	float CloseDistance;

	GameObject Enemy;

	void Start()
	{
		//Te//xt.GetComponent<Text>();
		anim.GetComponent<Animator>();
		CloseDistance = Mathf.Infinity;
		text.enabled = false;
		text.transform.position = new Vector2(Hero.rb_Hero.position.x, Hero.rb_Hero.position.y + 5);
	}
	void Update()
	{
		posHero = Hero.rb_Hero.position;
		Find();
		if (Physics2D.OverlapCircle(posHero, 120, 1 << LayerMask.NameToLayer("friendly(enemy)")))
		{
			anim.SetBool("AroundEnemy", true);
			if (r > r_Enemy)
			{
				r--;
				g -= (G - g_Enemy) / (R - r_Enemy);
				b -= (B - b_Enemy) / (R - r_Enemy);
				Camera.main.GetComponent<Camera>().backgroundColor = new Color32((byte)r, (byte)g, (byte)b, 255);
			}
		}
		else
		{
			if (r < R)
			{
				r++;
				g += (G - g_Enemy) / (R - r_Enemy);
				b += (B - b_Enemy) / (R - r_Enemy);
				Camera.main.GetComponent<Camera>().backgroundColor = new Color32((byte)r, (byte)g, (byte)b, 255);
			}
			anim.SetBool("AroundEnemy", false);
		}
	}
	void Find()
	{
		if (Input.GetKey(KeyCode.LeftControl))
		{
			if (GameObject.FindGameObjectWithTag("enemy") != null)
			{
				GameObject [] enemy = GameObject.FindGameObjectsWithTag("enemy");
				if (Enemy != null)
				{
					foreach (GameObject go in enemy)
					{
						float dist = Vector2.Distance(Enemy.transform.position, posHero);
						float dist_go = Vector2.Distance(go.transform.position, posHero);
						if (dist_go < dist) Enemy = go;
					}
					float distance = Vector2.Distance(Enemy.transform.position, posHero);
					if (distance < CloseDistance)
					{
						//CloseDistance = distance;
						text.text = ((int)distance).ToString();
					}
				}
				else
				{
					foreach (GameObject go in enemy)
					{
						Enemy = go;
						break;
					}
				}
			}
			else text.text = "Враги не обнаружены";
			text.enabled = true;

		}
		else text.enabled = false;

	}
}
