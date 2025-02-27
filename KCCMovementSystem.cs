using Photon.Deterministic;
using Quantum.Physics2D;

namespace Quantum
{
    public unsafe static class KCCMovementSystem
    {
        public static void Move(Frame f, EntityRef entity, FPVector2 inputDirection)
        {
            // 1. Get components
            if (!f.Unsafe.TryGetPointer(entity, out KCCMovement* movement)) return;
            if (!f.Unsafe.TryGetPointer(entity, out Transform2D* transform)) return;

            // 2. Load configuration from asset
            var settings = f.FindAsset<KCCMovementSettings>(movement->Settings.Id);

            // 3. Process movement
            ProcessMovement(f, entity, movement, transform, settings, inputDirection);
        }

        private static void ProcessMovement(Frame f, EntityRef currentEntity, KCCMovement* movement, Transform2D* transform,
            KCCMovementSettings settings, FPVector2 inputDirection)
        {
            // Sanitized direction
            FPVector2 direction = inputDirection.Magnitude > FP._1 ?
                inputDirection.Normalized :
                inputDirection;

            // Calculate acceleration
            FP acceleration = direction != FPVector2.Zero ?
                settings.Acceleration :
                settings.Deceleration;

            // Apply acceleration
            movement->Velocity = FPVector2.MoveTowards(
                  movement->Velocity,                // current
                  direction * settings.MaxSpeed,     // target
                  acceleration * f.DeltaTime         // maxDelta
             );

            // Rotation if enabled
            if (settings.RotateWithMovement && direction != FPVector2.Zero)
            {
                transform->Rotation = FPVector2.RadiansSkipNormalize(FPVector2.Up, direction);
            }

            // Process collisions
            var collisionData = ProcessCollisions(f, currentEntity, settings, transform);
            ApplyCollisionCorrection(f, transform, settings, collisionData);

            // Apply final movement
            transform->Position += movement->Velocity * f.DeltaTime;

            DrawDebugGizmos(settings, transform);
        }

        private static CollisionData ProcessCollisions(Frame f, EntityRef currentEntity, KCCMovementSettings settings, Transform2D* transform)
        {
            var data = new CollisionData();
            var shape = Shape2D.CreateCircle(settings.Radius);

            var hits = f.Physics2D.OverlapShape(
                position: transform->Position,
                rotation: FP._0,
                shape: shape,
                layerMask: settings.CollisionLayers,
                options: QueryOptions.HitStatics | QueryOptions.ComputeDetailedInfo
            );

            if (hits.Count > 0)
            {
                ProcessCollisionDetails(hits, currentEntity, transform, settings, ref data);
            }

            return data;
        }

        private static void ProcessCollisionDetails(HitCollection hits, EntityRef currentEntity, Transform2D* transform,
           KCCMovementSettings settings, ref CollisionData data)
        {
            for (int i = 0; i < hits.Count; i++)
            {
                var hit = hits[i];
                if (hit.IsTrigger || hit.Entity == currentEntity) continue;

                FPVector2 penetrationVector = transform->Position - hit.Point;
                FP penetrationDepth = penetrationVector.Magnitude - settings.Radius;

                if (penetrationDepth > FP._0)
                {
                    FPVector2 normal = penetrationVector.Normalized;
                    data.TotalCorrection += normal * -penetrationDepth;
                    data.MaxPenetration = FPMath.Max(data.MaxPenetration, penetrationDepth);
                }
            }
        }

        private static void ApplyCollisionCorrection(Frame f, Transform2D* transform,
            KCCMovementSettings settings, CollisionData collisionData)
        {
            if (collisionData.MaxPenetration > settings.AllowedPenetration)
            {
                FP correctionFactor = collisionData.MaxPenetration > settings.AllowedPenetration * 2 ?
                    FP._1 :
                    settings.CorrectionSpeed * f.DeltaTime;

                transform->Position += collisionData.TotalCorrection * correctionFactor;
            }
        }

        private static void DrawDebugGizmos(KCCMovementSettings settings, Transform2D* transform)
        {
#if DEBUG
            if (settings.DebugGizmos)
            {
                Draw.Circle(transform->Position, settings.Radius, ColorRGBA.Blue);
                Draw.Ray(transform->Position, transform->Forward * settings.Radius, ColorRGBA.Red);
            }
#endif
        }

        private struct CollisionData
        {
            public FPVector2 TotalCorrection;
            public FP MaxPenetration;
        }
    }
}