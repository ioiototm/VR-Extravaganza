using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DummyScript : MonoBehaviour
{

    public GameObject dummyPrefab;

    GameObject bullet;
    public GameObject bulletPrefab;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
      /*  if(activated)
        {
            if(bullet==null)
            {

                bullet = gameObject.GetComponent<ShootBullet>().FireTowards(GameObject.Find("VRCamera").transform);
            }
        }*/
    }


    bool activated = false;

    public void activateDummy()
    {
        activated = true;
    }

    public GameObject spawnRandomDummy()
    {

        int x = Random.Range(0, -18);

        //while(x>-2 && x<-11)
        //{
        //    x = Random.Range(0, -18);
        //}
        int z = Random.Range(0, -18);
        //while (z > -15 && z < -6)
        //{
        //    z = Random.Range(0, -18);
        //}
        //(x - center_x) ^ 2 + (y - center_y) ^ 2 < radius ^ 2

        while (Mathf.Pow(x + 5.16f, 2f) + Mathf.Pow(z + 11.6f, 2f) < Mathf.Pow(3, 2))
        {
            x = Random.Range(0, -18);
            z = Random.Range(0, -18);
        }


        GameObject t = Instantiate(dummyPrefab, new Vector3(x, 0, z), new Quaternion(0,0,0,0));
        //Instantiate(dummyPrefab,Transform)

        t.transform.LookAt(GameObject.Find("Player").transform);

        return t;

    }
}
