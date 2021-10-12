using Mediapipe;

class BodyDetectionValue
{
    public readonly NormalizedLandmarkList PoseLandmarks;
    public readonly NormalizedLandmarkList LeftHandLandmarks;
    public readonly NormalizedLandmarkList RightHandLandmarks;

    public BodyDetectionValue(NormalizedLandmarkList PoseLandmarks, NormalizedLandmarkList LeftHandLandmarks, NormalizedLandmarkList RightHandLandmarks)
    {
        this.PoseLandmarks = PoseLandmarks;
        this.LeftHandLandmarks = LeftHandLandmarks;
        this.RightHandLandmarks = RightHandLandmarks;
    }
}
