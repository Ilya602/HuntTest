using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Assets.Scripts.Scriptables;
using PathCreation.Examples;
using PathCreation;
using Cysharp.Threading.Tasks;

namespace Assets.Scripts.Hunt
{
    public class Hunter : MonoBehaviour
    {
        [SerializeField] private GameObject[] models;
        [SerializeField] private HunterData[] huntersData;
        private Animator anim;
        private Rigidbody rb;
        private PathFollower follower;
        private HuntController huntCont;
        private HunterData data;
        public HunterData Data => data;

        private int evoLevel;
        public bool IsJumped { get; set; } = false;


        public void Initialize(int evoLevel, PathCreator path, HuntController huntCont)
        {
            this.evoLevel = evoLevel;
            this.huntCont = huntCont;

            models[evoLevel].SetActive(true);
            anim = models[evoLevel].GetComponent<Animator>();
            rb = GetComponent<Rigidbody>();
            follower = GetComponent<PathFollower>();
            data = huntersData[evoLevel];

            follower.pathCreator = path;
            follower.speed = data.MoveSpeed;
            follower.startPosOffset = new Vector3(Random.Range(-3f, 3f), 0f, Random.Range(-6f, -4f));

            anim.SetTrigger("Move");
            DisableRagdoll(evoLevel);
        }

        async private void Fall(bool applyForce)
        {
            EnableRagdoll(evoLevel, applyForce);

            await UniTask.Delay(huntCont.Data.SwitchHunterTime);

            huntCont.SetNewActiveHunter();
        }

        private void CatchTarget(Target target)
        {
            target.GetDamage(data.Damage);

            Fall(false);
        }

        public void Jump(Vector3 jumpDir)
        {
            IsJumped = true;
            follower.enabled = false;
            rb.isKinematic = false;

            rb.AddForce(jumpDir, ForceMode.Impulse);
            anim.SetTrigger("Win");
        }

        private void Land(Collider other)
        {
            if (IsJumped && rb.velocity.y < 0f)
            {
                if (other.gameObject.CompareTag("Ground"))
                    Fall(true);

                else if (other.TryGetComponent<Target>(out Target target))
                    CatchTarget(target);
            }
        }

        private void EnableRagdoll(int evoLevel, bool applyForce)
        {
            anim.enabled = false;
            rb.isKinematic = true;

            Rigidbody[] rigidbodies = models[evoLevel].transform.GetComponentsInChildren<Rigidbody>();

            foreach (var rb in rigidbodies)
            {
                rb.detectCollisions = true;
                rb.isKinematic = false;

                if (applyForce)
                    rb.AddForce(Vector3.up * 10f, ForceMode.Impulse);
            }
        }

        private void DisableRagdoll(int evoLevel)
        {
            Rigidbody[] rigidbodies = models[evoLevel].transform.GetComponentsInChildren<Rigidbody>();

            foreach (var rb in rigidbodies)
            {
                rb.detectCollisions = false;
                rb.isKinematic = true;
            }
        }

        void OnTriggerEnter(Collider other)
        {
            Land(other);
        }
    }
}
