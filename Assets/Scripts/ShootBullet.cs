using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Valve.VR;
using Valve.VR.InteractionSystem;

public class ShootBullet : MonoBehaviour
{

    public GameObject bulletPrefab;
    public Transform spawn;

    public AudioClip firePistol;
    public AudioSource source;

    public ParticleSystem ps;

    public GameObject spawnedBullet;

    
    
    // Start is called before the first frame update
    void Start()
    {
        
        //Time.timeScale = 0.1f;
        //Time.fixedDeltaTime = Time.timeScale * 0.02f;
        /*for (int handIndex = 0; handIndex < Player.instance.hands.Length; handIndex++)
        {
            Hand hand = Player.instance.hands[handIndex];

            if (hand != null)
            {
                Debug.Log(hand);
                hand.HideController(true);
            }
        }
        for (int handIndex = 0; handIndex < Player.instance.hands.Length; handIndex++)
        {
            Hand hand = Player.instance.hands[handIndex];
            if (hand != null)
            {
                hand.SetSkeletonRangeOfMotion(Valve.VR.EVRSkeletalMotionRange.WithoutController);
            }
        }*/

    }


    public bool activatedToShoot = false;

    Transform toShootAt;

    public bool canShoot = true;

    // Update is called once per frame
    void Update()
    {
       /* counter++;
        if(counter==100)
        {
            counter = 0;
            Fire();
        }
        if(Input.GetKeyDown(KeyCode.Space))
        {
            Fire();
        }
        */
        if(activatedToShoot)
        {
            if (spawnedBullet == null && canShoot == true)
            {
                StartCoroutine(waitBeforeShootRandomTime());
                canShoot = false;
            }
        }
        source.pitch = TimeInput.timeDilation;

        if(ps!=null)
        {
            ps.playbackSpeed = TimeInput.timeDilation;
        }
        
    }
    public void activateToShoot()
    {
        StartCoroutine(waitABitBeforeStartingToShoot());
        
       
    }

    IEnumerator waitABitBeforeStartingToShoot()
    {

        //yield return new WaitForSeconds(4f);

        activatedToShoot = true;
        yield return null;
    }

    public void Fire()
    {
        GameObject bullet = Instantiate(bulletPrefab);
        //Physics.IgnoreCollision(bullet.GetComponent<Collider>(), spawn.parent.GetComponent<Collider>());

        bullet.transform.position = spawn.position;
        Vector3 rotation = bullet.transform.rotation.eulerAngles;
        bullet.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y, rotation.z-90);

        bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.up* 10, ForceMode.Impulse);

        BulletScript bs = bullet.GetComponent<BulletScript>();
        bs.setVel(bullet.transform.up * 10);

        
        source.Play();
    }

    public void setTransformToFireTo(Transform target)
    {
        toShootAt = target;
    }


    IEnumerator waitBeforeShootRandomTime()
    {

        yield return new WaitForSeconds(Random.Range(2, 4));

        FireTowards(toShootAt);
    }

    public GameObject FireTowards(Transform target)
    {
        GameObject bullet = Instantiate(bulletPrefab);

        StopAllCoroutines();

        spawnedBullet = bullet;
        canShoot = true;

        toShootAt = target;
        //Physics.IgnoreCollision(bullet.GetComponent<Collider>(), spawn.parent.GetComponent<Collider>());

        bullet.transform.position = spawn.position;
        Vector3 rotation = bullet.transform.rotation.eulerAngles;
        bullet.transform.LookAt(target);
        //bullet.transform.rotation = Quaternion.Euler(rotation.x, transform.eulerAngles.y-90, rotation.z - 90);

       // bullet.GetComponent<Rigidbody>().AddForce(bullet.transform.up * 10, ForceMode.Impulse);

        BulletScript bs = bullet.GetComponent<BulletScript>();
        bs.setVel(bullet.transform.forward * 10);
        if(ps!=null)
        {
            ps.Play();
        }
        /* Vector3 targetDir = actor.transform.position - gunBarrel.transform.position;
                   Vector3 newDir = Vector3.RotateTowards(gunBarrel.transform.forward, targetDir, 0.09f, 0.0f);

                   //gunBarrel.transform.rotation = Quaternion.LookRotation(newDir);

                  /* gunBarrel.transform.eulerAngles = new Vector3(gunBarrel.transform.eulerAngles.x,
                     180f - gunBarrel.transform.eulerAngles.y,
                     gunBarrel.transform.eulerAngles.z);*/

        //gunBarrel.transform.LookAt(actor.transform);
        source.clip = firePistol;
        source.Play();

        if(!bs.firstBullet)
        StartCoroutine(DestroyInCase());
        


        return bullet;
    }

    IEnumerator DestroyInCase()
    {
        yield return new WaitForSeconds(20f);

        Destroy(spawnedBullet);
    }

}
