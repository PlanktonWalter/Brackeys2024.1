using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

[RequireComponent(typeof(NavMeshAgent))]
public sealed class Ghost : MonoBehaviour
{

    [SerializeField] private PlayerCharacter _player;

    private NavMeshAgent _agent;
    private State _state = State.Chasing;

    public void StartChase()
    {

    }

    private void Awake()
    {
        _agent = GetComponent<NavMeshAgent>();
    }

    private void Update()
    {
        switch (_state)
        {
            case State.Idle: UpdateIdle(); break;
            case State.Chasing: UpdateChasing(); break;
        }
    }

    private void UpdateIdle()
    {

    }

    private void UpdateChasing()
    {
        _agent.SetDestination(_player.transform.position);
    }

    private enum State
    {
        Idle,
        Chasing,
    }

}
