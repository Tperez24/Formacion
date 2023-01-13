using UnityEngine;

namespace PatronesDeComportamiento.Command
{
    public class ComplexCommand : ICommand
    {
        private Receiver _receiver;
        private string _a,_b;

        public ComplexCommand(Receiver receiver, string a, string b)
        {
            _receiver = receiver;
            _a = a;
            _b = b;
        }

        public void Execute()
        {
            Debug.Log("Executed function on complex command");
            _receiver.DoSomething(_a);
            _receiver.DoSomething(_b);
        }
    }
}