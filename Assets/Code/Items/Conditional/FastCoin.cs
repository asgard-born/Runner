using Character.Conditions;

namespace Items.Conditional
{
    public class FastCoin : ConditionalItem
    {
        private void Awake()
        {
            InitializeCondition(typeof(SlowCondition));
        }
    }
}