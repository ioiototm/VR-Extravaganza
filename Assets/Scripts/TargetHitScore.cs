using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TargetHitScore : MonoBehaviour
{

    public Transform bullsEye;

    public ParticleSystem ps;


    public AudioClip swish;
    public AudioClip explosion;

    AudioSource src;
    // Start is called before the first frame update
    void Start()
    {
        src = GetComponent<AudioSource>();
            src.clip = swish;
        src.Play();
    }

    // Update is called once per frame
    void Update()
    {
        if(ps.isPlaying)
        ps.playbackSpeed = TimeInput.timeDilation;
    }

    private void OnTriggerEnter(Collider other)
    {

       
        other.gameObject.GetComponent<CapsuleCollider>().isTrigger = false;

        float dist = Vector3.Distance(other.gameObject.transform.position,bullsEye.position);

        if (dist > 0.4) { }

        float score = TimeInput.map(dist, 0.03f, 0.4f, 0, 100);
        score = Mathf.Max(score, 0);

        Debug.Log(100 - score);
        if(100-score<=0)
        {
            score = 100;
        }

        Story.Score += (100 - (int)score);

        //0.4 0.03

    }

    private void OnCollisionEnter(Collision collision)
    {
        Debug.Log("ENTERED" + collision.gameObject.name);
    }
    private void OnCollisionExit(Collision collision)
    {
        Debug.Log("Exit");
        if(!ps.isPlaying)
        ps.Play();

        if (collision.gameObject.CompareTag("Bullet"))
        {
            src.clip = explosion;
            src.Play();
            StartCoroutine(destroyAfterABit());
        }
    }

    IEnumerator destroyAfterABit()
    {
        
        yield return new WaitForSeconds(0.6f);

       
        
        Destroy(gameObject.transform.parent.parent.gameObject);

    }

}
