using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimationController : MonoBehaviour
{
    private Animator anim;

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
        return anim.GetCurrentAnimatorStateInfo(0).IsName(name); // ���� �ִϸ��̼� ������ Ȯ���ϰ� �ִϸ��̼� ���̶�� �̸��� ��ȯ
    }
}
