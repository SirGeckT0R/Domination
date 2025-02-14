using UnityEngine;

namespace Helpers
{
    public static class LayerMaskHelper
    {
        public static bool IsInLayerMask(GameObject obj, LayerMask maskToCompare) => (maskToCompare.value & (1 << obj.layer)) != 0;
        public static bool IsInLayerMask(int layer, LayerMask maskToCompare) => (maskToCompare.value & (1 << layer)) != 0;
    }
}