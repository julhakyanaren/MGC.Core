using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace MGC.Physics.Mechanics.Kinematics
{
    public static class KinematicsExtensions
    {
        public static double PeriodFromAngularVelocity(this double angularVelocity)
        {
            return CircularMotion.PeriodFromAngularVelocity(angularVelocity);
        }

        public static double PeriodFromFrequency(this double frequency)
        {
            return CircularMotion.PeriodFromFrequency(frequency);
        }

        public static double FrequencyFormPeriod(this double period)
        {
            return CircularMotion.FrequencyFromPeriod(period);
        }

        public static double FrequencyFormAngularVelocity(this double angularVelocity)
        {
            return CircularMotion.FrequencyFromAngularVelocity(angularVelocity);
        }
    }
}
