using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Jhu.SpecSvc.Alglib.Wrappers
{
    public enum StopCriterium
    {
        UnsupportedAlgorithms = -10,
                //-10   unsupported combination of algorithm settings:
                //1) StpMax is set to non-zero value,
                //AND 2) non-default preconditioner is used.
                //You can't use both features at the same moment,
                //so you have to choose one of them (and to turn
                //off another one).
        InconsistentConstraints = -3,
                //-3    inconsistent constraints. Feasible point is
                //either nonexistent or too hard to find. Try to
                //restart optimizer with better initial
                //approximation

        DerivativeCheckFailed = -9,
                //* -9    derivative correctness check failed;
                //see Rep.WrongNum, Rep.WrongI, Rep.WrongJ for
                //more information.
        LessThanEpsF = 1,
                //*  1    relative function improvement is no more than
                //EpsF.
        LessThanEpsX = 2,
                //*  2    relative step is no more than EpsX.
        LessThanEpsG = 4,
                //*  4    gradient is no more than EpsG.
        MaxIterations = 5,
                //*  5    MaxIts steps was taken
        StopConditionsStringent = 7,
                //*  7    stopping conditions are too stringent,
                //further improvement is impossible
    }
}
