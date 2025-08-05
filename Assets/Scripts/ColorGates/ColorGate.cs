using Misc;
using UnityEngine;

namespace ColorGates
{
    public class ColorGate : MonoBehaviour
    {
        [SerializeField] private ColorType _colorType;
        [SerializeField] private Material _colorMaterial;

        public ColorType GetColorType()
        {
            return _colorType;
        }

        public Material GetColorMaterial()
        {
            return _colorMaterial;
        }
        
    }
}