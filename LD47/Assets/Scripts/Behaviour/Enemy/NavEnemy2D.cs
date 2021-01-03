using Assets.WaveSpawner;
using City;
using PlayerBuilding;
using UnityEngine;
using UnityEngine.AI;

public class NavEnemy2D : Spawnable
{
    enum State
    {
        MoveToCity,
        MoveToTarget,
        AttackTarget
    }

    NavMeshAgent agent;
    Camera cam;
    State state;
    IDamageable target;
    Vector3 cityPosition;

    private void Start()
    {
        agent = GetComponent<NavMeshAgent>();
        agent.enabled = true;
        agent.updateRotation = false;
        agent.updateUpAxis = false;
        cam = Camera.main;
        state = State.MoveToCity;
        cityPosition = FindObjectOfType<CityController>().transform.position;
    }

    private void Update()
    {
        if (Input.GetMouseButtonDown(0))
        {
            var target = cam.ScreenToWorldPoint(Input.mousePosition);
            target.z = 0;
            agent.destination = target;
        }

        switch (state)
        {
            case State.MoveToCity:
                agent.destination = cityPosition;
                FindPossibleTarget();
                break;
            case State.MoveToTarget:
                break;
            case State.AttackTarget:
                break;
        }
    }

    private void FindPossibleTarget()
    {

    }
}
