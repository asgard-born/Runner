namespace Framework.Reactive
{
    public static class ReactiveTriggerExtension
    {
        public static TOut NotifyAndReturnResult<TOut>(this ReactiveTrigger<Return<TOut>> reactiveTrigger)
        {
            Return<TOut> resultContainer = new Return<TOut>();
            reactiveTrigger.Notify(resultContainer);
            return resultContainer.Value;
        }

        public static TOut NotifyAndReturnResult<TIn, TOut>(this ReactiveTrigger<TIn, Return<TOut>> reactiveTrigger, TIn inValue)
        {
            Return<TOut> resultContainer = new Return<TOut>();
            reactiveTrigger.Notify(inValue, resultContainer);
            return resultContainer.Value;
        }

        public static TOut NotifyAndReturnResult<TIn1, TIn2, TOut>(this ReactiveTrigger<TIn1, TIn2, Return<TOut>> reactiveTrigger, TIn1 inValue1, TIn2 inValue2)
        {
            Return<TOut> resultContainer = new Return<TOut>();
            reactiveTrigger.Notify(inValue1, inValue2, resultContainer);
            return resultContainer.Value;
        }

        public static TOut NotifyAndReturnResult<TIn1, TIn2, TIn3, TOut>
            (this ReactiveTrigger<TIn1, TIn2, TIn3, Return<TOut>> reactiveTrigger, TIn1 inValue1, TIn2 inValue2, TIn3 inValue3)
        {
            Return<TOut> resultContainer = new Return<TOut>();
            reactiveTrigger.Notify(inValue1, inValue2, inValue3, resultContainer);
            return resultContainer.Value;
        }
    }
}