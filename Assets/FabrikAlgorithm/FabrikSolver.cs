using System;
using UnityEngine;

namespace FabrikAlgorithm
{
    public class FabrikSolver : MonoBehaviour
    {
       [SerializeField] private Transform[] bones;
       private float[] _bonesLengths;

       [SerializeField] private int solverIterations = 2; //Veces que aplicamos fabrik

       [SerializeField] private Transform targetPosition,newTargetPoint;

       private Transform _parent;

       private void Start()
       {
           _parent = transform.parent;
           _bonesLengths = new float[bones.Length];
           
           //Calcular longuitud de cada hueso
           for (var i = 0; i < bones.Length; i++)
           {
               if (i < bones.Length - 1) _bonesLengths[i] = (bones[i + 1].position - bones[i].position).magnitude;
               else //Si es el último hueso
                    _bonesLengths[i] = 0;
           }
       }

       private void Update()
       {
           SolveIk();
           RaycastNewPosition();
       }

       private void SolveIk()
       {
           var finalBonesPosition = new Vector3[bones.Length];
           
           //GuardamosPosicionesActuales
           for (int i = 0; i < bones.Length; i++) finalBonesPosition[i] = bones[i].position;
           
           //Aplicamos fabrik tantas veces como se indique en "solveIterations"
           for (int i = 0; i < solverIterations; i++)
               finalBonesPosition = SolveForwardPositions(SolveInversePositions(finalBonesPosition));
           
           //Aplicamos el resultado a cada hueso
           for (int i = 0; i < bones.Length; i++)
           {
               bones[i].position = finalBonesPosition[i];
               
               
               //Aplicamos Rotaciones
               if(i != bones.Length -1)
                   bones[i].rotation = Quaternion.LookRotation(finalBonesPosition[i + 1] - bones[i].position);
           }
       }
       //Raycast desde el cuerpo hasta el nuevo target para desplazarlo a su nueva posición respecto al suelo
       public void RaycastNewPosition()
       {
           if (Physics.Raycast(newTargetPoint.position, newTargetPoint.TransformDirection(Vector3.down), out var hit))
           {
               newTargetPoint.position =  new Vector3(hit.point.x,hit.point.y + 0.264f,hit.point.z);
           }
       }
       
       //Primera parte, desde hueso libre hasta anclado
       Vector3[] SolveInversePositions(Vector3[] forwardPositions)
       {
           var inversePositions = new Vector3[forwardPositions.Length];

           //Calculamo posiciones desde el último hueso como si estubiera en la target pos hasta el primero
           for (int i = forwardPositions.Length - 1; i >= 0; i--)
           {
               if (i == forwardPositions.Length - 1) //Si es último hueso 
                   inversePositions[i] = targetPosition.position;
               else
               {
                   var primePosition = inversePositions[i + 1];
                   var basePosition = forwardPositions[i];
                   
                   var direction = (basePosition - primePosition).normalized;
                   var distance = _bonesLengths[i];

                   inversePositions[i] = primePosition +(direction * distance);
               }
           }

           return inversePositions;
       }
       
       Vector3[] SolveForwardPositions(Vector3[] inversePositions)
       {
           var forwardPositions = new Vector3[inversePositions.Length];

           //Calculamo posiciones desde el primer hueso como si estuviera en la target pos hasta el último
           for (int i = 0; i < inversePositions.Length; i++)
           {
               if (i == 0) //Si es primer hueso 
                   forwardPositions[i] = bones[0].position;
               else
               {
                   var primePosition = inversePositions[i];
                   var secondLastPrimePosition = forwardPositions[i - 1];
                   
                   var direction = (primePosition - secondLastPrimePosition).normalized;
                   var distance = _bonesLengths[i - 1];

                   forwardPositions[i] = secondLastPrimePosition +(direction * distance);
               }
           }

           return forwardPositions;
       }

       private void OnDrawGizmos()
       {
           Gizmos.color = Color.red;
           Vector3 direction = newTargetPoint.TransformDirection(Vector3.down) * 5;
           Gizmos.DrawRay(newTargetPoint.position, direction);
       }
    }
}
