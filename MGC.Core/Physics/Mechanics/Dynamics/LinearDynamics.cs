namespace MGC.Physics.Mechanics.Dynamics
{
    /// <summary>
    /// Linear (translational) dynamics helper methods.
    /// </summary>
    /// <remarks>
    /// <para>
    /// This class contains practical formulas used in classical mechanics for bodies treated as point masses.
    /// It focuses on forces and acceleration in 1D/2D/3D, incline decomposition, friction, springs, and drag.
    /// </para>
    /// <para><b>Sign conventions:</b></para>
    /// <list type="bullet">
    /// <item><description>For 1D methods, direction is represented by the sign (+ / -) of input values.</description></item>
    /// <item><description>For 2D/3D vector-like returns, the result is a tuple of components (Fx, Fy, Fz) or (ax, ay, az).</description></item>
    /// <item><description>Angles are in <b>radians</b> unless stated otherwise.</description></item>
    /// </list>
    /// <para><b>Units:</b></para>
    /// <list type="bullet">
    /// <item><description>Force: Newton (N)</description></item>
    /// <item><description>Mass: kilogram (kg)</description></item>
    /// <item><description>Acceleration: meter per second squared (m/s^2)</description></item>
    /// <item><description>Displacement: meter (m)</description></item>
    /// <item><description>Velocity: meter per second (m/s)</description></item>
    /// <item><description>Spring stiffness: N/m</description></item>
    /// <item><description>Damping coefficient: N·s/m</description></item>
    /// </list>
    /// </remarks>
    public static class LinearDynamics
    {
        /// <summary>
        /// Calculates acceleration from net force and mass using Newton's second law.
        /// </summary>
        /// <param name="force">Net force acting on the body (N).</param>
        /// <param name="mass">Mass of the body (kg). Must be greater than zero.</param>
        /// <returns>Acceleration (m/s^2).</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="mass"/> is less than or equal to zero.</exception>
        public static double Acceleration(double force, double mass)
        {
            if (mass <= 0)
            {
                throw new ArgumentException("Mass must be greater than zero.", nameof(mass));
            }
            return force / mass;
        }
        /// <summary>
        /// Calculates acceleration components in 2D from multiple forces defined by magnitudes and angles.
        /// Angles are measured in radians relative to the +X axis.
        /// </summary>
        /// <param name="mass">Mass of the body (kg). Must be greater than zero.</param>
        /// <param name="magnitudes">Force magnitudes (N).</param>
        /// <param name="anglesRadians">Force angles in radians relative to the +X axis.</param>
        /// <returns>Acceleration components (ax, ay) in m/s^2.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="mass"/> is less than or equal to zero, or arrays have different lengths.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="magnitudes"/> or <paramref name="anglesRadians"/> is null.</exception>
        public static (double x, double y) Acceleration2D(double mass, double[] magnitudes, double[] anglesRadians)
        {
            if (mass <= 0)
            {
                throw new ArgumentException("Mass must be greater than zero.", nameof(mass));
            }

            var (fx, fy) = NetForce2DComponents(magnitudes, anglesRadians);
            return (fx / mass, fy / mass);
        }
        /// <summary>
        /// Calculates acceleration components in 3D from a set of force vectors (components).
        /// </summary>
        /// <param name="mass">Mass of the body (kg). Must be greater than zero.</param>
        /// <param name="forces">Array of forces as (x, y, z) components in Newtons.</param>
        /// <returns>Acceleration components (ax, ay, az) in m/s^2.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="mass"/> is less than or equal to zero.</exception>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="forces"/> is null.</exception>
        public static (double x, double y, double z) Acceleration3D(double mass, (double x, double y, double z)[] forces)
        {
            if (mass <= 0)
            {
                throw new ArgumentException("Mass must be greater than zero.", nameof(mass));
            }
            if (forces == null)
            {
                throw new ArgumentNullException(nameof(forces));
            }
            var (fx, fy, fz) = NetForce3DComponents(forces);
            return (fx / mass, fy / mass, fz / mass);
        }

        /// <summary>
        /// Calculates force from mass and acceleration using Newton's second law.
        /// </summary>
        /// <param name="mass">Mass of the body (kg). Must be greater than zero.</param>
        /// <param name="acceleration">Acceleration (m/s^2).</param>
        /// <returns>Force (N).</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="mass"/> is less than or equal to zero.</exception>
        public static double Force(double mass, double acceleration)
        {
            if (mass <= 0)
            {
                throw new ArgumentException("Mass must be greater than zero.", nameof(mass));
            }
            return mass * acceleration;
        }
        /// <summary>
        /// Calculates mass from force and acceleration: m = F / a.
        /// </summary>
        /// <param name="force">Force (N).</param>
        /// <param name="acceleration">Acceleration (m/s^2). Must be non-zero.</param>
        /// <returns>Mass (kg).</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="acceleration"/> is zero.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the computed mass is negative.</exception>
        
        public static double MassFromForce(double force, double acceleration)
        {
            if (acceleration == 0)
            {
                throw new ArgumentException("Acceleration must be non zero.", nameof(acceleration));
            }
            if (force / acceleration < 0)
            {
                throw new InvalidOperationException("Calculated mass is negative. Check force and acceleration signs.");
            }
            return force / acceleration;
        }

        /// <summary>
        /// Calculates weight (gravitational force) using P = m * g.
        /// </summary>
        /// <param name="mass">Mass (kg). Must be greater than zero.</param>
        /// <returns>Weight (N).</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="mass"/> is less than or equal to zero.</exception>
        public static double Weight(double mass)
        {
            if (mass <= 0)
            {
                throw new ArgumentException("Mass must be greater than zero.", nameof(mass));
            }
            return mass * Constants.StandardGravity;
        }
        /// <summary>
        /// Calculates normal force on a horizontal surface: N = m * g.
        /// </summary>
        /// <param name="mass">Mass (kg). Must be greater than zero.</param>
        /// <returns>Normal force (N).</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="mass"/> is less than or equal to zero.</exception>
       
        public static double NormalForce(double mass)
        {
            if (mass <= 0)
            {
                throw new ArgumentException("Mass must be greater than zero.", nameof(mass));
            }
            return mass * Constants.StandardGravity;
        }
        /// <summary>
        /// Calculates normal force on an incline: N = m * g * cos(angle).
        /// Angle is in radians.
        /// </summary>
        /// <param name="mass">Mass (kg). Must be greater than zero.</param>
        /// <param name="angleRadians">Incline angle in radians.</param>
        /// <returns>Normal force (N).</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="mass"/> is less than or equal to zero.</exception>
        public static double NormalForce(double mass, double angleRadians)
        {
            if (mass <= 0)
            {
                throw new ArgumentException("Mass must be greater than zero.", nameof(mass));
            }
            return mass * Constants.StandardGravity * Math.Cos(angleRadians);
        }

        /// <summary>
        /// Calculates kinetic friction force magnitude: F = μ * N.
        /// </summary>
        /// <param name="frictionCoeff">Coefficient of friction (unitless). Must be non-negative.</param>
        /// <param name="normalForce">Normal force magnitude (N).</param>
        /// <returns>Friction force magnitude (N).</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="frictionCoeff"/> is negative.</exception>
        public static double FrictionForce(double frictionCoeff, double normalForce)
        {
            if (frictionCoeff < 0)
            {
                throw new ArgumentException("Friction coefficient must be non-negative.", nameof(frictionCoeff));
            }
            return frictionCoeff * normalForce;
        }
        /// <summary>
        /// Calculates the maximum static friction magnitude: Fmax = μs * N.
        /// </summary>
        /// <param name="staticFrictionCoeff">Static friction coefficient (unitless). Must be non-negative.</param>
        /// <param name="normalForce">Normal force magnitude (N).</param>
        /// <returns>Maximum static friction magnitude (N).</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="staticFrictionCoeff"/> is negative.</exception>
        public static double MaxStaticFriction(double staticFrictionCoeff, double normalForce)
        {
            if (staticFrictionCoeff < 0)
            {
                throw new ArgumentException("Static friction coefficient must be non-negative.", nameof(staticFrictionCoeff));
            }
            return staticFrictionCoeff * normalForce;
        }
        /// <summary>
        /// Calculates the component of gravity parallel to an incline: F = m * g * sin(angle).
        /// </summary>
        /// <param name="mass">Mass (kg). Must be greater than zero.</param>
        /// <param name="angleRadians">Incline angle in radians.</param>
        /// <returns>Parallel component of gravitational force (N).</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="mass"/> is less than or equal to zero.</exception>
        
        public static double GravityParallel(double mass, double angleRadians)
        {
            if (mass <= 0)
            {
                throw new ArgumentException("Mass must be greater than zero.", nameof(mass));
            }
            return mass * Constants.StandardGravity * Math.Sin(angleRadians);
        }
        /// <summary>
        /// Calculates the component of gravity perpendicular to an incline: F = m * g * cos(angle).
        /// </summary>
        /// <param name="mass">Mass (kg). Must be greater than zero.</param>
        /// <param name="angleRadians">Incline angle in radians.</param>
        /// <returns>Perpendicular component of gravitational force (N).</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="mass"/> is less than or equal to zero.</exception>
        public static double GravityPerpendicular(double mass, double angleRadians)
        {
            if (mass <= 0)
            {
                throw new ArgumentException("Mass must be greater than zero.", nameof(mass));
            }
            return mass * Constants.StandardGravity * Math.Cos(angleRadians);
        }

        /// <summary>
        /// Calculates acceleration with friction along one axis: a = (Fdrive - Ffriction) / m.
        /// </summary>
        /// <param name="drivingForce">Driving force along the axis (N).</param>
        /// <param name="frictionForce">Friction force magnitude along the axis (N).</param>
        /// <param name="mass">Mass (kg). Must be greater than zero.</param>
        /// <returns>Acceleration (m/s^2).</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="mass"/> is less than or equal to zero.</exception>
        public static double AccelerationWithFriction(double drivingForce, double frictionForce, double mass)
        {
            if (mass <= 0)
            {
                throw new ArgumentException("Mass must be greater than zero.", nameof(mass));
            }
            return (drivingForce - frictionForce) / mass;
        }

        /// <summary>
        /// Calculates the resultant force in 1D by summing all forces (with sign).
        /// </summary>
        /// <param name="forces">Forces along a single axis (N). Use sign to indicate direction.</param>
        /// <returns>Net force (N).</returns>
        public static double NetForce1D(params double[] forces)
        {
            double force = 0;
            for (int f = 0; f < forces.Length; f++)
            {
                force += forces[f];
            }
            return force;
        }
        /// <summary>
        /// Calculates the magnitude of the resultant force in 2D from magnitudes and angles.
        /// Angles are measured in radians relative to the +X axis.
        /// </summary>
        /// <param name="magnitudes">Force magnitudes (N).</param>
        /// <param name="anglesRadians">Force angles in radians relative to the +X axis.</param>
        /// <returns>Magnitude of the net force (N).</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="magnitudes"/> or <paramref name="anglesRadians"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when arrays have different lengths.</exception>
        public static double NetForce2D(double[] magnitudes, double[] anglesRadians)
        {
            if (magnitudes == null)
            {
                throw new ArgumentNullException(nameof(magnitudes));
            }
            if (anglesRadians == null)
            {
                throw new ArgumentNullException(nameof(anglesRadians));
            }
            if (magnitudes.Length != anglesRadians.Length)
            {
                throw new ArgumentException("Magnitudes and AnglesRadians must have the same length.");
            }

            double forceX = 0.0;
            double forceY = 0.0;

            for (int i = 0; i < magnitudes.Length; i++)
            {
                double f = magnitudes[i];
                double a = anglesRadians[i];

                forceX += f * Math.Cos(a);
                forceY += f * Math.Sin(a);
            }

            return Math.Sqrt(forceX * forceX + forceY * forceY);
        }
        /// <summary>
        /// Calculates the magnitude of the net force in 3D from force components.
        /// </summary>
        /// <param name="forces">Array of forces as (x, y, z) components in Newtons.</param>
        /// <returns>Magnitude of the net force (N).</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="forces"/> is null.</exception>
        public static double NetForce3D((double x, double y, double z)[] forces)
        {
            var (fx, fy, fz) = NetForce3DComponents(forces);
            return Math.Sqrt(fx * fx + fy * fy + fz * fz);
        }

        /// <summary>
        /// Calculates the resultant force components in 2D from magnitudes and angles.
        /// Angles are measured in radians relative to the +X axis.
        /// </summary>
        /// <param name="magnitudes">Force magnitudes (N).</param>
        /// <param name="anglesRadians">Force angles in radians relative to the +X axis.</param>
        /// <returns>Net force components (Fx, Fy) in Newtons.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="magnitudes"/> or <paramref name="anglesRadians"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when arrays have different lengths.</exception>
        public static (double x, double y) NetForce2DComponents(double[] magnitudes, double[] anglesRadians)
        {
            if (magnitudes == null)
            {
                throw new ArgumentNullException(nameof(magnitudes));
            }
            if (anglesRadians == null)
            {
                throw new ArgumentNullException(nameof(anglesRadians));
            }
            if (magnitudes.Length != anglesRadians.Length)
            {
                throw new ArgumentException("Magnitudes and anglesRadians must have the same length.");
            }

            double forceX = 0.0;
            double forceY = 0.0;

            for (int i = 0; i < magnitudes.Length; i++)
            {
                double f = magnitudes[i];
                double a = anglesRadians[i];

                forceX += f * Math.Cos(a);
                forceY += f * Math.Sin(a);
            }

            return (forceX, forceY);
        }
        /// <summary>
        /// Calculates the net force components in 3D by summing force vectors.
        /// </summary>
        /// <param name="forces">Array of forces as (x, y, z) components in Newtons.</param>
        /// <returns>Net force components (Fx, Fy, Fz) in Newtons.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="forces"/> is null.</exception>
        public static (double x, double y, double z) NetForce3DComponents((double x, double y, double z)[] forces)
        {
            if (forces == null)
            {
                throw new ArgumentNullException(nameof(forces));
            }

            double forceX = 0.0;
            double forceY = 0.0;
            double forceZ = 0.0;

            for (int i = 0; i < forces.Length; i++)
            {
                forceX += forces[i].x;
                forceY += forces[i].y;
                forceZ += forces[i].z;
            }

            return (forceX, forceY, forceZ);
        }

        /// <summary>
        /// Calculates spring restoring force in 1D using Hooke's law: F = -k * x.
        /// </summary>
        /// <param name="stiffness">Spring stiffness k (N/m). Must be non-negative.</param>
        /// <param name="displacement">Displacement from equilibrium x (m). Sign indicates direction.</param>
        /// <returns>Spring force (N).</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="stiffness"/> is negative.</exception>
        public static double SpringForce1D(double stiffness, double displacement)
        {
            if (stiffness < 0)
            {
                throw new ArgumentException("Stiffness must be non-negative.", nameof(stiffness));
            }
            return -stiffness * displacement;
        }
        /// <summary>
        /// Calculates spring restoring force components in 2D using Hooke's law: F = -k * x.
        /// </summary>
        /// <param name="stiffness">Spring stiffness k (N/m).</param>
        /// <param name="displacement">Displacement vector from equilibrium (m).</param>
        /// <returns>Spring force components (Fx, Fy) in Newtons.</returns>
        public static (double x, double y) SpringForce2D(double stiffness,
            (double x, double y) displacement)
        {
            return
                (
                -stiffness * displacement.x,
                -stiffness * displacement.y
                );
        }
        /// <summary>
        /// Calculates spring restoring force components in 3D using Hooke's law: F = -k * x.
        /// </summary>
        /// <param name="stiffness">Spring stiffness k (N/m).</param>
        /// <param name="displacement">Displacement vector from equilibrium (m).</param>
        /// <returns>Spring force components (Fx, Fy, Fz) in Newtons.</returns>
        public static (double x, double y, double z) SpringForce3D(double stiffness,
            (double x, double y, double z) displacement)
        {
            return
                (
                -stiffness * displacement.x,
                -stiffness * displacement.y,
                -stiffness * displacement.z
                );
        }

        /// <summary>
        /// Calculates the magnitude of spring force from displacement magnitude: |F| = k * |x|.
        /// Note: direction is not represented by this method.
        /// </summary>
        /// <param name="stiffness">Spring stiffness k (N/m). Must be non-negative.</param>
        /// <param name="displacementMagnitude">Displacement magnitude |x| (m).</param>
        /// <returns>Spring force magnitude (N).</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="stiffness"/> is negative.</exception>
        public static double SpringForceMagnitude(double stiffness, double displacementMagnitude)
        {
            if (stiffness < 0)
            {
                throw new ArgumentException("Stiffness must be non-negative.", nameof(stiffness));
            }
            return stiffness * displacementMagnitude;
        }

        /// <summary>
        /// Calculates 1D spring-damper force: F = -k * x - c * v.
        /// </summary>
        /// <param name="stiffness">Spring stiffness k (N/m). Must be non-negative.</param>
        /// <param name="damping">Damping coefficient c (N·s/m). Must be non-negative.</param>
        /// <param name="displacement">Displacement from equilibrium x (m).</param>
        /// <param name="velocity">Velocity v (m/s).</param>
        /// <returns>Spring-damper force (N).</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="stiffness"/> or <paramref name="damping"/> is negative.</exception>
        public static double SpringDamperForce1D(double stiffness, double damping, double displacement, double velocity)
        {
            if (stiffness < 0)
            {
                throw new ArgumentException("Stiffness must be non-negative.", nameof(stiffness));
            }
            if (damping < 0)
            {
                throw new ArgumentException("Damping must be non-negative.", nameof(damping));
            }
            return -stiffness * displacement - damping * velocity;
        }
        /// <summary>
        /// Calculates 2D spring-damper force components: F = -k * x - c * v.
        /// </summary>
        /// <param name="stiffness">Spring stiffness k (N/m). Must be non-negative.</param>
        /// <param name="damping">Damping coefficient c (N·s/m). Must be non-negative.</param>
        /// <param name="displacement">Displacement vector from equilibrium (m).</param>
        /// <param name="velocity">Velocity vector (m/s).</param>
        /// <returns>Spring-damper force components (Fx, Fy) in Newtons.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="stiffness"/> or <paramref name="damping"/> is negative.</exception>
        public static (double x, double y) SpringDamperForce2D(double stiffness, double damping,
            (double x, double y) displacement,
            (double x, double y) velocity)
        {
            if (stiffness < 0)
            {
                throw new ArgumentException("Stiffness must be non-negative.", nameof(stiffness));
            }
            if (damping < 0)
            {
                throw new ArgumentException("Damping must be non-negative.", nameof(damping));
            }
            return
                (
                -stiffness * displacement.x - damping * velocity.x,
                -stiffness * displacement.y - damping * velocity.y
                );
        }
        /// <summary>
        /// Calculates 3D spring-damper force components: F = -k * x - c * v.
        /// </summary>
        /// <param name="stiffness">Spring stiffness k (N/m). Must be non-negative.</param>
        /// <param name="damping">Damping coefficient c (N·s/m). Must be non-negative.</param>
        /// <param name="displacement">Displacement vector from equilibrium (m).</param>
        /// <param name="velocity">Velocity vector (m/s).</param>
        /// <returns>Spring-damper force components (Fx, Fy, Fz) in Newtons.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="stiffness"/> or <paramref name="damping"/> is negative.</exception>
        public static (double x, double y, double z) SpringDamperForce3D(double stiffness, double damping,
            (double x, double y, double z) displacement,
            (double x, double y, double z) velocity)
        {
            if (stiffness < 0)
            {
                throw new ArgumentException("Stiffness must be non-negative.", nameof(stiffness));
            }
            if (damping < 0)
            {
                throw new ArgumentException("Damping must be non-negative.", nameof(damping));
            }
            return
                (
                -stiffness * displacement.x - damping * velocity.x,
                -stiffness * displacement.y - damping * velocity.y,
                -stiffness * displacement.z - damping * velocity.z
                );
        }

        /// <summary>
        /// Calculates 1D linear drag force: Fd = -b * v.
        /// </summary>
        /// <param name="dragCoeff">Linear drag coefficient b. Must be non-negative.</param>
        /// <param name="velocity">Velocity v (m/s).</param>
        /// <returns>Drag force (N).</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="dragCoeff"/> is negative.</exception>
        public static double LinearDrag1D(double dragCoeff, double velocity)
        {
            if (dragCoeff < 0)
            {
                throw new ArgumentException("Drag coefficient must be non-negative.", nameof(dragCoeff));
            }
            return -dragCoeff * velocity;
        }
        /// <summary>
        /// Calculates 2D linear drag force components: Fd = -b * v.
        /// </summary>
        /// <param name="dragCoeff">Linear drag coefficient b. Must be non-negative.</param>
        /// <param name="velocity">Velocity vector (m/s).</param>
        /// <returns>Drag force components (Fx, Fy) in Newtons.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="dragCoeff"/> is negative.</exception>
        public static (double x, double y) LinearDrag2D(double dragCoeff, (double x, double y) velocity)
        {
            if (dragCoeff < 0)
            {
                throw new ArgumentException("Drag coefficient must be non-negative.", nameof(dragCoeff));
            }

            return (-dragCoeff * velocity.x, -dragCoeff * velocity.y);
        }
        /// <summary>
        /// Calculates 3D linear drag force components: Fd = -b * v.
        /// </summary>
        /// <param name="dragCoeff">Linear drag coefficient b. Must be non-negative.</param>
        /// <param name="velocity">Velocity vector (m/s).</param>
        /// <returns>Drag force components (Fx, Fy, Fz) in Newtons.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="dragCoeff"/> is negative.</exception>
        public static (double x, double y, double z) LinearDrag3D(double dragCoeff, (double x, double y, double z) velocity)
        {
            if (dragCoeff < 0)
            {
                throw new ArgumentException("Drag coefficient must be non-negative.", nameof(dragCoeff));
            }

            return (-dragCoeff * velocity.x, -dragCoeff * velocity.y, -dragCoeff * velocity.z);
        }
        /// <summary>
        /// Calculates 1D quadratic drag force: Fd = -k * v * |v|.
        /// </summary>
        /// <param name="k">Quadratic drag coefficient k (e.g., 0.5 * rho * Cd * A). Must be non-negative.</param>
        /// <param name="velocity">Velocity v (m/s).</param>
        /// <returns>Drag force (N).</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="k"/> is negative.</exception>
        
        public static double QuadraticDrag(double k, double velocity)
        {
            if (k < 0)
            {
                throw new ArgumentException("Drag coefficient k must be non-negative.", nameof(k));
            }

            return -k * velocity * Math.Abs(velocity);
        }
        /// <summary>
        /// Calculates 3D quadratic drag force components: Fd = -k * |v| * v.
        /// </summary>
        /// <param name="k">Quadratic drag coefficient k (e.g., 0.5 * rho * Cd * A). Must be non-negative.</param>
        /// <param name="velocity">Velocity vector (m/s).</param>
        /// <returns>Drag force components (Fx, Fy, Fz) in Newtons.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="k"/> is negative.</exception>
        public static (double x, double y, double z) QuadraticDrag3D(double k, (double x, double y, double z) velocity)
        {
            if (k < 0)
            {
                throw new ArgumentException("Drag coefficient k must be non-negative.", nameof(k));
            }

            double speed = Math.Sqrt(
                velocity.x * velocity.x +
                velocity.y * velocity.y +
                velocity.z * velocity.z);

            if (speed == 0.0)
            {
                return (0.0, 0.0, 0.0);
            }

            return
                (
                -k * speed * velocity.x,
                -k * speed * velocity.y,
                -k * speed * velocity.z
                );
        }

        /// <summary>
        /// Calculates acceleration down an incline with kinetic friction:
        /// a = g * (sin(angle) - μk * cos(angle)).
        /// </summary>
        /// <param name="angleRadians">Incline angle in radians.</param>
        /// <param name="kineticFrictionCoeff">Kinetic friction coefficient μk. Must be non-negative.</param>
        /// <returns>Acceleration along the incline (m/s^2).</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="kineticFrictionCoeff"/> is negative.</exception>
        public static double AccelerationDownIncline(double angleRadians, double kineticFrictionCoeff)
        {
            if (kineticFrictionCoeff < 0)
            {
                throw new ArgumentException("Kinetic friction coefficient must be non-negative.", nameof(kineticFrictionCoeff));
            }
            return Constants.StandardGravity * (Math.Sin(angleRadians) - kineticFrictionCoeff * Math.Cos(angleRadians));
        }
        /// <summary>
        /// Calculates acceleration along an incline from an external force parallel to the surface,
        /// opposing kinetic friction. This method does not include the gravity component along the incline.
        /// </summary>
        /// <param name="mass">Mass (kg). Must be greater than zero.</param>
        /// <param name="angleRadians">Incline angle in radians.</param>
        /// <param name="forceParallel">External force applied parallel to the incline (N).</param>
        /// <param name="kineticFrictionCoeff">Kinetic friction coefficient μk. Must be non-negative.</param>
        /// <returns>Acceleration along the incline (m/s^2).</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="mass"/> is less than or equal to zero or <paramref name="kineticFrictionCoeff"/> is negative.</exception>
        public static double AccelerationAlongIncline(double mass, double angleRadians, double forceParallel, double kineticFrictionCoeff)
        {
            if (kineticFrictionCoeff < 0)
            {
                throw new ArgumentException("Kinetic friction coefficient must be non-negative.", nameof(kineticFrictionCoeff));
            }
            if (mass <= 0)
            {
                throw new ArgumentException("Mass must be greater than zero.", nameof(mass));
            }
            return (forceParallel - kineticFrictionCoeff * mass * Constants.StandardGravity * Math.Cos(angleRadians)) / mass;
        }
        /// <summary>
        /// Calculates acceleration along an incline including gravity and kinetic friction:
        /// a = (Fparallel + m*g*sin(angle) - μk*m*g*cos(angle)) / m.
        /// </summary>
        /// <param name="mass">Mass (kg). Must be greater than zero.</param>
        /// <param name="angleRadians">Incline angle in radians.</param>
        /// <param name="forceParallel">External force applied parallel to the incline (N).</param>
        /// <param name="kineticFrictionCoeff">Kinetic friction coefficient μk. Must be non-negative.</param>
        /// <returns>Acceleration along the incline (m/s^2).</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="mass"/> is less than or equal to zero or <paramref name="kineticFrictionCoeff"/> is negative.</exception>
        public static double AccelerationAlongInclineWithGravity(double mass, double angleRadians, double forceParallel, double kineticFrictionCoeff)
        {
            if (mass <= 0)
            {
                throw new ArgumentException("Mass must be greater than zero.", nameof(mass));
            }
            if (kineticFrictionCoeff < 0)
            {
                throw new ArgumentException("Kinetic friction coefficient must be non-negative.", nameof(kineticFrictionCoeff));
            }
            double n = mass * Constants.StandardGravity * Math.Cos(angleRadians);
            double friction = kineticFrictionCoeff * n;
            double gravityParallel = mass * Constants.StandardGravity * Math.Sin(angleRadians);

            return (forceParallel + gravityParallel - friction) / mass;
        }

        /// <summary>
        /// Determines whether a body will start sliding down an incline due to gravity,
        /// using the static friction condition: tan(angle) &gt; μs.
        /// </summary>
        /// <param name="angleRadians">Incline angle in radians.</param>
        /// <param name="staticFrictionCoeff">Static friction coefficient μs. Must be non-negative.</param>
        /// <returns>True if sliding will start; otherwise false.</returns>
        /// <exception cref="ArgumentException">Thrown when <paramref name="staticFrictionCoeff"/> is negative.</exception>
        public static bool WillStartSliding(double angleRadians, double staticFrictionCoeff)
        {
            if (staticFrictionCoeff < 0)
            {
                throw new ArgumentException("Static friction coefficient must be non-negative.", nameof(staticFrictionCoeff));
            }
            return Math.Tan(angleRadians) > staticFrictionCoeff;
        }
    }
}
