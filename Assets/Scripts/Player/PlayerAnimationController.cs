using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator anim;
    private bool onMetal = true;
    private bool onGround = true;
    private int platformMask;
    private int metalMask;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
    }

    private void Start()
    {
        platformMask = 1 << LayerMask.NameToLayer("Platform");
        metalMask = 1 << LayerMask.NameToLayer("Metal");
    }

    private void Update()
    {
        GroundCheck();        
    }

    public float MoveSpeed
    {
        get => anim.GetFloat("movementSpeed");
        set => anim.SetFloat("movementSpeed", value);
    }

    public bool AimModeIs
    {
        get => anim.GetBool("isAimMode");
        set => anim.SetBool("isAimMode", value);
    }

    private void GroundCheck()
    {
        bool ground = Physics.Raycast(transform.position, Vector3.down, 2.0f, platformMask);
        onGround = ground;
        bool metalGround = Physics.Raycast(transform.position, Vector3.down, 2.0f, metalMask);
        onMetal = metalGround;
    }

    public void OnReload()
    {
        anim.SetTrigger("onReload");
    }

    public void Play(string _stateName, int _layer, float _normalizedTime)
    {
        anim.Play(_stateName, _layer, _normalizedTime);
    }

    public bool CurrentAnimationIs(string name)
    {
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name); // 현재 애니메이션 중인지 확인하고 애니메이션 중이라면 이름을 반환
    }

    public void PlayFootstepSound()
    {
        int random1 = Random.Range(0, 5);
        int random2 = Random.Range(0, 3);
        if (onGround)
        {
            switch (random1)
            {
                case 0:
                    SoundManager.instance.Play2DSFX("Footstep_01");
                    break;
                case 1:
                    SoundManager.instance.Play2DSFX("Footstep_02");
                    break;
                case 2:
                    SoundManager.instance.Play2DSFX("Footstep_03");
                    break;
                case 3:
                    SoundManager.instance.Play2DSFX("Footstep_04");
                    break;
                case 4:
                    SoundManager.instance.Play2DSFX("Footstep_05");
                    break;
            }
        }
        else if (onMetal)
        {
            switch (random2)
            {
                case 0:
                    SoundManager.instance.Play2DSFX("Running_On_Metal_01");
                    break;
                case 1:
                    SoundManager.instance.Play2DSFX("Running_On_Metal_02");
                    break;
                case 2:
                    SoundManager.instance.Play2DSFX("Running_On_Metal_03");
                    break;
            }
        }
    }
}
