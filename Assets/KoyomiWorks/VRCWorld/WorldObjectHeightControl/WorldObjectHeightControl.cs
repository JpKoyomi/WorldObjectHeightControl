
using UdonSharp;
using UnityEngine;
using VRC.SDKBase;

public class WorldObjectHeightControl : UdonSharpBehaviour
{
    public const float DefaultMinHeight = 0.2f;
    public const float DefaultMaxHeight = 5.0f;

    [SerializeField]
    private float MinEyeHeight = DefaultMinHeight;

    [SerializeField]
    private float MaxEyeHeight = DefaultMaxHeight;

    [SerializeField]
    private float MinObjectHeight = DefaultMinHeight;

    [SerializeField]
    private float MaxObjectHeight = DefaultMaxHeight;

    [SerializeField]
    private Transform[] transforms = new Transform[0];

    private float EyeHeightRange = DefaultMaxHeight - DefaultMinHeight;
    private float ObjectHeightRange = DefaultMaxHeight - DefaultMinHeight;

    private void Start()
    {
        EyeHeightRange = MaxEyeHeight - MinEyeHeight;
        ObjectHeightRange = MaxObjectHeight - MinObjectHeight;
    }

    public override void OnAvatarChanged(VRCPlayerApi player)
    {
        //base.OnAvatarChanged(player);
        if (Networking.LocalPlayer.playerId != player.playerId)
        {
            return;
        }
        Evaluate(player.GetAvatarEyeHeightAsMeters());
    }

    public override void OnAvatarEyeHeightChanged(VRCPlayerApi player, float eyeHeight)
    {
        //base.OnAvatarEyeHeightChanged(player, eyeHeight);
        if (Networking.LocalPlayer.playerId != player.playerId)
        {
            return;
        }
        player.GetAvatarEyeHeightAsMeters();
        Evaluate(player.GetAvatarEyeHeightAsMeters());
    }

    private void Evaluate(float eyeHeight)
    {
        if (eyeHeight <= 0.0f)
        {
            return;
        }
        float rate = (Mathf.Clamp(eyeHeight, MinEyeHeight, MaxEyeHeight) - MinEyeHeight) / EyeHeightRange;
        float height = ObjectHeightRange * rate + MinObjectHeight;
        for (int i = 0; i < transforms.Length; i++)
        {
            transforms[i].localPosition = new Vector3(transforms[i].localPosition.x, height, transforms[i].localPosition.z);
        }
    }
}
