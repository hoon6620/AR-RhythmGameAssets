using Mediapipe;
using UnityEngine;

public class BodyDetectGraph : DemoGraph
{
    [SerializeField] bool drawNodes = true;

    private const string poseLandmarksStream = "pose_landmarks";
    private OutputStreamPoller<NormalizedLandmarkList> poseLandmarksStreamPoller;
    private NormalizedLandmarkListPacket poseLandmarksPacket;

    private const string leftHandLandmarksStream = "left_hand_landmarks";
    private OutputStreamPoller<NormalizedLandmarkList> leftHandLandmarksStreamPoller;
    private NormalizedLandmarkListPacket leftHandLandmarksPacket;

    private const string rightHandLandmarksStream = "right_hand_landmarks";
    private OutputStreamPoller<NormalizedLandmarkList> rightHandLandmarksStreamPoller;
    private NormalizedLandmarkListPacket rightHandLandmarksPacket;

    private const string poseLandmarksPresenceStream = "pose_landmarks_presence";
    private OutputStreamPoller<bool> poseLandmarksPresenceStreamPoller;
    private BoolPacket poseLandmarksPresencePacket;

    private const string leftHandLandmarksPresenceStream = "left_hand_landmarks_presence";
    private OutputStreamPoller<bool> leftHandLandmarksPresenceStreamPoller;
    private BoolPacket leftHandLandmarksPresencePacket;

    private const string rightHandLandmarksPresenceStream = "right_hand_landmarks_presence";
    private OutputStreamPoller<bool> rightHandLandmarksPresenceStreamPoller;
    private BoolPacket rightHandLandmarksPresencePacket;

    private SidePacket sidePacket;

    private void Awake()
    {
        //if (drawNodes)
        //    gameObject.AddComponent<BodyDetectAnnotationController>();
    }

    public override Status StartRun()
    {
        poseLandmarksStreamPoller = graph.AddOutputStreamPoller<NormalizedLandmarkList>(poseLandmarksStream).Value();
        poseLandmarksPacket = new NormalizedLandmarkListPacket();

        leftHandLandmarksStreamPoller = graph.AddOutputStreamPoller<NormalizedLandmarkList>(leftHandLandmarksStream).Value();
        leftHandLandmarksPacket = new NormalizedLandmarkListPacket();

        rightHandLandmarksStreamPoller = graph.AddOutputStreamPoller<NormalizedLandmarkList>(rightHandLandmarksStream).Value();
        rightHandLandmarksPacket = new NormalizedLandmarkListPacket();


        poseLandmarksPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(poseLandmarksPresenceStream).Value();
        poseLandmarksPresencePacket = new BoolPacket();

        leftHandLandmarksPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(leftHandLandmarksPresenceStream).Value();
        leftHandLandmarksPresencePacket = new BoolPacket();

        rightHandLandmarksPresenceStreamPoller = graph.AddOutputStreamPoller<bool>(rightHandLandmarksPresenceStream).Value();
        rightHandLandmarksPresencePacket = new BoolPacket();

        sidePacket = new SidePacket();
        sidePacket.Emplace("enable_iris_detection", new BoolPacket(false));

        return graph.StartRun(sidePacket);
    }

    public override void RenderOutput(WebCamScreenController screenController, TextureFrame textureFrame)
    {
        var bodyDetectionValue = FetchNextHolisticValue();
        RenderAnnotation(screenController, bodyDetectionValue);

        screenController.DrawScreen(textureFrame);
    }

    private BodyDetectionValue FetchNextHolisticValue()
    {
        var isPoseLandmarksPresent = FetchNextPoseLandmarksPresence();
        var isLeftHandLandmarksPresent = FetchNextLeftHandLandmarksPresence();
        var isRightHandLandmarksPresent = FetchNextRightHandLandmarksPresence();

        var poseLandmarks = isPoseLandmarksPresent ? FetchNextPoseLandmarks() : new NormalizedLandmarkList();
        var leftHandLandmarks = isLeftHandLandmarksPresent ? FetchNextLeftHandLandmarks() : new NormalizedLandmarkList();
        var rightHandLandmarks = isRightHandLandmarksPresent ? FetchNextRightHandLandmarks() : new NormalizedLandmarkList();

        return new BodyDetectionValue(poseLandmarks, leftHandLandmarks, rightHandLandmarks);
    }

    private NormalizedLandmarkList FetchNextPoseLandmarks()
    {
        return FetchNext(poseLandmarksStreamPoller, poseLandmarksPacket, poseLandmarksStream);
    }

    private NormalizedLandmarkList FetchNextLeftHandLandmarks()
    {
        return FetchNext(leftHandLandmarksStreamPoller, leftHandLandmarksPacket, leftHandLandmarksStream);
    }

    private NormalizedLandmarkList FetchNextRightHandLandmarks()
    {
        return FetchNext(rightHandLandmarksStreamPoller, rightHandLandmarksPacket, rightHandLandmarksStream);
    }

    private bool FetchNextPoseLandmarksPresence()
    {
        return FetchNext(poseLandmarksPresenceStreamPoller, poseLandmarksPresencePacket, poseLandmarksPresenceStream);
    }

    private bool FetchNextLeftHandLandmarksPresence()
    {
        return FetchNext(leftHandLandmarksPresenceStreamPoller, leftHandLandmarksPresencePacket, leftHandLandmarksPresenceStream);
    }

    private bool FetchNextRightHandLandmarksPresence()
    {
        return FetchNext(rightHandLandmarksPresenceStreamPoller, rightHandLandmarksPresencePacket, rightHandLandmarksPresenceStream);
    }

    private void RenderAnnotation(WebCamScreenController screenController, BodyDetectionValue value)
    {
        if (drawNodes)
            GetComponent<BodyDetectAnnotationController>().Draw(screenController.transform, value.PoseLandmarks, value.LeftHandLandmarks, value.RightHandLandmarks, false);

        HitRangeManager.Inst.Draw(value.PoseLandmarks, value.RightHandLandmarks, value.LeftHandLandmarks);
    }

    protected override void PrepareDependentAssets()
    {
        PrepareDependentAsset("hand_landmark.bytes");
        PrepareDependentAsset("hand_recrop.bytes");
        PrepareDependentAsset("handedness.txt");
        PrepareDependentAsset("palm_detection.bytes");
        PrepareDependentAsset("pose_landmark_full_body.bytes");
    }
}
