using System;

namespace MGC.Physics.Mechanics.Dynamics
{
    /// <summary>
    /// Provides circular motion dynamics utilities (centripetal force, applied scenarios, and radial vectors).
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class contains formulas related to <b>circular dynamics</b> — primarily the inward (radial)
    /// acceleration and force required to keep an object moving along a circular path.
    /// </para>
    /// <para>
    /// The API is designed for a <b>pure .NET physics core</b> and intentionally avoids Unity or any engine-specific
    /// vector types. Spatial values are represented using raw <c>double</c> coordinates or tuple results,
    /// allowing the library to be used in any environment. A separate adapter layer can later convert these
    /// tuples into engine vectors (e.g., Unity's <c>Vector3</c>).
    /// </para>
    /// <para>
    /// Covered topics include:
    /// <list type="bullet">
    /// <item><description>Centripetal force magnitude from linear speed or angular speed.</description></item>
    /// <item><description>Applied scenarios: minimum speed at the top of a vertical loop and frictionless banked curve angle.</description></item>
    /// <item><description>Radial direction and radial (centripetal) acceleration/force as vectors toward the center.</description></item>
    /// </list>
    /// </para>
    /// <para>
    /// Assumed SI units:
    /// mass in kilograms (kg), distances in meters (m), time in seconds (s),
    /// speed in meters per second (m/s), angular velocity in radians per second (rad/s),
    /// acceleration in meters per second squared (m/s^2), and force in newtons (N).
    /// </para>
    /// <para>
    /// This class does <b>not</b> include rotational dynamics concepts such as torque, moment of inertia,
    /// or angular momentum. Those should be implemented in a separate module (e.g., RotationDynamics/TorqueDynamics).
    /// </para>
    /// </remarks>
    public static class CircularDynamics
    {
        /// <summary>
        /// Computes the magnitude of the centripetal force using linear (tangential) velocity.
        /// </summary>
        /// <param name="mass">Object mass (kg). A mass value of zero is allowed and results in zero force.</param>
        /// <param name="linearVelocity">Tangential (linear) speed (m/s).</param>
        /// <param name="radius">Radius of the circular path (m). Must be greater than zero.</param>
        /// <returns>
        /// Magnitude of the centripetal force (N).
        /// </returns>
        /// <remarks>
        /// <para>
        /// Centripetal force is the inward force required to keep an object moving along a circular path.
        /// The returned value is a <b>magnitude</b> only; direction is always toward the center.
        /// </para>
        /// <para>
        /// Formula:
        /// <c>F = m * v^2 / r</c>
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="radius"/> is not greater than zero,
        /// or when <paramref name="mass"/> is negative.
        /// </exception>
        public static double CentripetalForceFromVelocity(double mass, double linearVelocity, double radius)
        {
            if (radius <= 0)
            {
                throw new ArgumentException("Radius must be greater then zero.", nameof(radius));
            }
            if (mass < 0)
            {
                throw new ArgumentException("Mass must be non negative.", nameof(mass));
            }
            return mass * linearVelocity * linearVelocity / radius;
        }

        /// <summary>
        /// Computes the magnitude of the centripetal force using angular velocity.
        /// </summary>
        /// <param name="mass">Object mass (kg). A mass value of zero is allowed and results in zero force.</param>
        /// <param name="omega">Angular velocity (rad/s).</param>
        /// <param name="radius">Radius of the circular path (m). Must be greater than zero.</param>
        /// <returns>
        /// Magnitude of the centripetal force (N).
        /// </returns>
        /// <remarks>
        /// <para>
        /// This overload is convenient when angular speed is known instead of linear speed.
        /// The returned value is a <b>magnitude</b> only; direction is always toward the center.
        /// </para>
        /// <para>
        /// Formula:
        /// <c>F = m * omega^2 * r</c>
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="radius"/> is not greater than zero,
        /// or when <paramref name="mass"/> is negative.
        /// </exception>
        public static double CentripetalForceFromOmega(double mass, double omega, double radius)
        {
            if (radius <= 0)
            {
                throw new ArgumentException("Radius must be greater then zero.", nameof(radius));
            }
            if (mass < 0)
            {
                throw new ArgumentException("Mass must be non negative.", nameof(mass));
            }
            return mass * omega * omega * radius;
        }

        /// <summary>
        /// Computes the minimum speed required at the top of a vertical loop to maintain contact.
        /// </summary>
        /// <param name="radius">Loop radius (m). Must be greater than zero.</param>
        /// <returns>
        /// Minimum linear speed at the top of the loop (m/s).
        /// </returns>
        /// <remarks>
        /// <para>
        /// Ideal model (no friction/energy loss). At the top of the loop, the minimum-speed condition occurs
        /// when the normal force becomes zero (contact is just maintained).
        /// </para>
        /// <para>
        /// Formula:
        /// <c>v_min = sqrt(g * r)</c>
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="radius"/> is not greater than zero.
        /// </exception>
        public static double MinSpeedAtTopOfVerticalLoop(double radius)
        {
            if (radius <= 0)
            {
                throw new ArgumentException("Radius must be greater then zero.", nameof(radius));
            }
            return Math.Sqrt(PhysicConstants.StandardGravity * radius);
        }

        /// <summary>
        /// Computes the banking angle of a frictionless curve for a given speed and turn radius.
        /// </summary>
        /// <param name="linearVelocity">Linear speed of the object (m/s).</param>
        /// <param name="radius">Turn radius (m). Must be greater than zero.</param>
        /// <returns>
        /// Banking angle in radians.
        /// </returns>
        /// <remarks>
        /// <para>
        /// For a frictionless banked curve, the horizontal component of the normal force provides the needed
        /// centripetal acceleration, while the vertical component balances weight.
        /// </para>
        /// <para>
        /// Formula:
        /// <c>tan(theta) = v^2 / (r * g)</c>
        /// </para>
        /// <para>
        /// The returned angle is measured from the horizontal plane.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// Thrown when <paramref name="radius"/> is not greater than zero.
        /// </exception>
        public static double BankAngleNoFrictionRadians(double linearVelocity, double radius)
        {
            if (radius <= 0)
            {
                throw new ArgumentException("Radius must be greater then zero.", nameof(radius));
            }
            return Math.Atan(linearVelocity * linearVelocity / (radius * PhysicConstants.StandardGravity));
        }

        /// <summary>
        /// Computes a unit direction vector pointing from the current position toward the center.
        /// </summary>
        /// <param name="posX">X-coordinate of the position.</param>
        /// <param name="posY">Y-coordinate of the position.</param>
        /// <param name="posZ">Z-coordinate of the position.</param>
        /// <param name="centerX">X-coordinate of the center.</param>
        /// <param name="centerY">Y-coordinate of the center.</param>
        /// <param name="centerZ">Z-coordinate of the center.</param>
        /// <returns>
        /// A unit vector (x, y, z) that points toward the center.
        /// </returns>
        /// <remarks>
        /// <para>
        /// The returned vector has magnitude 1 and represents direction only.
        /// It is commonly used to build radial (centripetal) acceleration and force vectors.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// Thrown when the position coincides with the center (direction undefined).
        /// </exception>
        public static (double x, double y, double z) DirectionToCenter(
            double posX, double posY, double posZ,
            double centerX, double centerY, double centerZ)
        {
            double[] vector =
            {
                centerX - posX,
                centerY - posY,
                centerZ - posZ
            };

            double length = Math.Sqrt(
                vector[0] * vector[0] +
                vector[1] * vector[1] +
                vector[2] * vector[2]);

            if (length == 0)
            {
                throw new ArgumentException("Position coincides with center; direction to center is undefined.");
            }

            return (vector[0] / length, vector[1] / length, vector[2] / length);
        }

        /// <summary>
        /// Computes the centripetal acceleration vector directed toward the center using linear speed.
        /// </summary>
        /// <param name="linearSpeed">Linear speed (m/s).</param>
        /// <param name="posX">X-coordinate of the position.</param>
        /// <param name="posY">Y-coordinate of the position.</param>
        /// <param name="posZ">Z-coordinate of the position.</param>
        /// <param name="centerX">X-coordinate of the center.</param>
        /// <param name="centerY">Y-coordinate of the center.</param>
        /// <param name="centerZ">Z-coordinate of the center.</param>
        /// <returns>
        /// Centripetal acceleration vector (m/s^2).
        /// </returns>
        /// <remarks>
        /// <para>
        /// Centripetal acceleration changes the direction of velocity (curves the path) and points toward the center.
        /// Its magnitude is <c>a = v^2 / r</c>, applied along the radial direction.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentException">
        /// Thrown when the position coincides with the center (radius is zero).
        /// </exception>
        public static (double ax, double ay, double az) CentripetalAccelerationVector(
            double linearSpeed,
            double posX, double posY, double posZ,
            double centerX, double centerY, double centerZ)
        {
            double dx = centerX - posX;
            double dy = centerY - posY;
            double dz = centerZ - posZ;

            double radius = Math.Sqrt(dx * dx + dy * dy + dz * dz);

            if (radius == 0)
            {
                throw new ArgumentException("Position coincides with center; centripetal acceleration is undefined.");
            }

            double aC = (linearSpeed * linearSpeed) / radius;

            return (
                aC * dx / radius,
                aC * dy / radius,
                aC * dz / radius
            );
        }

        /// <summary>
        /// Computes the centripetal force vector directed toward the center using linear speed.
        /// </summary>
        /// <param name="mass">
        /// Object mass (kg). Must be greater than zero in this vector method.
        /// </param>
        /// <param name="linearSpeed">Linear speed (m/s).</param>
        /// <param name="posX">X-coordinate of the position.</param>
        /// <param name="posY">Y-coordinate of the position.</param>
        /// <param name="posZ">Z-coordinate of the position.</param>
        /// <param name="centerX">X-coordinate of the center.</param>
        /// <param name="centerY">Y-coordinate of the center.</param>
        /// <param name="centerZ">Z-coordinate of the center.</param>
        /// <returns>
        /// Centripetal force vector (N).
        /// </returns>
        /// <remarks>
        /// <para>
        /// This force is the inward force responsible for circular motion.
        /// It is computed as <c>F = m * a</c>, where <c>a</c> is the centripetal acceleration vector.
        /// </para>
        /// <para>
        /// The direction is always toward the center.
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="mass"/> is not greater than zero.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the position coincides with the center (radius is zero).
        /// </exception>
        public static (double fx, double fy, double fz) CentripetalForceVector(
            double mass, double linearSpeed,
            double posX, double posY, double posZ,
            double centerX, double centerY, double centerZ)
        {
            if (mass <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(mass), "Mass must be > 0.");
            }

            double dx = centerX - posX;
            double dy = centerY - posY;
            double dz = centerZ - posZ;

            double r2 = dx * dx + dy * dy + dz * dz;

            if (r2 == 0)
            {
                throw new ArgumentException("Position coincides with center; centripetal force is undefined.");
            }

            double factor = mass * (linearSpeed * linearSpeed) / r2;

            return (
                factor * dx,
                factor * dy,
                factor * dz
            );
        }

        /// <summary>
        /// Computes the centripetal force vector directed toward the center using angular velocity.
        /// </summary>
        /// <param name="mass">
        /// Object mass (kg). Must be greater than zero in this vector method.
        /// </param>
        /// <param name="omega">Angular velocity (rad/s).</param>
        /// <param name="posX">X-coordinate of the position.</param>
        /// <param name="posY">Y-coordinate of the position.</param>
        /// <param name="posZ">Z-coordinate of the position.</param>
        /// <param name="centerX">X-coordinate of the center.</param>
        /// <param name="centerY">Y-coordinate of the center.</param>
        /// <param name="centerZ">Z-coordinate of the center.</param>
        /// <returns>
        /// Centripetal force vector (N).
        /// </returns>
        /// <remarks>
        /// <para>
        /// When angular velocity is known, the centripetal acceleration vector can be written as:
        /// </para>
        /// <para>
        /// <c>a_vec = omega^2 * r_vec</c>
        /// </para>
        /// <para>
        /// where <c>r_vec</c> is the vector pointing from the position toward the center.
        /// The corresponding force vector is:
        /// </para>
        /// <para>
        /// <c>F_vec = mass * omega^2 * r_vec</c>
        /// </para>
        /// </remarks>
        /// <exception cref="ArgumentOutOfRangeException">
        /// Thrown when <paramref name="mass"/> is not greater than zero.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the position coincides with the center (radius is zero).
        /// </exception>
        public static (double fx, double fy, double fz) CentripetalForceVectorFromOmega(
            double mass,
            double omega,
            double posX, double posY, double posZ,
            double centerX, double centerY, double centerZ)
        {
            if (mass <= 0)
            {
                throw new ArgumentOutOfRangeException(nameof(mass), "Mass must be > 0.");
            }

            double dx = centerX - posX;
            double dy = centerY - posY;
            double dz = centerZ - posZ;

            if (dx == 0 && dy == 0 && dz == 0)
            {
                throw new ArgumentException("Position coincides with center; centripetal force is undefined.");
            }

            double factor = mass * (omega * omega);

            return (
                factor * dx,
                factor * dy,
                factor * dz
            );
        }
    }
}
