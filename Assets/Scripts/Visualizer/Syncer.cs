using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Syncer : MonoBehaviour
{
    public float bias;
    public float timeStep;
    public float timeToBeat;
    public float restSmoothTime;
    private float prevAudioVal;
    private float audioVal;
    private float timer;
    protected bool isBeat;
 
    private void Update()
    {
        OnUpdate();
    }

    public virtual void OnUpdate(){
        prevAudioVal = audioVal;
        audioVal = Spectrum.val;
        if (prevAudioVal > bias && audioVal <= bias){
            if (timer > timeStep){
                OnBeat();
            }   
        }

        if (prevAudioVal <= bias && audioVal > bias){
            if (timer > timeStep){
                OnBeat();
            }
        }
        timer += Time.deltaTime;
    }
    
    public virtual void OnBeat(){
        timer = 0;
        isBeat = true;
    }
}
