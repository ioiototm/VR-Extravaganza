using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallsLiftUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AudioClip crane;
    public AudioSource craneSource;

    public void startGoingUp()
    {
        craneSource.clip = crane;
        craneSource.Play();

        StartCoroutine(tableLift(1));
    }

    public void startGoingDown()
    {
        craneSource.clip = crane;
        craneSource.Play();

        StartCoroutine(tableLift(-1));
    }



    IEnumerator tableLift(int direction)
    {

        for (float i = 0f; i < 3f; i += Time.deltaTime)
        {
            transform.Translate(transform.up * direction * Time.deltaTime / 3f*1.3f);

            yield return null;
        }


    }

    /*
     * using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class wallsLiftUp : MonoBehaviour
{
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public AudioClip crane;
    public AudioSource craneSource;

    public void startGoingUp()
    {
        craneSource.clip = crane;
        craneSource.Play();

        StartCoroutine(tableLift(1));
    }

    public void startGoingDown()
    {
        craneSource.clip = crane;
        craneSource.Play();

        StartCoroutine(tableLift(-1));
    }

     IEnumerator tableLift(int direction)
    {
        
        for (float i = 0f;i<3f;i+=Time.deltaTime)
        {
            transform.Translate(transform.up*direction*Time.deltaTime/3f);

            yield return null;
        }

        
    }
}

     * 
     * */
}
