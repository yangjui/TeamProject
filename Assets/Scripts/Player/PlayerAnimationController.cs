using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator anim;
    private bool onMetal = true;

    private void Awake()
    {
        anim = GetComponentInChildren<Animator>();
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
        int random = Random.Range(0, 5);
        int ran = Random.Range(0, 3);
        if (!onMetal)
        {
            switch (random)
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
            switch (ran)
            {
                case 0:
                    SoundManager.instance.Play2DSFX("Running_On_Metal_01");
                    break;
                case 1:
                    SoundManager.instance.Play2DSFX("Running_On_Metal_03");
                    break;
                case 2:
                    SoundManager.instance.Play2DSFX("Running_On_Metal_04");
                    break;
            }
        }
    }
}
