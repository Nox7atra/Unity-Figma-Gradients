using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace Nox7atra.UIFigmaGradients
{
    public class UIFigmaGradientRadialDrawer : UIFigmaGradientLinearDrawer
    {

        [SerializeField] private Vector2 _Center;
        [Range(0.01f, 10)]
        [SerializeField] private float _Radius = 1;
        protected override Material GradientMaterial => new Material(Shader.Find("UI/RadialGradientShader"));

        protected override void GenerateHelperUvs(VertexHelper vh)
        {
            UIVertex vert = new UIVertex();
            for (int i = 0; i < vh.currentVertCount; i++) {
                vh.PopulateUIVertex(ref vert, i);
                vert.normal = _Center;
                vert.uv1 = new Vector2(_Angle, _Radius);
                vh.SetUIVertex(vert, i);
            }
        }
    }
}