using Shared;
using UnityEngine;

namespace Character.Behaviour
{
    public class RunBehaviour : CharacterBehaviour
    {
        public RunBehaviour(Ctx ctx) : base(ctx)
        {
            _currentAction = CharacterAction.Moving;
        }

        public override void DoBehave()
        {
            switch (_currentAction)
            {
                case CharacterAction.Moving:
                    OnMove();

                    break;

                case CharacterAction.Jumping:
                    OnJumping();

                    break;

                case CharacterAction.Falling:
                    OnFalling();

                    break;
            }
        }

        protected override void OnButtonUp()
        {
            if (IsGrounded())
            {
                return;
            }

            Jump(_ctx.stats.jumpForce);
            _currentAction = CharacterAction.Jumping;
        }

        protected override void OnButtonDown()
        {
        }

        protected override void OnButtonLeft()
        {
        }

        protected override void OnButtonRight()
        {
        }

        private void Jump(float jumpForce)
        {
            _ctx.rigidbody.AddForce(Vector3.up * jumpForce, ForceMode.VelocityChange);
            _ctx.animatorView.PlayJump();
        }

        private bool IsFalling()
        {
            return _ctx.rigidbody.velocity.y < 0;
        }

        private void OnMove()
        {
            Run(_ctx.stats.speed);
        }

        private void OnJumping()
        {
            if (IsFalling())
            {
                _currentAction = CharacterAction.Falling;
            }
        }

        private void Stop()
        {
            _ctx.rigidbody.isKinematic = true;
        }

        private void Run(float speed)
        {
            if (speed <= 0) return;

            _ctx.rigidbody.MovePosition(_ctx.transform.position + _ctx.transform.forward * speed * Time.fixedDeltaTime);
            _ctx.animatorView.PlayRun();
        }

        private bool IsGrounded()
        {
            if (Physics.Raycast(_ctx.transform.position + new Vector3(0, 0.2f, 0), Vector3.down, .2f))
            {
                if (_ctx.rigidbody.velocity.y <= 0)
                {
                    return true;
                }
            }

            return false;
        }

        private void OnFalling()
        {
            if (IsGrounded())
            {
                _currentAction = CharacterAction.Moving;
            }
            else
            {
                //OnFalling --> View (Rx)
            }
        }
    }
}