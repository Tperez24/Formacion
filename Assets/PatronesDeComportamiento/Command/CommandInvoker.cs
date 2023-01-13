namespace PatronesDeComportamiento.Command
{
    public class CommandInvoker
    {
        private ICommand _onStart,_onEnd;

        public void SetOnStart(ICommand command) => _onStart = command;
        
        public void SetOnEnd(ICommand command) => _onEnd = command;

        public void DoSomethingInportant()
        {
            _onStart?.Execute();
            _onEnd?.Execute();
        }
    }
}