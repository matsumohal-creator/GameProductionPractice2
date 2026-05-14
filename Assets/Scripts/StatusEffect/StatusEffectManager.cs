using System.Collections.Generic;
using UnityEngine;
using UnityEngine.TextCore.Text;

public class StatusEffectManager : MonoBehaviour
{
    public void ProcessTurnEnd(Character character)
    {
        List<StatusEffect> removeList
            = new List<StatusEffect>();

        foreach (var effect in character.statusEffects)
        {
            switch (effect.data.effectType)
            {
                case StatusEffectType.Poison:

                    character.TakeDamage(effect.stack);

                    effect.stack--;

                    break;

                case StatusEffectType.Burn:

                    character.TakeTrueDamage(effect.stack);

                    effect.stack--;

                    break;

                case StatusEffectType.Fatigue:

                    effect.stack--;

                    break;

                case StatusEffectType.Weakness:

                    effect.stack = 0;

                    break;

                case StatusEffectType.Vulnerable:

                    effect.stack = 0;

                    break;
            }

            if (effect.stack <= 0)
            {
                removeList.Add(effect);
            }
        }

        foreach (var remove in removeList)
        {
            character.statusEffects.Remove(remove);
        }
    }
}