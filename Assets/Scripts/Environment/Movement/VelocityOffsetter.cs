using System.Collections.Generic;
using UnityEngine;
using PSEMO.Environment.Functionality;

namespace PSEMO.Environment.Movement
{
    public class VelocityOffsetter : MonoBehaviour
    {
        [SerializeField] private GameObject _mover;
        private IMover mover;

        private List<IVelocityOffsettable> offsettables = new();

        void Awake()
        {
            mover = _mover.GetComponent<IMover>();
        }

        private void OnCollisionEnter2D(Collision2D col)
        {
            HandleEnter(col.collider);
        }

        private void OnTriggerEnter2D(Collider2D col)
        {
            HandleEnter(col);
        }

        private void OnCollisionExit2D(Collision2D col)
        {
            HandleExit(col.collider);
        }

        private void OnTriggerExit2D(Collider2D col)
        {
            HandleExit(col);
        }

        private void HandleEnter(Collider2D col)
        {
            if (col.TryGetComponent(out IVelocityOffsettable offsettable))
            {
                if (!offsettables.Contains(offsettable))
                {
                    offsettables.Add(offsettable);
                    offsettable.AddVelocityOffset(mover);
                }
            }
        }

        private void HandleExit(Collider2D col)
        {
            if (col.TryGetComponent(out IVelocityOffsettable offsettable))
            {
                if (offsettables.Contains(offsettable))
                {
                    offsettables.Remove(offsettable);
                    offsettable.RemoveVelocityOffset(mover);
                }
            }
        }
    }
}
