using System;
using UnityEditor.Timeline.Actions;
using UnityEngine;

namespace PatronesDeComportamiento.Command
{
    public class Programm : MonoBehaviour
    {
        private void Start()
        {
            CommandInvoker invoke = new CommandInvoker();
            invoke.SetOnStart(new SimpleCommand("Holap funciones simples realizadas"));
            
            Receiver receiver = new Receiver();
            invoke.SetOnEnd(new ComplexCommand(receiver,"Send email","Save report"));
            
            invoke.DoSomethingInportant();
        }
    }
}