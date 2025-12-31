using System;

namespace MGC.Physics.Mechanics.Kinematics
{
    /// <summary>
    /// Provides helper methods for circular (rotational) motion kinematics in one dimension.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class contains formulas for uniform and uniformly accelerated circular motion.
    /// All angular quantities are expressed in radians unless explicitly stated otherwise.
    /// </para>
    /// <para>
    /// The sign of angular velocity and angular acceleration represents the direction of rotation.
    /// Period and frequency are always returned as non-negative values.
    /// </para>
    /// <para>
    /// SI units are used throughout:
    /// angles in radians (rad), time in seconds (s),
    /// angular velocity in radians per second (rad/s),
    /// angular acceleration in radians per second squared (rad/s²),
    /// linear quantities in meters (m) and meters per second (m/s).
    /// </para>
    /// </remarks>
    public static class CircularMotion
    {
        /// <summary>
        /// Converts degrees to radians.
        /// </summary>
        private static double DegToRad(double degrees) => degrees * System.Math.PI / 180.0;

        /// <summary>
        /// Converts radians to degrees.
        /// </summary>
        private static double RadToDeg(double radians) => radians * 180.0 / System.Math.PI;

        /// <summary>
        /// Computes the arc length for circular motion.
        /// </summary>
        /// <param name="radius">Circle radius (m).</param>
        /// <param name="angleRadians">Angular displacement in radians.</param>
        /// <returns>Arc length (m): s = r · φ.</returns>
        public static double ArcLength(double radius, double angleRadians)
        {
            return radius * angleRadians;
        }

        /// <summary>
        /// Computes the arc length from an angular displacement given in degrees.
        /// </summary>
        /// <param name="radius">Circle radius (m).</param>
        /// <param name="angleDegrees">Angular displacement in degrees.</param>
        /// <returns>Arc length (m).</returns>
        public static double ArcLengthFromDegrees(double radius, double angleDegrees)
        {
            return radius * DegToRad(angleDegrees);
        }

        /// <summary>
        /// Computes angular displacement from arc length.
        /// </summary>
        /// <param name="arcLength">Arc length (m).</param>
        /// <param name="radius">Circle radius (m). Must be non-zero.</param>
        /// <returns>Angular displacement (rad): φ = s / r.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="radius"/> is zero.</exception>
        public static double AngularDisplacement(double arcLength, double radius)
        {
            if (radius == 0)
            {
                throw new ArgumentException("Radius must be non-zero.", nameof(radius));
            }

            return arcLength / radius;
        }

        /// <summary>
        /// Computes angular displacement for uniformly accelerated circular motion.
        /// </summary>
        /// <param name="initialAngularVelocity">Initial angular velocity (rad/s).</param>
        /// <param name="angularAcceleration">Constant angular acceleration (rad/s²).</param>
        /// <param name="time">Time interval (s).</param>
        /// <returns>Angular displacement (rad): φ = ω₀t + ½αt².</returns>
        public static double AngularDisplacement(double initialAngularVelocity, double angularAcceleration, double time)
        {
            return initialAngularVelocity * time + angularAcceleration * System.Math.Pow(time, 2) / 2.0;
        }

        /// <summary>
        /// Computes angular velocity from angular displacement expressed in radians.
        /// </summary>
        /// <param name="angleRadians">Angular displacement (rad).</param>
        /// <param name="time">Time interval (s). Must be non-zero.</param>
        /// <returns>Angular velocity (rad/s): ω = φ / t.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="time"/> is zero.</exception>
        public static double AngularVelocityRadians(double angleRadians, double time)
        {
            if (time == 0)
            {
                throw new ArgumentException("Time must be non-zero.", nameof(time));
            }

            return angleRadians / time;
        }

        /// <summary>
        /// Computes angular velocity from angular displacement expressed in degrees.
        /// </summary>
        /// <param name="angleDegrees">Angular displacement (degrees).</param>
        /// <param name="time">Time interval (s). Must be non-zero.</param>
        /// <returns>Angular velocity (rad/s).</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="time"/> is zero.</exception>
        public static double AngularVelocityDegrees(double angleDegrees, double time)
        {
            if (time == 0)
            {
                throw new ArgumentException("Time must be non-zero.", nameof(time));
            }

            return DegToRad(angleDegrees) / time;
        }

        /// <summary>
        /// Computes final angular velocity for uniformly accelerated circular motion.
        /// </summary>
        /// <param name="initialAngularVelocity">Initial angular velocity (rad/s).</param>
        /// <param name="angularAcceleration">Constant angular acceleration (rad/s²).</param>
        /// <param name="time">Time interval (s).</param>
        /// <returns>Final angular velocity (rad/s): ω = ω₀ + αt.</returns>
        public static double FinalAngularVelocity(double initialAngularVelocity, double angularAcceleration, double time)
        {
            return initialAngularVelocity + angularAcceleration * time;
        }

        /// <summary>
        /// Computes angular acceleration from initial and final angular velocities.
        /// </summary>
        /// <param name="initialAngularVelocity">Initial angular velocity (rad/s).</param>
        /// <param name="angularVelocity">Final angular velocity (rad/s).</param>
        /// <param name="time">Time interval (s). Must be non-zero.</param>
        /// <returns>Angular acceleration (rad/s²): α = (ω − ω₀) / t.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="time"/> is zero.</exception>
        public static double AngularAcceleration(double initialAngularVelocity, double angularVelocity, double time)
        {
            if (time == 0)
            {
                throw new ArgumentException("Time must be non-zero.", nameof(time));
            }

            return (angularVelocity - initialAngularVelocity) / time;
        }

        /// <summary>
        /// Computes time required to change angular velocity with constant angular acceleration.
        /// </summary>
        /// <param name="initialAngularVelocity">Initial angular velocity (rad/s).</param>
        /// <param name="angularVelocity">Final angular velocity (rad/s).</param>
        /// <param name="angularAcceleration">Constant angular acceleration (rad/s²). Must be non-zero.</param>
        /// <returns>Time interval (s).</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="angularAcceleration"/> is zero.</exception>
        public static double TimeFromAngularVelocities(double initialAngularVelocity, double angularVelocity, double angularAcceleration)
        {
            if (angularAcceleration == 0)
            {
                throw new ArgumentException("Angular acceleration must be non-zero.", nameof(angularAcceleration));
            }

            return (angularVelocity - initialAngularVelocity) / angularAcceleration;
        }

        /// <summary>
        /// Computes linear (tangential) velocity from angular velocity.
        /// </summary>
        /// <param name="angularVelocity">Angular velocity (rad/s).</param>
        /// <param name="radius">Circle radius (m).</param>
        /// <returns>Linear velocity (m/s): v = ωr.</returns>
        public static double LinearVelocity(double angularVelocity, double radius)
        {
            return angularVelocity * radius;
        }

        /// <summary>
        /// Computes angular velocity from linear velocity.
        /// </summary>
        /// <param name="linearVelocity">Linear velocity (m/s).</param>
        /// <param name="radius">Circle radius (m). Must be non-zero.</param>
        /// <returns>Angular velocity (rad/s): ω = v / r.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="radius"/> is zero.</exception>
        public static double AngularVelocityFromLinear(double linearVelocity, double radius)
        {
            if (radius == 0)
            {
                throw new ArgumentException("Radius must be non-zero.", nameof(radius));
            }

            return linearVelocity / radius;
        }

        /// <summary>
        /// Computes tangential acceleration.
        /// </summary>
        /// <param name="angularAcceleration">Angular acceleration (rad/s²).</param>
        /// <param name="radius">Circle radius (m).</param>
        /// <returns>Tangential acceleration (m/s²): aₜ = αr.</returns>
        public static double TangentialAcceleration(double angularAcceleration, double radius)
        {
            return angularAcceleration * radius;
        }

        /// <summary>
        /// Computes centripetal acceleration from linear velocity.
        /// </summary>
        /// <param name="linearVelocity">Linear velocity (m/s).</param>
        /// <param name="radius">Circle radius (m). Must be non-zero.</param>
        /// <returns>Centripetal acceleration (m/s²): aₙ = v² / r.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="radius"/> is zero.</exception>
        public static double CentripetalAccelerationFromLinear(double linearVelocity, double radius)
        {
            if (radius == 0)
            {
                throw new ArgumentException("Radius must be non-zero.", nameof(radius));
            }
            return System.Math.Pow(linearVelocity, 2) / radius;
        }

        /// <summary>
        /// Computes centripetal acceleration from angular velocity.
        /// </summary>
        /// <param name="angularVelocity">Angular velocity (rad/s).</param>
        /// <param name="radius">Circle radius (m). Must be non-zero.</param>
        /// <returns>Centripetal acceleration (m/s²): aₙ = ω²r.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="radius"/> is zero.</exception>
        public static double CentripetalAccelerationFromAngular(double angularVelocity, double radius)
        {
            if (radius == 0)
            {
                throw new ArgumentException("Radius must be non-zero.", nameof(radius));
            }
            return System.Math.Pow(angularVelocity, 2) * radius;
        }

        /// <summary>
        /// Computes the magnitude of total acceleration in circular motion.
        /// </summary>
        /// <param name="tangentialAcceleration">Tangential acceleration (m/s²).</param>
        /// <param name="centripetalAcceleration">Centripetal acceleration (m/s²).</param>
        /// <returns>Total acceleration magnitude (m/s²).</returns>
        public static double TotalAcceleration(double tangentialAcceleration, double centripetalAcceleration)
        {
            return System.Math.Sqrt(System.Math.Pow(tangentialAcceleration, 2) + System.Math.Pow(centripetalAcceleration, 2));
        }

        /// <summary>
        /// Computes the period of rotation from angular velocity.
        /// </summary>
        /// <param name="angularVelocity">Angular velocity (rad/s). Must be non-zero.</param>
        /// <returns>Period of rotation (s): T = 2π / |ω|.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="angularVelocity"/> is zero.</exception>
        public static double PeriodFromAngularVelocity(double angularVelocity)
        {
            if (angularVelocity == 0)
            {
                throw new ArgumentException("Angular velocity must be non-zero.", nameof(angularVelocity));
            }
            return 2.0 * System.Math.PI / System.Math.Abs(angularVelocity);
        }

        /// <summary>
        /// Computes the period of rotation from frequency.
        /// </summary>
        /// <param name="frequency">Frequency (Hz). Must be non-zero.</param>
        /// <returns>Period of rotation (s): T = 1 / |f|.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="frequency"/> is zero.</exception>
        public static double PeriodFromFrequency(double frequency)
        {
            if (frequency == 0)
            {
                throw new ArgumentException("Frequency must be non-zero.", nameof(frequency));
            }
            return 1.0 / System.Math.Abs(frequency);
        }

        /// <summary>
        /// Computes frequency from the period of rotation.
        /// </summary>
        /// <param name="period">Period of rotation (s). Must be non-zero.</param>
        /// <returns>Frequency (Hz): f = 1 / |T|.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="period"/> is zero.</exception>
        public static double FrequencyFromPeriod(double period)
        {
            if (period == 0)
            {
                throw new ArgumentException("Period must be non-zero.", nameof(period));
            }
            return 1.0 / System.Math.Abs(period);
        }

        /// <summary>
        /// Computes frequency from angular velocity.
        /// </summary>
        /// <param name="angularVelocity">Angular velocity (rad/s). Must be non-zero.</param>
        /// <returns>Frequency (Hz): f = |ω| / (2π).</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="angularVelocity"/> is zero.</exception>
        public static double FrequencyFromAngularVelocity(double angularVelocity)
        {
            if (angularVelocity == 0)
            {
                throw new ArgumentException("Angular velocity must be non-zero.", nameof(angularVelocity));
            }
            return System.Math.Abs(angularVelocity) / (2.0 * System.Math.PI);
        }

        /// <summary>
        /// Computes the final angular speed using the time-independent kinematic equation.
        /// </summary>
        /// <param name="omega0">Initial angular velocity (rad/s).</param>
        /// <param name="angularAcceleration">Angular acceleration (rad/s²).</param>
        /// <param name="angularDisplacement">Angular displacement (rad).</param>
        /// <returns>Final angular speed (rad/s).</returns>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when the expression under the square root is negative (no real motion).
        /// </exception>
        /// <remarks>
        /// This method returns the magnitude of angular velocity, not its direction.
        /// </remarks>
        public static double FinalAngularSpeedFromDisplacement(double omega0, double angularAcceleration, double angularDisplacement)
        {
            double value = System.Math.Pow(omega0, 2) + 2.0 * angularAcceleration * angularDisplacement;

            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(
                    nameof(angularDisplacement),
                    value,
                    "No real angular motion exists for the given parameters (negative value under square root)."
                );
            }
            return System.Math.Sqrt(value);
        }

        /// <summary>
        /// Computes angular displacement using the time-independent kinematic equation.
        /// </summary>
        /// <param name="omega0">Initial angular velocity (rad/s).</param>
        /// <param name="omega">Final angular velocity (rad/s).</param>
        /// <param name="angularAcceleration">Angular acceleration (rad/s²). Must be non-zero.</param>
        /// <returns>Angular displacement (rad): φ = (ω² − ω₀²) / (2α).</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="angularAcceleration"/> is zero.</exception>
        public static double AngularDisplacementFromVelocities(double omega0, double omega, double angularAcceleration)
        {
            if (angularAcceleration == 0)
            {
                throw new ArgumentException("Angular acceleration must be non-zero.", nameof(angularAcceleration));
            }
            return (System.Math.Pow(omega, 2) - System.Math.Pow(omega0, 2)) / (2.0 * angularAcceleration);
        }
    }
}
