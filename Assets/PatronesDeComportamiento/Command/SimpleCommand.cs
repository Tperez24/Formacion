using UnityEngine;

namespace PatronesDeComportamiento.Command
{
    public class SimpleCommand : ICommand
    {
        private string _playLoad;

        public SimpleCommand(string playLoad) => _playLoad = playLoad;

        public void Execute()
        {
            Debug.Log("Executed function on simple command");
        }
    }
}