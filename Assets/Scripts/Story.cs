using System.Collections;
using System.Collections.Generic;
using UnityEngine;

using Valve.VR.InteractionSystem;
using Valve.VR;

public class Story : MonoBehaviour
{
    public SteamVR_Behaviour_Skeleton leftHand,rightHand;

    public Hand rightHandInt;
    public SteamVR_Action_Single trigger;

    public GameObject lights;
    public Light barrelLight;

    public Animator anim;

    public GameObject gunBarrel;
    public GameObject actor;
    public Transform playerCamTransform;


    public AudioClip introVoice;
    public AudioClip afterIntro;
    public AudioClip justASecond;
    public AudioClip startBullets;
    public AudioClip miss;
    public AudioClip hit;

    public AudioClip beforeShoot;
    public AudioClip afterShoot;

    public AudioSource source;
    public AudioSource music;

    public AudioClip bgMusic;


    public TrainingTableScript trainingTableScript;
    public wallsLiftUp pWalls;


    public static int Score = 0;

    public enum state
    {
        preIntro,
        Intro,
        Tutorial,
        Game,
        GameF,
        End
    }

    public static state currentState = state.preIntro;

    // Start is called before the first frame update
    void Start()
    {
        leftHand.rangeOfMotion = EVRSkeletalMotionRange.WithoutController;
        rightHand.rangeOfMotion = EVRSkeletalMotionRange.WithController;
        anim.SetFloat("speedOfAnimation", 0f);
        barrelLight.intensity = 0f;
        dummies = null;
        //lights.SetActive(false);
    }

    public static bool PickedUpLast = false;
    bool test = false;

    bool justOnce = false;

    bool justOnceMainGame = false;

    bool barrelMoving = false;

    public GameObject goodEnding;
    public GameObject badEnding;


    public AudioClip endMusic;

    
    public void pickUpLastPage()
    {
        PickedUpLast = true;
    }

    // Update is called once per frame
    void Update()
    {


        switch (currentState)
        {
            case state.preIntro:
                if (!ControllerButtonHints.IsButtonHintActive(rightHandInt, trigger) && !justOnce)
                {
                    ControllerButtonHints.ShowButtonHint(rightHandInt, trigger);

                    //Score++;

                }

               
                float tri = trigger.GetAxis(SteamVR_Input_Sources.RightHand);
                //Debug.Log(tri);
                //currentState++;
                if (tri > 0.5f && !justOnce)
                {

                    ControllerButtonHints.HideAllButtonHints(rightHandInt);

                    GameObject temp = GameObject.Find("vr_glove_right_model_slim(Clone)");
                    rightHand = temp.GetComponent<SteamVR_Behaviour_Skeleton>();
                    rightHand.rangeOfMotion = EVRSkeletalMotionRange.WithoutController;
                    // rightHand.rangeOfMotion = EVRSkeletalMotionRange.WithoutController;
                    source.clip = introVoice;
                    source.Play();
                    justOnce = true;
                    StartCoroutine(waitForIntroVoice());
                    //currentState = state.GameF;
                    

                    //stateOfDummies();


                   


                }
                break;
            case state.Intro:

                if (barrelMoving)
                //gunBarrel.transform.rotation.eulerAngles;
                {
                    gunBarrel.transform.eulerAngles = new Vector3(gunBarrel.transform.eulerAngles.x,
                      gunBarrel.transform.eulerAngles.y - 0.089f,
                      gunBarrel.transform.eulerAngles.z);
                    /* Vector3 targetDir = actor.transform.position - gunBarrel.transform.position;
                     Vector3 newDir = Vector3.RotateTowards(gunBarrel.transform.forward, targetDir, 0.09f, 0.0f);

                     //gunBarrel.transform.rotation = Quaternion.LookRotation(newDir);

                    /* gunBarrel.transform.eulerAngles = new Vector3(gunBarrel.transform.eulerAngles.x,
                       180f - gunBarrel.transform.eulerAngles.y,
                       gunBarrel.transform.eulerAngles.z);*/

                    //gunBarrel.transform.LookAt(actor.transform);
                }
                if (!justOnce)
                {
                    source.clip = beforeShoot;
                    source.Play();
                    //anim.SetFloat("speedOfAnimation", 1.5f);
                    justOnce = true;
                    StartCoroutine(waitForMusic());
                }

                break;

            case state.Game:

                if (!justOnceMainGame)
                {
                    pWalls.startGoingUp();


                    trainingTableScript.startGoingDown();

                    justOnceMainGame = true;
                } 

                break;

            case state.GameF:


                //DummyScript s = gameObject.GetComponent<DummyScript>();
                //GameObject t = s.spawnRandomDummy();

                //t.GetComponentInChildren<ShootBullet>().FireTowards(playerCamTransform);

                stateOfDummies();
                justOnceMainGame = false;

                break;
            case state.End:
                if(!justOnceMainGame)
                {
                    pWalls.startGoingDown();

                    //Score += 1000;

                    StartCoroutine(endGame());


                    justOnceMainGame = true;
                }
                break;
        }

        

        if (!test)
        {
           

           
        }
    }

    IEnumerator endGame()
    {

        yield return new WaitForSeconds(2f);


        if(Score>200)
        {
            goodEnding.SetActive(true);
        }
        else
        {
            badEnding.SetActive(true);
        }


        music.clip = endMusic;
        music.volume = 0.1f;
        music.Play();

    }

    public string dummySequence;


    public GameObject[] dummies;


    bool checkIfDummiesAreNull()
    {

        bool theyAreNull = true;
        foreach (GameObject go in dummies)
        {
            if (go != null)
            {
                theyAreNull = false;
            }
        }

        return theyAreNull;
    }

    void stateOfDummies()
    {

        if (dummies == null || checkIfDummiesAreNull())
        {
            //char number = dummySequence[0];

            if (dummySequence == "")
            {
                currentState++;
            }
            else
            {
                int numberInt = int.Parse(dummySequence[0].ToString());

                dummies = new GameObject[numberInt];

                dummySequence = dummySequence.Substring(1, dummySequence.Length - 1);

                for (int i = 0; i < numberInt; i++)
                {
                    DummyScript s = gameObject.GetComponent<DummyScript>();
                    GameObject t = s.spawnRandomDummy();
                    t.GetComponentInChildren<ShootBullet>().setTransformToFireTo(playerCamTransform);
                    t.GetComponentInChildren<ShootBullet>().activateToShoot();
                    dummies[i] = t;
                }

            }
        }


    }




    IEnumerator waitForIntroVoice()
    {
        while(source.isPlaying)
        {
            yield return null;
        }
        currentState++;
        justOnce = false;
    }

    IEnumerator waitForMusic()
    {
        //5:53 seconds
        for (float i = 0f; i <= 5.53f; i += Time.deltaTime)
        {
            barrelLight.intensity = i/5.53f;

            yield return null;
        }
        /*for(float i = 0f;i<=1f;i+=0.003f)
        {
            barrelLight.intensity = i;

            yield return null;
        }*/
        anim.SetFloat("speedOfAnimation", 1.5f);
        barrelMoving = true;
        StartCoroutine(waitForShoot());
    }


    IEnumerator waitForShoot()
    {
        //around 3 seconds
        yield return new WaitForSeconds(3f);
        anim.SetBool("isShooting", true);
        StartCoroutine(turnToShoot());
        barrelMoving = false;
        /*gunBarrel.transform.eulerAngles = new Vector3(gunBarrel.transform.eulerAngles.x,
                     gunBarrel.transform.eulerAngles.y - 1f,
                     gunBarrel.transform.eulerAngles.z);*/

    }

    IEnumerator turnToShoot()
    {
   
        for (int i =0;i<100;i++)
        {
            actor.transform.eulerAngles = new Vector3(actor.transform.eulerAngles.x,
                   actor.transform.eulerAngles.y - 100/90,
                   actor.transform.eulerAngles.z);
            //yield return null;
        }
        
        anim.SetFloat("speedOfAnimation", 0.5f);
        StartCoroutine(waitToShoot());
       
        yield return null;
    }

    IEnumerator waitToShoot()
    {
        yield return new WaitForSeconds(0.5f);

        actor.GetComponentInChildren<ShootBullet>().FireTowards(playerCamTransform);
        StartCoroutine(slowDownTimeAfterShoot());
    }



    IEnumerator slowDownTimeAfterShoot()
    {

       
        for(float i = 0f;i<2f;i+=Time.deltaTime)
        {
            TimeInput.timeDilation -= Time.deltaTime/2f;
            if (TimeInput.timeDilation < 0) TimeInput.timeDilation = 0f;
            //Debug.Log("Time is " + TimeInput.timeDilation);
            yield return null;
        }
        TimeInput.timeDilation = 0;

        source.clip = afterShoot;
        source.Play();
        setMaterialTransparent();

        StartCoroutine(fadeBarrel());
        StartCoroutine(fadeInLights());
        yield return null;

    }

    IEnumerator fadeBarrel()
    {
        Renderer renderer = gunBarrel.GetComponentInChildren<MeshRenderer>();

        Material m = renderer.material;
        

        for (int i = 255; i > 0; i--)
        {
            m.color = new Color(m.color.r, m.color.g, m.color.b, i/255f);
            yield return null;
        }

        

        yield return null;

    }


    //https://www.youtube.com/watch?v=nNjNWDZSkAI
    void setMaterialTransparent()
    {
        Renderer renderer = gunBarrel.GetComponentInChildren<MeshRenderer>();

        Material m = renderer.material;

        m.SetFloat("_Mode", 2);
        m.SetInt("_SrcBlend", (int)UnityEngine.Rendering.BlendMode.SrcAlpha);
        m.SetInt("_DstBlend", (int)UnityEngine.Rendering.BlendMode.OneMinusSrcAlpha);
        m.SetInt("_ZWrite", 0);
        m.DisableKeyword("_ALPHATEST_ON");
        m.EnableKeyword("_ALPHABLEND_ON");
        m.DisableKeyword("_ALPHAPREMULTIPLY_ON");
        m.renderQueue = 3000;

      

    }


    IEnumerator fadeInLights()
    {

        lights.SetActive(true);

        float intensity = 0;
        //Light []lightsObjects = ;
        foreach (Light l in lights.GetComponentsInChildren<Light>())
        {
            l.intensity = 0;
        }

        for (float i = 0; i<4f;i+=Time.deltaTime)
        //for (intensity = 0; intensity <= 1.6f; intensity += Time.deltaTime/2f)
        {
            foreach (Light l in lights.GetComponentsInChildren<Light>())
            {
                l.intensity += 1.6f *(Time.deltaTime / 4f );
            }

            yield return null;
        }

        while(source.isPlaying)
        {
            yield return null;
        }

        source.clip = afterIntro;
        source.Play();

       

        StartCoroutine(waitForIntroVoiceToFinishAndLiftTable());

        yield return null;


    }
    
    IEnumerator waitForIntroVoiceToFinishAndLiftTable()
    {
        while(source.isPlaying)
        {
            yield return null;
        }

        source.clip = justASecond;
        source.Play();

        yield return new WaitForSeconds(2.08f);

        trainingTableScript.startGoingUp();
        yield return new WaitForSeconds(2f);

        source.clip = startBullets;
        source.Play();

        music.clip = bgMusic;
        music.volume = 0.02f;
        music.Play();

        yield return new WaitForSeconds(3f);

        currentState++;
        //pWalls.startGoingUp();

    }


    public void playMiss()
    {
        source.clip = miss;
        source.Play();
    }

    public void playHit()
    {
        source.clip = hit;
        source.Play();
    }



}
