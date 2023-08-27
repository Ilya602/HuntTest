using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Assets.Scripts.Scriptables;
using Assets.Scripts.General;
using PathCreation.Examples;
using PathCreation;

namespace Assets.Scripts.Hunt
{
    public class Target : MonoBehaviour
    {
        [SerializeField] private TargetUI uiCont;
        [SerializeField] private TargetData data;
        public TargetData Data => data;

        private Animator anim;
        private PathFollower follower;

        private int health;
        public int Health => health;


        public void Initialize(PathCreator path)
        {
            uiCont.Initialize();

            anim = GetComponent<Animator>();
            follower = GetComponent<PathFollower>();

            health = data.Health;
            follower.speed = data.MoveSpeed;
            follower.pathCreator = path;
            EventManager.Invoke("update-target-ui");
            anim.SetTrigger("Move");
        }

        public void GetDamage(int damage)
        {
            health -= damage;
            EventManager.Invoke("update-target-ui");

            if (health <= 0)
            {
                // Game Over
            }
        }
    }
}
