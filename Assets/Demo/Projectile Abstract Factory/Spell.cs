using System;
using UnityEngine;

namespace Demo.Projectile_Abstract_Factory
{
    public class Spell : MonoBehaviour,IAbstractSpell
    {
        public void SetAoc(AnimatorOverrideController aoc) => GetAnimator().runtimeAnimatorController = aoc;
        public void Enable(bool enable) => gameObject.SetActive(enable);
        public void DestroyParent() => Destroy(transform.parent.gameObject);
        public Animator GetAnimator() => GetComponent<Animator>();
        public void Translate(IAbstractPointer pointer) => transform.position = pointer.GetPosition();
    }
    public interface IAbstractSpell
    {
        void SetAoc(AnimatorOverrideController aoc);
        void Enable(bool enable);
        void DestroyParent();
        Animator GetAnimator();
        void Translate(IAbstractPointer pointer);
    }
}