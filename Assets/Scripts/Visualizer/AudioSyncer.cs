using UnityEngine;

public class AudioSyncer : MonoBehaviour
{
    public float bias;
    public float timeStep;
    public float timeToBeat;
    public float restSmoothTime;

    private float m_previousAudioValue;
    private float m_audioValue;
    private float m_timer;

    protected bool m_isBeat;

    private void Update()
    {
        OnUpdate();
    }

    public virtual void OnBeat() {
        //Debug.Log("Beat");
        m_timer = 0;
        m_isBeat = true;
    }

    public virtual void OnUpdate()
    {
        m_previousAudioValue = m_audioValue;
        m_audioValue = AudioSpectrum.spectrumValue;

        if (m_previousAudioValue > bias && m_audioValue <= bias)
        {
            if (m_timer > timeToBeat)
            {
                OnBeat();
            }
        }

        if (m_previousAudioValue <= bias && m_audioValue > bias)
        {
            if (m_timer > timeToBeat)
            {
                OnBeat();
            }
        }

        m_timer += Time.deltaTime;
    }
}
