using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;

public class ARScript : MonoBehaviour
{
    public GameObject objectPrefab;
    private ARRaycastManager raycastManager;
    private GameObject spawnedObject;
    private List<ARRaycastHit> hits = new List<ARRaycastHit>();
    private List<ARPlane> detectedPlanes = new List<ARPlane>();
    private ARPlaneManager planeManager;

    void Start()
    {
        raycastManager = GetComponent<ARRaycastManager>();
        planeManager = GetComponent<ARPlaneManager>();

        // Subscribe to plane detection events
        planeManager.planesChanged += OnPlanesChanged;
    }

    void Update()
    {
        if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
        {
            Touch touch = Input.GetTouch(0);
            if (raycastManager.Raycast(touch.position, hits, TrackableType.PlaneWithinPolygon))
            {
                Pose hitPose = hits[0].pose;
                if (spawnedObject == null)
                {
                    spawnedObject = Instantiate(objectPrefab, hitPose.position, hitPose.rotation);
                    FaceCamera();
                    spawnedObject = null;
                    //DisablePlaneDetection();
                }
            }
        }
    }

    void FaceCamera()
    {
        if (Camera.main != null && spawnedObject != null)
        {
            Vector3 lookAtPosition = new Vector3(Camera.main.transform.position.x, spawnedObject.transform.position.y, Camera.main.transform.position.z);
            spawnedObject.transform.rotation = Quaternion.Euler(0, 180, 0);
        }
    }

    void DisablePlaneDetection()
    {
        ARPlaneManager planeManager = GetComponent<ARPlaneManager>();
        planeManager.enabled = false;
        ARPointCloudManager pointCloudManager = GetComponent<ARPointCloudManager>();
        pointCloudManager.enabled = false;
      
        foreach (var plane in detectedPlanes)
        {
            plane.gameObject.SetActive(false);
        }
    }

    void OnPlanesChanged(ARPlanesChangedEventArgs args)
    {
        foreach (var plane in args.added)
        {
            detectedPlanes.Add(plane);
        }

        foreach (var plane in args.removed)
        {
            detectedPlanes.Remove(plane);
        }
    }

    public void Reload()
    {
        foreach (var plane in planeManager.trackables)
        {
            Destroy(plane.gameObject);
        }
        SceneManager.LoadScene("BlankAR");
    }
}
