using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PostProcessing;

using Valve.VR;

public class TimeInput : MonoBehaviour
{


    public SteamVR_ActionSet actionSet;
    public SteamVR_Action_Single trigger;
    public SteamVR_Action_Vibration vibration;
    public PostProcessingBehaviour effects;

    [Range(0,100)]
    public float meter = 100f;

    public static float meterStatic;

    [Range(0,100)]
    public float holdPowerPercentage = 100f;

    [Range(0, 1)]
    public float vignetteIntensity = 0f;

    [Range(0,1)]
    public float saturation = 1f;

    public float vignetteRange = 2f;


    public static float timeDilation = 1f;
    public float timeDilation1 = 1f;



    private void Start()
    {
        actionSet.Activate(SteamVR_Input_Sources.Any, 0, true);
    }


    private void Update()
    {
        
        meterStatic = meter;
        float triggerValue = trigger.GetAxis(SteamVR_Input_Sources.LeftHand);
        //Debug.Log("trigger is " + triggerValue);

        if(triggerValue>0)
        {
            triggerValue = map(triggerValue, 0, 1, 0.5f, 0.86f);
            if(meter>0)
            {
                increaseMeter(-triggerValue*0.5f);
            }
            else if(meter<=0)
            {
                increaseVignette(0.02f);
                increaseHoldPowerPercentage(-0.5f);
                
            }


            slowDownTime(triggerValue);


        }
        else if(triggerValue==0)
        {
            increaseVignette(-0.02f);
            increaseHoldPowerPercentage(0.5f);
            increaseMeter(1f);
            slowDownTime(0);
            
        }

        if(meter<50)
        {
            vibration.Execute(0, 0.2f,50,1/meter, SteamVR_Input_Sources.LeftHand);
        }

        

    }

    void increaseMeter(float amount)
    {
        meter += amount;

        if (meter > 100) meter = 100;
        if (meter < 0) meter = 0;
    }

    void increaseVignette(float amount)
    {

        VignetteModel v = effects.profile.vignette;
        VignetteModel.Settings test = v.settings;

        ColorGradingModel c = effects.profile.colorGrading;

        ColorGradingModel.Settings settings = c.settings;

        
        vignetteIntensity += amount;
        saturation -= amount;
        if (vignetteIntensity > vignetteRange)
        {
            vignetteIntensity = vignetteRange;
        }
        if (vignetteIntensity < 0)
        {
            vignetteIntensity = 0f;
        }

        if(saturation<0)
        {
            saturation = 0;
        }
        if(saturation>1)
        {
            saturation = 1;
        }


        test.intensity = vignetteIntensity;
        v.settings = test;

        settings.basic.saturation = saturation;
        c.settings = settings;

        effects.profile.vignette = v;

        effects.profile.colorGrading = c;
    }

    void increaseHoldPowerPercentage(float amount)
    {
        
        holdPowerPercentage += amount;
        if (holdPowerPercentage > 100)
        {
            holdPowerPercentage = 100;
        }
        if (holdPowerPercentage < 0)
        {
            holdPowerPercentage = 0;
        }
    }



    void slowDownTime(float value)
    {


        if (Story.currentState != Story.state.preIntro && Story.currentState != Story.state.Intro & Story.currentState != Story.state.Tutorial)
        {
            timeDilation = 1 - value * (holdPowerPercentage) / 100;
            timeDilation1 = timeDilation;
            /*Time.timeScale = 1 - value*(holdPowerPercentage)/100;
            Time.fixedDeltaTime = Time.timeScale * 0.02f;*/
        }
    }

    private void OnApplicationQuit()
    {
        setVignetteIntensity(0);
        Time.timeScale = 1;
        Time.fixedDeltaTime = Time.timeScale * 0.02f;
    }


    void setVignetteIntensity(float intensity)
    {
        VignetteModel v = effects.profile.vignette;
        VignetteModel.Settings test = v.settings;
        test.intensity = intensity;
        v.settings = test;

        ColorGradingModel c = effects.profile.colorGrading;
        ColorGradingModel.Settings settings = c.settings;
        settings.basic.saturation = 1-intensity;
        c.settings = settings;

        effects.profile.vignette = v;
        effects.profile.colorGrading = c;
    }


    public static float map(float x, float in_min, float in_max, float out_min, float out_max)
    {
        return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    }
    //public SteamVR_ActionSet m_ActionSet;

    //public SteamVR_Action_Vector2 touchPadCoordinates;
    //public SteamVR_Action_Single trigger;
    //public SteamVR_Action_Vibration vibration;

    //public PostProcessingBehaviour grayscale;


    //public float maxSecondsTimeSlow = 10f;
    //public float coolDownTime = 5f;
    //public float slowdownFactor = 0.05f;
    //public float slowdownLength = 2f;

    //[Range(0,100)]
    //public int meter=100;

    //bool runOutOfTime = false;
    //bool triggerPressed = false;

    //void Start()
    //{
    //    m_ActionSet.Activate(SteamVR_Input_Sources.Any, 0, true);
    //    Time.timeScale = 0.7f;
    //    Time.fixedDeltaTime = Time.timeScale * 0.02f;
    //}


    //void Update()
    //{
    //    //Vector2 pos = touchPadCoordinates[SteamVR_Input_Sources.LeftHand].axis;

    //    /*if(pos!=Vector2.zero)
    //    {
    //        Debug.Log(pos);
    //        VignetteModel v = grayscale.profile.vignette;
    //        VignetteModel.Settings test = v.settings;
    //        test.intensity += 0.1f;
    //        v.settings = test;

    //        grayscale.profile.vignette = v;
    //    }*/
    //    DoSlowMotionTrigger();

    //    if (triggerPressed)
    //    {
    //        meter--;
    //    }
    //    else meter++;

    //    if(meter==0)
    //    {
    //        //start cooldown
    //    }

    //}



    //public void DoSlowMotionTrigger()
    //{
    //    float tri = trigger.GetAxis(SteamVR_Input_Sources.LeftHand);




    //    if (tri == 0)
    //    {
    //        triggerPressed = false;
    //        //not pressed
    //    }
    //    else triggerPressed = true;
    //    /*if (tri != 0)
    //    {
    //        Time.timeScale = 1 - tri;
    //        Time.fixedDeltaTime = Time.timeScale * 0.02f;

    //        vibration.Execute(0, 0.2f, 100, tri, SteamVR_Input_Sources.LeftHand);
    //    }*/
    //}

    //public void DoSlowMotionPad(float y)
    //{
    //    Time.timeScale = y + 1f ;
    //    Time.fixedDeltaTime = Time.timeScale * 0.02f;
    //}

    //float map(float x, float in_min, float in_max, float out_min, float out_max)
    //{
    //    return (x - in_min) * (out_max - out_min) / (in_max - in_min) + out_min;
    //}

    //IEnumerator timeSlowCountdown()
    //{
    //    yield return new WaitForSeconds(maxSecondsTimeSlow / 2);
    //    runOutOfTime = true;

    //}
    //IEnumerator waitForCooldown()
    //{
    //    yield return new WaitForSeconds(coolDownTime);
    //    runOutOfTime = false;
    //}

    //void slowDownTimeNormal()
    //{

    //}
    //void slowDownTimeRunOut()
    //{

    //}
    //void fixRunDown()
    //{

    //}
}
