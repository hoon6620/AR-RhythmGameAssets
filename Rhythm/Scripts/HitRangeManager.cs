using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Mediapipe;

public class HitRangeManager : MonoBehaviour
{
    public static HitRangeManager Inst;
    void Awake() => Inst = this;

    [SerializeField]
    Vector2 screenSize;
    [SerializeField]
    GameObject hitRangePrefab;

    GameObject rightHandRange;
    GameObject leftHandRange;

    GameObject rightElbowRange;
    GameObject leftElbowRange;

    GameObject rightKneeRange;
    GameObject leftKneeRange;

    private void Start()
    {
        rightHandRange = Instantiate(hitRangePrefab);
        leftHandRange = Instantiate(hitRangePrefab);

        rightElbowRange = Instantiate(hitRangePrefab);
        leftElbowRange = Instantiate(hitRangePrefab);

        rightKneeRange = Instantiate(hitRangePrefab);
        leftKneeRange = Instantiate(hitRangePrefab);

        rightHandRange.GetComponent<HitRange>().SetUp(NoteType.HAND_RIGHT);
        leftHandRange.GetComponent<HitRange>().SetUp(NoteType.HAND_LEFT);

        rightElbowRange.GetComponent<HitRange>().SetUp(NoteType.ELBOW_RIGHT);
        leftElbowRange.GetComponent<HitRange>().SetUp(NoteType.ELBOW_LEFT);

        rightKneeRange.GetComponent<HitRange>().SetUp(NoteType.KNEE_RIGHT);
        leftKneeRange.GetComponent<HitRange>().SetUp(NoteType.KNEE_LEFT);
    }

    private void OnDestroy()
    {
        Destroy(rightHandRange);
        Destroy(leftHandRange);

        Destroy(rightElbowRange);
        Destroy(leftElbowRange);

        Destroy(rightKneeRange);
        Destroy(leftKneeRange);
    }

    public void Draw(NormalizedLandmarkList pose, NormalizedLandmarkList rightHand, NormalizedLandmarkList leftHand)
    {
        DrawKneeElbowRange(pose);
        DrawHandRange(rightHand, leftHand);
    }

    void DrawHandRange(NormalizedLandmarkList rightHand, NormalizedLandmarkList leftHand)
    {
        if (leftHand.Landmark.Count != 0)
        {
            rightHandRange.SetActive(true);
            rightHandRange.transform.position = (LandMark2Vector3(leftHand.Landmark[0]) + LandMark2Vector3(leftHand.Landmark[5]))/2;
        }
        else
            rightHandRange.SetActive(false);

        if (rightHand.Landmark.Count != 0)
        {
            leftHandRange.SetActive(true);
            leftHandRange.transform.position = (LandMark2Vector3(rightHand.Landmark[0]) + LandMark2Vector3(rightHand.Landmark[5]))/2;
        }
        else
            leftHandRange.SetActive(false);
    }

    void DrawKneeElbowRange(NormalizedLandmarkList pose)
    {
        if (pose.Landmark.Count == 0)
            return;
        rightElbowRange.transform.position = LandMark2Vector3(pose.Landmark[13]);
        leftElbowRange.transform.position = LandMark2Vector3(pose.Landmark[14]);

        rightKneeRange.transform.position = LandMark2Vector3(pose.Landmark[25]);
        leftKneeRange.transform.position = LandMark2Vector3(pose.Landmark[26]);
    }

    Vector3 LandMark2Vector3(NormalizedLandmark landmark)
    {
        float x = landmark.X - 0.5f;
        float y = 0.5f - landmark.Y;

        return Vector3.Scale(new Vector3(x, y, 0), screenSize);
    }
}
