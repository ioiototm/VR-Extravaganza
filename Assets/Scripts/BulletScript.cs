using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR.InteractionSystem;

public class BulletScript : MonoBehaviour
{

    public Rigidbody bodyOfBullet;
    public Vector3 vel;

    
    
    public Rigidbody rigidbody;
    public float timeScale;
    Transform transform;

    public Hand rightHand;

    public bool isBeingHeld = false;
    /*
    [SerializeField]
    private float _timeScale = 1;
    public float timeScale
    {
        get { return _timeScale; }
        set
        {
            value = Mathf.Abs(value);
            float tempTimeScaleApplied = value / timeScale;
            _timeScale = value;

            rigidbody.mass /= tempTimeScaleApplied;
            rigidbody.velocity *= tempTimeScaleApplied;
            rigidbody.angularVelocity *= tempTimeScaleApplied;
        }
    }

    void Awake()
    {
        timeScale = _timeScale;
    }

    void Update()
    {
        timeScale = TimeInput.timeDilation;
    }*/
    bool first = false;
    public bool firstBullet = false;

    Vector3 velocitySaved;

    private void Update()
    {

        if(isBeingHeld)
        {
            if(TimeInput.timeDilation>0.5)
            {
                rightHand.DetachObject(this.gameObject);
            }
        }
        else
        {
            //Debug.LogError("saved  velocity is " + velocitySaved + " and " + rigidbody.velocity * TimeInput.timeDilation);
            rigidbody.velocity = velocitySaved * TimeInput.timeDilation;
        }
        Debug.Log("Current state is " + Story.currentState);

        if(Story.currentState == Story.state.Tutorial)
        {
            if(once && firstBullet && Story.PickedUpLast)
            {
                once = false;

                GameObject temp = Instantiate(originalBulletInCaseFirst,transform.position,transform.rotation);
                Destroy(gameObject);
                
                //SphereCollider sc = gameObject.AddComponent(typeof(SphereCollider)) as SphereCollider;
                /* Interactable i = gameObject.AddComponent(typeof(Interactable)) as Interactable;

                i.hideControllerOnAttach = true;
                i.hideHandOnAttach = false;

                Throwable t = gameObject.AddComponent(typeof(Throwable)) as Throwable;*/
            }
        }



        
        
        

    }

    public bool once = true;

    void FixedUpdate()
    {
        //float dt = Time.fixedDeltaTime * timeScale;

        //rigidbody.velocity *= TimeInput.timeDilation;
        //rigidbody.velocity += Physics.gravity/rigidbody.mass * 0.2f;
    }

    

    public void setVel(Vector3 vel)
    {
        velocitySaved = vel;
    }

    void Start()
    {
        rigidbody = GetComponent<Rigidbody>();
        timeScale = 1;
        transform = GetComponent<Transform>();
        GameObject rightHandGO = GameObject.Find("RightHand");
        rightHand = rightHandGO.GetComponent<Hand>();
        //velocitySaved = rigidbody.velocity;
        /*Time.timeScale = 0.1f;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;*/
    }

    // Update is called once per frame
   

    public void pickup()
    {

        //bodyOfBullet = GetComponent<Rigidbody>();
        //vel = bodyOfBullet.velocity;


       

        isBeingHeld = true;

    }

    public GameObject originalBulletInCaseFirst;

    

    public void detach()
    {
        //bodyOfBullet.AddForce(vel, ForceMode.Impulse);
        if (Story.currentState == Story.state.Tutorial)
        {
            Story.currentState++;
        }


            //velocitySaved = velocitySaved - Vector3.forward;
            velocitySaved = this.transform.forward*10;
        isBeingHeld = false;
    }

    private void OnTriggerEnter(Collider other)
    {

        if(other.gameObject.CompareTag("Wall"))
        {
            if (Story.currentState == Story.state.Game)
            {
               
                GameObject storyManager = GameObject.Find("StoryManager");
                Story s = storyManager.GetComponent<Story>();
                s.playMiss();

                Story.currentState++;

                Animator anim = GameObject.Find("Shooting").GetComponent<Animator>() ;

                anim.SetBool("dead", true);

                StartCoroutine(vanishBaddie(GameObject.Find("Shooting")));

                


            }
            else
            {
                Destroy(gameObject);
            }

          

        }
        else if (other.gameObject.CompareTag("Baddie"))
        {
            if (Story.currentState == Story.state.Game)
            {
                
                GameObject storyManager = GameObject.Find("StoryManager");
                Story s = storyManager.GetComponent<Story>();
                s.playHit();
                Story.currentState++;
                Animator anim = GameObject.Find("Shooting").GetComponent<Animator>();

                anim.SetBool("dead", true);
                StartCoroutine(vanishBaddie(GameObject.Find("Shooting")));
            }
        }
        else if(other.gameObject.CompareTag("Player"))
        {
            if (Story.currentState != Story.state.Intro && Story.currentState != Story.state.Game)
            {
                Story.Score -= 10;
                Destroy(gameObject);
            }
        }

        Debug.Log("TRIGGER TO " + other.gameObject.tag);

        /* bodyOfBullet = GetComponent<Rigidbody>();
         vel = bodyOfBullet.velocity;*/
    }


    IEnumerator vanishBaddie(GameObject g)
    {
        yield return new WaitForSeconds(5f);

        Destroy(g);
        Destroy(this);
    }


}
