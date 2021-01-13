using UnityEngine;
using UnityEngine.UI;

namespace Nox7atra.UIFigmaGradients
{
   public class UIFigmaGradientLinearDrawer : MaskableGraphic
   { 
      [SerializeField]
      private Gradient _Gradient = new Gradient();
      [SerializeField]
      private GradientResolution _GradientResolution = GradientResolution.k256;
      [Range(0, 360)]
      [SerializeField] 
      protected float _Angle = 180;

      private Texture2D _GradientTexture;
      protected virtual Material GradientMaterial => new Material(Shader.Find("UI/LinearGradientShader"));
      public override Texture mainTexture => _GradientTexture;
   #if UNITY_EDITOR
      protected override void OnValidate()
      {
         base.OnValidate();
         Refresh();
      }
   #endif
      protected override void Awake()
      {
         base.Awake();
         Refresh();
      }

      public Texture2D GenerateTexture(bool makeNoLongerReadable = false)
      {
         Texture2D tex = new Texture2D(1, (int)_GradientResolution, TextureFormat.ARGB32, false, true);
         tex.wrapMode = TextureWrapMode.Clamp;
         tex.filterMode = FilterMode.Bilinear;
         tex.anisoLevel = 1;
         Color[] colors = new Color[(int)_GradientResolution];
         float div = (float)(int)_GradientResolution;
         for (int i = 0; i < (int)_GradientResolution; ++i)
         {
            float t = (float)i/div;
            colors[i] = _Gradient.Evaluate(t);
         }
         tex.SetPixels(colors);
         tex.Apply(false, makeNoLongerReadable);
         
         return tex;
      }
      
      public void Refresh()
      {
         if (_GradientTexture != null)
         {
            DestroyImmediate(_GradientTexture);
         }

         material = GradientMaterial;
         _GradientTexture = GenerateTexture();
      }
      void OnDestroy()
      {
         if (_GradientTexture != null)
         {
            DestroyImmediate(_GradientTexture);
         }
      }

      protected virtual void GenerateHelperUvs(VertexHelper vh)
      {
         UIVertex vert = new UIVertex();
         for (int i = 0; i < vh.currentVertCount; i++) {
            vh.PopulateUIVertex(ref vert, i);
            vert.uv1 = new Vector2(_Angle, _Angle);          
            vh.SetUIVertex(vert, i);
         }
      }
      protected override void OnPopulateMesh(VertexHelper vh)
      {
         base.OnPopulateMesh(vh);
         GenerateHelperUvs(vh);
      }
   }
}