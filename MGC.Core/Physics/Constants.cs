using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGC.Core.Physics
{
    public static class Constants
    {
        public const double SpeedOfLight = 299792458.0;
        public const double Planck = 6.62607015e-34;
        public static readonly double ReducedPlanck = Planck / (System.Math.PI * 2.0);
        public const double ElementaryCharge = 1.602176634e-19;
        public const double Boltzmann = 1.380649e-23;
        public const double Avogadro = 6.02214076e23;
        public static readonly double GasConstant = Avogadro * Boltzmann;

        public const double GravitationalConstant = 6.67430e-11;
        public const double StandartGravity = 9.80665;

        public const double VacuumPermittivity = 8.8541878128e-12;
        public const double VacuumPermeability = 1.25663706212e-6;
        public static readonly double CoulombConstant = 1.0 / (4.0 * System.Math.PI * VacuumPermittivity);

        public const double StefanBoltzmann = 5.670374419e-8;

        public const double StandardAtmosphere = 101325.0;
        public const double Bar = 100000.0f;
    }
}
