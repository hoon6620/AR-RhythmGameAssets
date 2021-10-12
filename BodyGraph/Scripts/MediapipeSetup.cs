using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MediapipeSetup : MonoBehaviour
{
    [SerializeField] GameObject myGraph;
    private WebCamDevice[] devices;


    IEnumerator Start()
    {
        Debug.Log($"Graph Changed: {myGraph.name}");
        ARDirector.Inst.ChangeGraph(myGraph);

        yield return new WaitForEndOfFrame();

        ResetOptions();
    }

    void ResetOptions()
    {
        devices = WebCamTexture.devices;

        if (devices.Length == 0)
        {

        }
        else
        {
            var device = devices[0];
            Debug.Log("WebCamDevice Changed: " + device.name);
            ARDirector.Inst.ChangeWebCamDevice(device);
        }
    }
}
