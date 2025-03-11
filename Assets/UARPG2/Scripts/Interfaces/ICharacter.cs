using System;
using UnityEngine;
using Zenject;

public interface ICharacter
{
    [Flags] public enum DefaultStateFlags
    {
        dead = 1 << 31,
    }

    public MonoBehaviour asMono { get; }
    public GameObject gameObject { get; }
    public Transform transform { get; }

    public Attack[] attacks { get; set; }

    public int stateFlags { get; set; }

    public Damage damage { get; }
    public float moveSpeed { get; }
    public float rotationSpeed { get; }

    public IFactionInfo faction { get; }

    public ICharacter target { get; }

    public IStatsManager statsManager { get; }
    public IResistanceManager resistance { get; }
    public bool animated { get; }
    public Animator anim { get; }
    public AnimationStateControl animatedState { get; }
    public IMovement movement { get; }

    public float health { get; set; }

    public void Attack(Attack attack);

    public void Damage(Damage damage);

    public void Kill();


    public void SetHealthRaw(float newHp);

    public event Action<float, float> HealthChanged;
}

public class ICharacterFactory : PlaceholderFactory<ICharacter> { }