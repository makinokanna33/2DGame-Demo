using Com.LuisPedroFonseca.ProCamera2D;
using GameFrameWork;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraController : SingletonMono<CameraController>
{
    private ShakePreset Roit;
    private ShakePreset Roar;
    [HideInInspector] public ProCamera2D proCamera2D;
    private ProCamera2DShake shake;

    public Vector2 offset = new Vector2(0, 3.37f);
    protected override void Awake()
    {
        instance = this;
        proCamera2D = GetComponent<ProCamera2D>();
        shake = proCamera2D.GetComponent<ProCamera2DShake>();
        if(shake != null)
        {
            shake.ShakePresets.Clear();
            shake.ShakePresets.Add(ResourcesManager.Instance.LoadAssetSync<ShakePreset>("ShakeData/Roit"));
            shake.ShakePresets.Add(ResourcesManager.Instance.LoadAssetSync<ShakePreset>("ShakeData/Roar"));
        }
    }

    private void Start()
    {
        SetFollowTarget(PlayerController.Instance.currentCharacter.transform);
    }

    public void SetFollowTarget(Transform target)
    {
        proCamera2D.RemoveAllCameraTargets();
        proCamera2D.AddCameraTarget(target, targetOffset: offset);
    }

    public void ShakeCamera(float time, int shakePresetIndex = 0)
    {
        shake.ShakePresets[shakePresetIndex].Duration = time;
        shake.Shake(shakePresetIndex);
    }
}
