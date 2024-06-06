using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Scripting;

public class OBEManager : MonoBehaviour
{
    [SerializeField]
    private InputCoordinator coordinator;
    [SerializeField]
    private Camera headSetCamera;
    [SerializeField]
    private GameObject OBECameraObject;
    [SerializeField]
    private Transform finalOBEPosition;
    [SerializeField]
    private float moveSeconds = 1f;

    private OBEState state;
    private Camera OBECamera; 

    public enum OBEState {
        Before,
        Moving,
        DoneMoving
    }

    void Start()
    {
        OBECamera = OBECameraObject.GetComponent<Camera>();

        headSetCamera.enabled = true;
        OBECamera.enabled = false;
        coordinator.primaryButtonPress.AddListener(OnPrimaryButtonEvent);

        state = OBEState.Before;
    }

    public void OnPrimaryButtonEvent(bool pressed)
    {
        if(state == OBEState.Before && pressed) {
            state = OBEState.Moving;

            headSetCamera.enabled = false;
            OBECamera.enabled = true;

            StartCoroutine(DoOBE());
        }
    }

    IEnumerator DoOBE() {
        yield return StartCoroutine(MoveCamera());

        Debug.Log("Done moving");
        state = OBEState.DoneMoving;
    }

    IEnumerator MoveCamera() {
        float timeSinceStarted = 0f;
        Vector3 initialPosition = OBECameraObject.transform.position;

        while (true)
        {
            timeSinceStarted += Time.deltaTime;
            OBECameraObject.transform.position = Vector3.Lerp(initialPosition, finalOBEPosition.position, timeSinceStarted / moveSeconds);

            if (timeSinceStarted > moveSeconds)
            {
                yield break;
            }

            yield return null;
        }
    }
}
