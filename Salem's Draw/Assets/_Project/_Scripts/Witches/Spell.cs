﻿using System;
using System.Collections;
using UnityEngine;

namespace Salems_Draw
{
    [CreateAssetMenu(menuName = "Salem's Draw/Spell")]
    public class Spell : ScriptableObject
    {
        [SerializeField] private SpellSection[] spellSections;

        public Coroutine Cast(MonoBehaviour source, Func<Vector3> offset)
        {
            return CoroutineHelper.Instance.StartCoroutine(CastSpell(source, offset));
        }

        private IEnumerator CastSpell(MonoBehaviour source, Func<Vector3> offset)
        {
            foreach (var section in spellSections)
            {
                yield return new WaitForSeconds(section.Delay);
                if (section.SpellObject != null)
                    Instantiate(section.SpellObject, source.transform.position + section.ObjectOffset + offset(), source.transform.root.rotation);
                yield return new WaitForSeconds(section.Duration);
            }
        }

        [System.Serializable] private struct SpellSection
        {
            [SerializeField] private GameObject spellObject;
            [SerializeField] private Vector3 objectOffset;
            [SerializeField] private float delay;
            [SerializeField] private float duration;

            public readonly GameObject SpellObject => spellObject;
            public readonly Vector3 ObjectOffset => objectOffset;
            public readonly float Delay => delay;
            public readonly float Duration => duration;
        }
    }
}
