using UnityEngine;

public class PlayerShooting2 : MonoBehaviour
{
    Animator animator;

    public GameObject bullet;
    public GameObject shootPoint;
    public ParticleSystem muzzleEffect;
    public AudioSource shootSound;
    public int bulletsAmount;

    float lastShootTime;
    public float fireRate;

    void Update()
    {
        if (Input.GetKey(KeyCode.Mouse0) && bulletsAmount > 0 && Time.timeScale > 0)
        {
            animator.SetBool("Aiming", true);

            var timeSinceLastShoot = Time.time - lastShootTime;
            if (timeSinceLastShoot < fireRate)
                return;

            lastShootTime = Time.time;

            bulletsAmount--;
            muzzleEffect.Play();
            shootSound.Play();

            GameObject clone = Instantiate(bullet);
            clone.transform.position = shootPoint.transform.position;
            clone.transform.rotation = shootPoint.transform.rotation;
        }
         if (Input.GetKey(KeyCode.T) )
        {
             animator.SetBool("Aiming", false);
        }
    }

    void Awake()
    {
        animator = GetComponent<Animator>();
    }
}