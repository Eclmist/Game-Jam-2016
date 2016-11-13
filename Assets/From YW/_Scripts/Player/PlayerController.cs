using UnityEngine;
using System.Collections;

public enum PlayerEnum
{
	Player1 = 0,
	Player2 = 1,
	Null = 2
}

public class PlayerController : MonoBehaviour
{
	public float Speed = 8, Health = 1, InvlunTimer = 0.2f;
	//public float  RotSpeed = 300;
	public PlayerEnum playerType = PlayerEnum.Null;

	public float vertical, horizontal, invlunCounter;
	private GameObject aimer;

    float starttime;

    public GameObject crystal;

    protected void Start ()
	{
		aimer = Instantiate (GameManager.instance.AimerPrefab);
		aimer.GetComponent<AimerController> ().Init (gameObject, playerType);
		GameManager.instance.Players [(int)playerType] = gameObject;
        Grid.Instance.ApplyForce(2, transform.position, 3, ForceType.Explosive);
        starttime = Time.time;

    }

    void Enable()
    {
        //Do spawning animat

    }

    protected void FixedUpdate ()
	{
		if (playerType != PlayerEnum.Null) {
			vertical = Input.GetAxisRaw ("ShipVertical" + playerType.ToString ());
			horizontal = Input.GetAxisRaw ("ShipHorizontal" + playerType.ToString ());
			Movement ();
		}
	}

	protected void OnDestroy ()
	{
		Destroy (aimer);
	}

    public void TakeDamage()
    {
        if (Time.time - starttime > invlunCounter)
        {
            starttime = Time.time;
            GameManager.instance.PlayerDied(playerType);
            gameObject.SetActive(false);
            Grid.Instance.ApplyForce(2, transform.position, 3, ForceType.Explosive);

            float count = Mathf.Clamp(GameManager.instance.Score / 10, 0, 10);

            for (int i = 0; i < count; i++)
            {
                Instantiate(crystal, transform.position, Quaternion.identity);
                GameManager.instance.Score -= 10;
            }

            AudioManager.PlayExplosion(0.6F);

            Destroy(gameObject);

        }
    }

	private void Movement ()
	{
		Vector3 finalPos = transform.position + new Vector3 (horizontal, 0, vertical).normalized * Time.fixedDeltaTime * Speed;
		transform.position = GameManager.ForceWithInBonds (finalPos);

		/*
        transform.Rotate(new Vector3(0, horizontal * RotSpeed * Time.fixedDeltaTime, 0));
        transform.position += transform.forward * vertical * Speed * Time.fixedDeltaTime;
        */
	}

	public void AssignAimer (GameObject aimer)
	{
		this.aimer = aimer;
	}

    public void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Enemy"))
        {
            TakeDamage();
        }
    }
}
