namespace Code.UI.Presenters
{
    public class SessionPresenter
    {
        private readonly Ctx _ctx;

        public struct Ctx
        {
            public WinView winView;
            public LooseView looseView;
        }

        public SessionPresenter(Ctx ctx)
        {
            _ctx = ctx;
        }
        
        
    }
}