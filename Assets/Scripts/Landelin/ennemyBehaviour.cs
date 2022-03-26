using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.AI;

public class ennemyBehaviour : MonoBehaviour
{
    private ennemyStatus status;
    private NavMeshAgent m_navMeshAgent;

    [SerializeField] private Animator m_animator;
    private bool m_animationRunning;

    private RaycastHit m_hit;

    private bool m_velocityIncrease;
    private float m_lastMagnitude;

    private float m_waitBeforeAttack;

    void Start()
    {
        m_velocityIncrease = true;
        m_animationRunning = false;
        m_navMeshAgent = this.GetComponent<NavMeshAgent>();
        status = this.GetComponent<ennemyStatus>();

        //m_navMeshAgent.SetDestination(status.getTarget().transform.position);

    }

    void Update()
    {
        //Go to target position

        if(m_navMeshAgent.velocity.magnitude != 0)
        {
            m_animator.SetBool("Walk", true);
            if (m_navMeshAgent.velocity.magnitude > m_lastMagnitude)
                m_velocityIncrease = true;

            else
                m_velocityIncrease = false;

            m_lastMagnitude = m_navMeshAgent.velocity.magnitude;
        }

        if (m_navMeshAgent.velocity.magnitude <= 0.2f && !m_velocityIncrease)
        {
            m_animator.SetBool("Walk", false); 
            
            float difx = this.transform.position.x - status.getTarget().transform.position.x;
            float difz = this.transform.position.z - status.getTarget().transform.position.z;

            // S'il a atteint son objectif
            if (Mathf.Abs(difx) < .5f && Mathf.Abs(difz) < .5f)
            {
                StartCoroutine(launchAnimation(2, 1.5f));
            }
            else
            {
                // Il faut que le zombie se tourne vers son objectif
                if (Mathf.Abs(difx) < Mathf.Abs(difz))
                {
                    //Si au dessus
                    if (difz >= 0)
                    {
                        if (Physics.Raycast(transform.position, Vector3.back, out m_hit) && !m_animationRunning)
                        {
                            GameObject hitObject = m_hit.transform.gameObject;
                            StartCoroutine(attackFence(hitObject));
                        }
                    }
                    else
                    {
                        if (Physics.Raycast(transform.position, Vector3.forward, out m_hit) && !m_animationRunning)
                        {
                            GameObject hitObject = m_hit.transform.gameObject;
                            StartCoroutine(attackFence(hitObject));
                        }
                    }
                }
                else
                {
                    //Si à droite
                    if (difx >= 0)
                    {
                        if (Physics.Raycast(transform.position, Vector3.left, out m_hit) && !m_animationRunning)
                        {
                            GameObject hitObject = m_hit.transform.gameObject;
                            StartCoroutine(attackFence(hitObject));
                        }
                    }
                    else
                    {
                        if (Physics.Raycast(transform.position, Vector3.right, out m_hit) && !m_animationRunning)
                        {
                            GameObject hitObject = m_hit.transform.gameObject;
                            StartCoroutine(attackFence(hitObject));
                        }
                    }
                }
            }

        }

        if (Input.GetMouseButtonDown(0))
        {
            SetDestinationToMousePosition();
        }
    }

    void SetDestinationToMousePosition()
    {
        RaycastHit hit;
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition);
        if (Physics.Raycast(ray, out hit))
        {
            m_navMeshAgent.SetDestination(hit.point);
        }
    }

    private IEnumerator attackFence(GameObject hitObject)
    {
        m_velocityIncrease = true;
        float waitBeforeAttack = m_waitBeforeAttack;
        WaitForSeconds wait = new WaitForSeconds(1);
        while (hitObject)
        {
            //StartCoroutine(launchAnimation(0, waitBeforeAttack - 1.5f));
            while (waitBeforeAttack > 0)
            {
                yield return wait;
                waitBeforeAttack--;

                if (waitBeforeAttack <= 0)
                {
                    //StartCoroutine(launchAnimation(1, 1.5f));
                }
            }
            waitBeforeAttack = m_waitBeforeAttack;
        }
        if (!hitObject)
        {
            m_animator.SetBool("Walk", true);
            //m_navMeshAgent.SetDestination(status.getTarget().transform.position);
        }

    }

    private IEnumerator launchAnimation(int AnimationNumber, float time)
    {
        m_animationRunning = true;
        animationChangeStatus(AnimationNumber);

        yield return new WaitForSeconds(time);

        m_animationRunning = false;
        animationChangeStatus(AnimationNumber);
        
        if(AnimationNumber == 2)
        {
            Destroy(status.getTarget());
            Destroy(gameObject);
        }
    }

    private void animationChangeStatus(int x)
    {
        switch (x)
        {
            case 0:
                m_animator.SetBool("Idle", m_animationRunning);
                break;

            case 1:
                m_animator.SetBool("Attack", m_animationRunning);
                break;

            case 2:
                m_animator.SetBool("Eat", m_animationRunning);
                break;

        }
    }

}
