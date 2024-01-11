using Character.Conditions;

namespace Items.Conditional
{
    public class SlowCoin : ConditionalItem
    {
        private void Awake()
        {
            InitializeCondition(typeof(SlowCondition));
        }
    }
}