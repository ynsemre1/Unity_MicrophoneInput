using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VoiceListener : MonoBehaviour
{
    AudioSource audioSource;

    // Default microphone
    public string selectedDevice;

    // Block used for audioSource.GetOutputData()
    public static float[] samples = new float[128];

    public float soundThreshold = 0.008f; // Threshold for closing, you can set the desired value

    public TextMesh textEdit;

    void Start()
    {
        // Get the AudioSource component
        audioSource = GetComponent<AudioSource>();

        /**
         * If there are microphones,
         * select the default microphone,
         * set audioSource.clip to the default microphone, in a 1-second loop, at the sample rate (sampleRate)
         * enable the loop
         */
        if (Microphone.devices.Length > 0)
        {
            selectedDevice = Microphone.devices[0].ToString();
            audioSource.clip = Microphone.Start(selectedDevice, true, 1, AudioSettings.outputSampleRate);
            audioSource.loop = true;

            /**
             * As long as the recording position of the microphone is greater than 0,
             * play the clip
             */
            while (!(Microphone.GetPosition(selectedDevice) > 0))
            {
                audioSource.Play();
            }
        }   
    }

    void Update()
    {
        GetSoundData();
    }

    /**
     * Load block samples from the audioSource output data
     * Average the values over the block size
     * vals represents the microphone sound level, used to control the block height
     * Block height represents the magnitude
     */
    void GetSoundData()
    {
        audioSource.GetOutputData(samples, 0);

        float vals = 0.0f;

        for (int i = 0; i < 128; i++)
        {
            vals += Mathf.Abs(samples[i]);
        }

        float soundIntensity = vals /= 128.0f;
        Debug.Log("++++" + soundIntensity + "++++");

        if (soundIntensity > soundThreshold)
        {
            textEdit.text = "YOU ARE SPEAKING";
        }
        else
        {
            textEdit.text = "YOU ARE NOT SPEAKING";
        }
    }
}
