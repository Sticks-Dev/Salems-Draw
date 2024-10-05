using System.Collections;
using UnityEngine;

namespace Salems_Draw
{
    [CreateAssetMenu(menuName = "Salem's Draw/Spell")]
    public class Spell : ScriptableObject
    {
        [SerializeField] private SpellSection[] spellSections;

        public Coroutine Cast(MonoBehaviour source)
        {
            return source.StartCoroutine(CastSpell(source));
        }

        public IEnumerator CastSpell(MonoBehaviour source)
        {
            foreach (var section in spellSections)
            {
                yield return new WaitForSeconds(section.Delay);
                if (section.SpellObject != null)
                    Instantiate(section.SpellObject, source.transform.position + section.ObjectOffset, source.transform.root.rotation);
                yield return new WaitForSeconds(section.Duration);
            }
        }

        [System.Serializable] private struct SpellSection
        {
            [SerializeField] private string animationName;
            [SerializeField] private GameObject spellObject;
            [SerializeField] private Vector3 objectOffset;
            [SerializeField] private float delay;
            [SerializeField] private float duration;

            public readonly string AnimationName => animationName;
            public readonly GameObject SpellObject => spellObject;
            public readonly Vector3 ObjectOffset => objectOffset;
            public readonly float Delay => delay;
            public readonly float Duration => duration;
        }
    }
}
