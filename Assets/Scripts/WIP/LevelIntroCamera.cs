using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using UnityEngine.Experimental.Rendering.Universal;

public class LevelIntroCamera : MonoBehaviour
{
    [Header("Config")]
    [SerializeField] private float _waitTime;
    [SerializeField] private float _zoomTime;
    [SerializeField] private int _startingPPU;
    [SerializeField] private int _endingPPU;
    [SerializeField] private PixelPerfectCamera _pixelPerfectCamera;
    [SerializeField] private CinemachineBrain _cinemachineBrain;
    [SerializeField] private CinemachineVirtualCamera _levelCamera;
    [SerializeField] private CinemachineVirtualCamera _clumpPositionCamera;
    [SerializeField] private CinemachineVirtualCamera _clumpFollowCamera;

    [Header("Listening for...")]
    [SerializeField] private VoidEventSO _sceneLoaded;
    [SerializeField] private VoidEventSO _curtainsOpened;

    [Header("Broadcasting to...")]
    [SerializeField] private VoidEventSO _zoomFinished;

    private void OnEnable()
    {
        _sceneLoaded.OnEventRaised += SetUpCameras;
        _curtainsOpened.OnEventRaised += StartZoom;
    }

    private void OnDisable()
    {
        _sceneLoaded.OnEventRaised -= SetUpCameras;
        _curtainsOpened.OnEventRaised -= StartZoom;
    }

    private void SetUpCameras()
    {
        Debug.Log($"{name} Set up cameras");
        _pixelPerfectCamera.assetsPPU = _startingPPU;
        _cinemachineBrain.m_DefaultBlend.m_Time = _zoomTime;
        _levelCamera.Priority = 1;
        _clumpPositionCamera.Priority = 0;
        _clumpFollowCamera.Priority = 0;
    }


    private void StartZoom()
    {
        Debug.Log($"{name} start zoom {Time.time}");
        //canWait = true;
        StartCoroutine(StartZoomRoutine());
    }

    float timeWaited;
    bool canWait;
    bool canZoom;
    bool zoomDone;
    float lerpTime;
    float moveTimeX;
    float moveTimeZ;

    //private void FixedUpdate()
    //{
    //    if (canWait)
    //    {
    //        //Debug.Log($"{name} waiting");
    //        if (timeWaited < _waitTime)
    //        {
    //            timeWaited += Time.deltaTime;
    //        }
    //        else
    //        {
    //            canWait = false;
    //            canZoom = true;
    //        }
    //    }

    //    if (canZoom)
    //    {
    //        //Debug.Log($"{name} zooming {lerpTime}");
    //        _levelCamera.Priority = 0;
    //        _clumpPositionCamera.Priority = 1;
    //        _clumpFollowCamera.Priority = 0;
    //        if (lerpTime < _zoomTime)
    //        {
    //            lerpTime += Time.deltaTime / _zoomTime;
    //            _pixelPerfectCamera.assetsPPU =
    //                Mathf.FloorToInt(Mathf.Lerp(_startingPPU, _endingPPU, lerpTime));
    //            moveTimeX += Time.deltaTime / _zoomTime;
    //            moveTimeZ += Time.deltaTime / _zoomTime;
    //            var posX = Mathf.Lerp(_levelCamera.transform.position.x, _clumpPositionCamera.transform.position.x, moveTimeX);
    //            var posZ = Mathf.Lerp(_levelCamera.transform.position.z, _clumpPositionCamera.transform.position.z, moveTimeX);

    //            _levelCamera.transform.position = new Vector3(posX, _levelCamera.transform.position.y, posZ);
    //        }
    //        else
    //        {
    //            canZoom = false;
    //            zoomDone = true;
    //        }
    //    }

    //    if (zoomDone)
    //    {
    //        Debug.Log($"{name} zoom done {Time.time}");
    //        _levelCamera.Priority = 0;
    //        _clumpPositionCamera.Priority = 0;
    //        _clumpFollowCamera.Priority = 1;
    //        _zoomFinished.Raise(name);
    //        zoomDone = false;
    //    }
    //}


    //private void StartZoom() => StartCoroutine(StartZoomRoutine());
    private IEnumerator StartZoomRoutine()
    {
        Debug.Log($"{name} start zoom");
        _levelCamera.Priority = 1;
        _clumpPositionCamera.Priority = 0;
        _clumpFollowCamera.Priority = 0;
        yield return new WaitForSeconds(_waitTime);
        _levelCamera.Priority = 0;
        //_clumpPositionCamera.Priority = 1;
        _clumpFollowCamera.Priority = 1;
        var lerpTime = 0f;

        while (lerpTime < _zoomTime)
        {
            Debug.Log($"Blend duration {lerpTime / _zoomTime}");
            _pixelPerfectCamera.assetsPPU =
                (int)Mathf.Lerp(_startingPPU, _endingPPU, lerpTime / _zoomTime);
            lerpTime += Time.deltaTime;
            yield return new WaitForEndOfFrame();
        }
        _pixelPerfectCamera.assetsPPU = _endingPPU;
        _zoomFinished.Raise(name);
    }
}
