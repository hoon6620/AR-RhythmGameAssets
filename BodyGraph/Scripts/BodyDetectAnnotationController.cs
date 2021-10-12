using Mediapipe;
using UnityEngine;
using System.Collections.Generic;

public class BodyDetectAnnotationController : AnnotationController
{
    [SerializeField] GameObject poseLandmarksPrefab = null;
    [SerializeField] GameObject handLandmarksPrefab = null;

    private GameObject poseLandmarksAnnotation;
    private GameObject leftHandLandmarksAnnotation;
    private GameObject rightHandLandmarksAnnotation;

    enum Side
    {
        Left = 1,
        Right = 2,
    }

    void Start()
    {
        poseLandmarksAnnotation = Instantiate(poseLandmarksPrefab);
        leftHandLandmarksAnnotation = Instantiate(handLandmarksPrefab);
        rightHandLandmarksAnnotation = Instantiate(handLandmarksPrefab);
    }

    void OnDestroy()
    {
        Destroy(poseLandmarksAnnotation);
        Destroy(leftHandLandmarksAnnotation);
        Destroy(rightHandLandmarksAnnotation);
    }

    public override void Clear()
    {
        poseLandmarksAnnotation.GetComponent<FullBodyPoseLandmarkListAnnotationController>().Clear();
        leftHandLandmarksAnnotation.GetComponent<HandLandmarkListAnnotationController>().Clear();
        rightHandLandmarksAnnotation.GetComponent<HandLandmarkListAnnotationController>().Clear();
    }

    public void Draw(Transform screenTransform, NormalizedLandmarkList poseLandmarks, NormalizedLandmarkList leftHandLandmarks, NormalizedLandmarkList rightHandLandmarks, bool isFlipped = false)
    {
        poseLandmarksAnnotation.GetComponent<FullBodyPoseLandmarkListAnnotationController>().Draw(screenTransform, poseLandmarks, isFlipped);
        leftHandLandmarksAnnotation.GetComponent<HandLandmarkListAnnotationController>().Draw(screenTransform, leftHandLandmarks, isFlipped);
        rightHandLandmarksAnnotation.GetComponent<HandLandmarkListAnnotationController>().Draw(screenTransform, rightHandLandmarks, isFlipped);
    }
}
