using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Zenject;

public class DefaultCharacter : MonoBehaviour, ICharacter
{
    [System.Flags] public enum StateFlags
    {
        neutral = 0,
        dead = 1 << 31,
    }

    [SerializeField] StateFlags _state;
    [SerializeField] MonoBehaviour _target;
    [SerializeField] float _health;
    [SerializeField] Damage _baseDamage;
    [SerializeField] float _moveSpeedBase = 10f;
    [SerializeField] float _rotationSpeedBase = 10f;
    [SerializeField] Attack[] _attacks;
    [SerializeField] SimpleFactions _faction;

    private Damage _finalDamage;

    public Attack[] attacks { get => _attacks; set => _attacks = value; }
    public Damage statusDamage;
    public float statusMoveSpeed;
    public float statusRotationSpeed;

    public virtual int stateFlags
    {
        get => (int)_state;
        set => _state = (StateFlags)value;
    }

    public virtual Damage damage 
    {
        get
        {
            _finalDamage.Override(0f);
            _finalDamage.Add(_baseDamage);
            _finalDamage.Add(statusDamage);
            _finalDamage.Clamp(0f, false);
            return _finalDamage;
        }
        private set => _baseDamage = value; 
    }
    public virtual float moveSpeed
    {
        get
        {
            float ret = _moveSpeedBase + statusMoveSpeed;
            if (ret < 0f) ret = 0f;
            return ret;
        }
        private set => _moveSpeedBase = value;
    }
    public virtual float rotationSpeed
    {
        get => _rotationSpeedBase + statusRotationSpeed;
        private set => _rotationSpeedBase = value;
    }

    public IStatsManager         statsManager  { get; private set; }
    public IResistanceManager    resistance    { get; private set; }
    public bool animated                       { get; private set; }
    public Animator anim                       { get; private set; }
    public AnimationStateControl animatedState { get; private set; }
    public IMovement             movement      { get; private set; }

    public IFactionInfo faction => _faction;

    public virtual float health
    {
        get => _health;
        set
        {
            float oldHealth = _health;
            _health = value;
            if (_health <= 0)
            {
                _health = 0;
                Kill();
            }
            HealthChanged?.Invoke(oldHealth, _health);
        }
    }

    public MonoBehaviour asMono => this;

    [Inject] void Initialize(Animator anim, AnimationStateControl ctl, IResistanceManager resistance, IMovement movement, IStatsManager statsManager = null)
    {
        this.anim = anim;
        animatedState = ctl;
        animated = anim != null;
        this.resistance = resistance;
        this.movement = movement;
        this.statsManager = statsManager;
    }

    private void Awake()
    {
        _finalDamage = new Damage();
        _finalDamage.CreateEmptyAllTypes();
    }

    public event Action<float, float> HealthChanged;

    public ICharacter target => (ICharacter)_target;

    public void Attack(Attack attack) => attack.sequence.Play(this, attack.sequenceData, attack.linkedData);

    public virtual void Damage(Damage damage) => health -= resistance.ResistAllAndSum(damage);

    public virtual void Kill() => stateFlags |= (int)StateFlags.dead;

    public void SetHealthRaw(float newHP) => _health = newHP;

    public class Factory : PlaceholderFactory<DefaultCharacter> { }
}