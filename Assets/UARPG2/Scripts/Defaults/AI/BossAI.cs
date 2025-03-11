using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossAI : AI
{
    [Serializable] public class Phase
    {
        public float exitTime;
        public float exitHealth;
        public float newHealthBorder;
        public int[] allowedAttacks;
        public Vector3 startingPosition = Vector3.negativeInfinity;
        public Vector3 startingAngles = Vector3.negativeInfinity;
        public StateActions[] states;
    }

    [SerializeField] protected int _currentPhase;
    [SerializeField] protected Phase[] _phases;

    protected Action[] _actions;
    protected Attack[] _allowedAttacks;

    public int currentPhase => _currentPhase;

    private void OnEnable() => owner.HealthChanged += OnCharacterHit;
    private void OnDisable() => owner.HealthChanged -= OnCharacterHit;

    private void OnCharacterHit(float oldHP, float newHP)
    {
        float exitHealth = _phases[_currentPhase].exitHealth;
        if (exitHealth > 0f && newHP <= exitHealth)
        {
            NextPhase();
            owner.SetHealthRaw(exitHealth);
        }
    }

    public override void Initialize()
    {
        _currentPhase = -1;
        NextPhase();
    }

    public void NextPhase() => SetPhase(_currentPhase + 1);
    public void SetPhase(int phase)
    {
        if (phase < 0) enabled = false;
        else if (phase < _phases.Length)
        {
            _currentPhase = phase;
            _states = _phases[_currentPhase].states;

            FormAttacks();
            if (phaseTimer != null) StopCoroutine(phaseTimer);
            if (_phases[_currentPhase].exitTime > 0)
            {
                phaseTimer = PhaseTimer(_phases[_currentPhase].exitTime);
                StartCoroutine(phaseTimer);
            }
            else phaseTimer = null;
        }
        PhaseChange?.Invoke(_currentPhase);
    }

    public event Action<int> PhaseChange;

    private void FormAttacks()
    {
        List<Attack> ret = new List<Attack>();
        for (int i = 0; i < owner.attacks.Length; i++)
        {
            if (_phases[_currentPhase].allowedAttacks.Contains(i)) ret.Add(owner.attacks[i]);
        }
        _allowedAttacks = ret.ToArray();
    }

    private IEnumerator phaseTimer = null;
    private IEnumerator PhaseTimer(float time)
    {
        yield return new WaitForSeconds(time);
        phaseTimer = null;
        NextPhase();
    }
}
