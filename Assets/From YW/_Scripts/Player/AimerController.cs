using UnityEngine;
using System.Collections;

public class AimerController : MonoBehaviour
{
	public float BulletPerSecond = 5 /*, RotSpeed = 200*/;
	public static GameObject[] BulletTier;

	[SerializeField] private GameObject bulletPrefab;
	private float timerBetweenShots, /*fire*/ horizontal, vertical;
	private float lastHori = 0, lastVerti = 1;
	private float horiTimer, vertiTimer;
	private bool horiChecker, vertiChecker;
	private GameObject player, shootingPoint;
	private PlayerEnum playerType;

	public void Init (GameObject Player, PlayerEnum PlayerType)
	{
		BulletTier = Resources.LoadAll <GameObject> ("BulletPrefab");
		player = Player;
		shootingPoint = gameObject.transform.GetChild (0).gameObject;
		playerType = PlayerType;
	}

	protected void FixedUpdate ()
	{
		if (playerType != PlayerEnum.Null) {			
			horizontal = Input.GetAxis ("AimHorizontal" + playerType.ToString ());
			vertical = Input.GetAxis ("AimVertical" + playerType.ToString ());
			//fire = Input.GetAxis ("Fire" + playerType.ToString ());

			if (horizontal > 0 || horizontal < 0 || horiTimer > 0.03f) {
				vertiTimer += Time.fixedDeltaTime;
				horiTimer = 0;
				lastHori = horizontal;
			}

			if (vertical > 0 || vertical < 0 || vertiTimer > 0.03f) {
				horiTimer += Time.fixedDeltaTime;
				vertiTimer = 0;
				lastVerti = vertical;
			}

			Position ();
		} else {
			playerType = player.GetComponent<PlayerController> ().playerType;
		}
	}

	protected void LateUpdate ()
	{
		transform.position = player.transform.position;
	}

	private void Position ()
	{
		shootingPoint.transform.localPosition = new Vector3 (lastHori, 0, lastVerti).normalized;
		CheckAttack ();
	}

	private void CheckAttack ()
	{
		timerBetweenShots += Time.fixedDeltaTime;

		if (horizontal < 0 || horizontal > 0 || vertical < 0 || vertical > 0) {
			Attack ();
		}
	}

	private void Attack ()
	{
		if (timerBetweenShots > 1 / BulletPerSecond) {
			timerBetweenShots = 0;

            AudioManager.PlayShoot();
			GameObject bullet = Instantiate (BulletTier [GameManager.GetBulletTier (playerType)]);
			bullet.transform.position = transform.position;
			bullet.transform.LookAt (shootingPoint.transform.position);
			bullet.transform.DetachChildren ();
			Destroy (bullet);
		}
	}
}
