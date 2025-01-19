using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TrainingTableScript : MonoBehaviour
{

    public GameObject page1,page2,page3;
    public AudioClip crane;
    public AudioSource craneSource;
    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void startGoingUp()
    {
        craneSource.clip = crane;
        craneSource.Play();

        StartCoroutine(tableLift(-1));
    }

    public void startGoingDown()
    {
        craneSource.clip = crane;
        craneSource.Play();

        StartCoroutine(tableLift(1));
    }

   
    IEnumerator tableLift(int direction)
    {
        if (direction > 0)
        {
            Rigidbody rb = page1.GetComponent<Rigidbody>();
            rb.isKinematic = true;

            rb = page2.GetComponent<Rigidbody>();
            rb.isKinematic = true;

            rb = page3.GetComponent<Rigidbody>();
            rb.isKinematic = true;
        }

        for (float i = 0f;i<3f;i+=Time.deltaTime)
        {
            transform.Translate(transform.up*direction*Time.deltaTime/3f);

            yield return null;
        }

        if (direction < 0)
        {
            Rigidbody rb = page1.GetComponent<Rigidbody>();
            rb.isKinematic = false;

            rb = page2.GetComponent<Rigidbody>();
            rb.isKinematic = false;

            rb = page3.GetComponent<Rigidbody>();
            rb.isKinematic = false;
        }
        
    }
}
