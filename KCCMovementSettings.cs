using Photon.Deterministic;

namespace Quantum
{
    public unsafe class KCCMovementSettings : AssetObject
    {
        public FP CorrectionSpeed = FP._10;
        public FP MaxSpeed = FP._6;
        public FP Acceleration;
        public FP Deceleration;
        public FP AllowedPenetration = FP._0_10;
        public FP Radius = FP._0_50;
        public LayerMask CollisionLayers;
        public bool RotateWithMovement = true;
        public bool DebugGizmos = false;
    }
}