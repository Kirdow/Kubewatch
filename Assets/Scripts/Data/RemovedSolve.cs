using System;
using System.Collections.Generic;

namespace Kubewatch.Data
{
    public class RemovedSolve
    {
        public Solve Solve { get; set; }
        public RemovedSolve Next { get; set; }

        public RemovedSolve(Solve _solve, RemovedSolve _next)
        {
            Solve = _solve;
            Next = _next;
        }
    
        public RemovedSolve Restore(List<Solve> _solves)
        {
            if (_solves != null && Solve != null)
                _solves.Add(Solve);
            
            return Next;
        }
    }
}