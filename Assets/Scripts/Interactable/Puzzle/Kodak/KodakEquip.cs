using System;
using UnityEngine;
using DG.Tweening;

public class KodakEquip : MonoBehaviour
{
    public bool IsEquipped {get; private set; }
    public bool IsAiming { get; private set; }
    public bool IsTransitioning { get; private set; } // Blocks accidental inputs

    private bool _wantsAim; // Checks player's aiming intention
    
    [Header("Prop")]
    [SerializeField] private Transform cameraProp;

    [Header("Equip Pose")]
    [SerializeField] private Transform startPose;
    [SerializeField] private Transform endPose;
    
    [Header("Aim Pose")]
    [SerializeField] private Transform aimPose;
    
    [Header("Timing")]
    [SerializeField] private float equipDuration = 0.45f;
    [SerializeField] private float aimDuration = 0.25f;
    
    [Header("Ease")]
    [SerializeField] private Ease equipEase = Ease.OutCubic;
    [SerializeField] private Ease unequipEase = Ease.InCubic;
    [SerializeField] private Ease aimEase = Ease.OutCubic;
    [SerializeField] private Ease unAimEase = Ease.OutCubic;
    
    [Header("Cameras")]
    [SerializeField] private Camera playerCamera;
    [SerializeField] private Camera kodakCamera;
    [SerializeField] private AudioListener playerAudioListener;
    [SerializeField] private AudioListener kodakAudioListener;

    private Sequence _sequence;

    private void Awake()
    {
        kodakCamera.enabled = false;
        cameraProp.gameObject.SetActive(false);
    }

    public void ToggleCamera()
    {
        if (IsTransitioning)
        {
            return;
        }

        // Disable Unequip Camera during Aiming
        if (_wantsAim || IsAiming)
        {
            return;
        }

        if (IsEquipped)
        {
            UnEquipCamera();
        }
        else
        {
            EquipCamera();
        }
    }

    private void EquipCamera()
    {
        if (IsEquipped || IsTransitioning)
        {
            return;
        }

        KillTweens();

        IsTransitioning = true;
        IsAiming = false;
        
        cameraProp.gameObject.SetActive(true);
        cameraProp.localPosition = startPose.localPosition;
        cameraProp.localRotation = startPose.localRotation;

        _sequence = DOTween.Sequence();

        _sequence.Join(
            cameraProp.DOLocalMove(endPose.localPosition, equipDuration)
                .SetEase(equipEase)
            );

        _sequence.Join(
            cameraProp.DOLocalRotateQuaternion(endPose.localRotation, equipDuration)
            .SetEase(equipEase)
            );

        _sequence.OnComplete(() =>
        {
            IsEquipped = true;
            IsTransitioning = false;
        });
    }

    private void UnEquipCamera()
    {
        if (!IsEquipped || IsTransitioning || IsAiming || _wantsAim)
        {
            return;
        }
        
        KillTweens();

        IsTransitioning = true;
        IsAiming = false;

        _sequence = DOTween.Sequence();

        _sequence.Join(
            cameraProp.DOLocalMove(startPose.localPosition, equipDuration * 0.8f)
            .SetEase(unequipEase)
        );
        
        _sequence.Join(
            cameraProp.DOLocalRotateQuaternion(startPose.localRotation, equipDuration * 0.8f)
                .SetEase(unequipEase)
        );

        _sequence.OnComplete(() =>
        {
            IsEquipped = false;
            IsTransitioning = false;
            cameraProp.gameObject.SetActive(false);
        });
    }

    public void AimCamera()
    {
        if (!IsEquipped || IsAiming || IsTransitioning)
        {
            return;
        }

        KillTweens();

        IsTransitioning = true;

        _sequence = DOTween.Sequence();

        _sequence.Join(
            cameraProp.DOLocalMove(aimPose.localPosition, aimDuration)
                .SetEase(aimEase)
            );

        _sequence.Join(
            cameraProp.DOLocalRotateQuaternion(aimPose.localRotation, aimDuration)
                .SetEase(aimEase)
        );

        _sequence.OnComplete(() =>
        {
            IsTransitioning = false;
            
            if (!_wantsAim)
            {
                IsAiming = true;
                UnAimCamera();
                return;
            }

            IsAiming = true;
            SetKodakCameraActive(true);
        });
    }
    
    public void UnAimCamera()
    {
        if (!IsEquipped || !IsAiming || IsTransitioning)
        {
            return;
        }

        KillTweens();
        
        SetKodakCameraActive(false);

        IsTransitioning = true;

        _sequence = DOTween.Sequence();

        _sequence.Join(
            cameraProp.DOLocalMove(endPose.localPosition, aimDuration)
                .SetEase(unAimEase)
        );

        _sequence.Join(
            cameraProp.DOLocalRotateQuaternion(endPose.localRotation, aimDuration)
                .SetEase(unAimEase)
        );

        _sequence.OnComplete(() =>
        {
            IsAiming = false;
            IsTransitioning = false;

            if (_wantsAim)
            {
                AimCamera();
            }
        });
    }

    private void KillTweens()
    {
        _sequence?.Kill();
        cameraProp.DOKill();
    }

    private void OnDestroy()
    {
        KillTweens();
    }

    public void SetAiming(bool aiming)
    {
        _wantsAim = aiming;

        if (!IsEquipped)
        {
            SetKodakCameraActive(false);
            return;
        }

        if (IsTransitioning)
        {
            return;
        }

        if (_wantsAim && !IsAiming)
        {
            AimCamera();
        }
        else if (!_wantsAim && IsAiming)
        {
            UnAimCamera();
        }
    }

    private void SetKodakCameraActive(bool active)
    {
        playerCamera.enabled = !active;
        kodakCamera.enabled = active;

        playerAudioListener.enabled = !active;
        kodakAudioListener.enabled = active;
    }
    
    
    /*
     * =====================================
     *  Maybe Later (for code optimizations)
     * =====================================
     */
    
    private void MoveAnim(Sequence sequence, Transform cam, Vector3 endPoint, float duration, Ease ease)
    {
        sequence.Join(
            cam.DOLocalMove(endPoint, duration).SetEase(ease)
        );
    }

    private void RotateAnim(Sequence sequence, Transform cam, Quaternion endRot, float duration, Ease ease)
    {
        sequence.Join(
            cam.DOLocalRotateQuaternion(endRot, duration).SetEase(ease)
        );
    }

    private void InitSequence(Sequence sequence)
    {
        _sequence = sequence;
    }
}
