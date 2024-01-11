using Character.Behaviour;

namespace Items.Behavioural
{
    public class FlyCoin : BehaviouralItem
    {
        private void Awake()
        {
            InitializeBehaviour(typeof(FlyBehaviour));
        }
    }
}