using System;
using System.Linq;
using UnityEngine;

    /// <summary>
    /// Class for managing POIs. Use singletons to find and manage POIs. Instances mplicitly conver
    /// </summary>
    public class POI
    {
        public readonly Transform Transform;
        /// <summary>
        /// Name of transform, minus all instances of POI prefix
        /// </summary>
        public string Name
        {
            get
            {
                return ParseTransformName(Transform.name);
            }
        }

        private POI(Transform transform)
        {
      
            Transform = transform;
            /*Renderer renderer = transform.renderer;
            if (renderer)
                renderer.enabled = false;*/
        }
        static public implicit operator Transform(POI poi)
        {
            return (poi.Transform);
        }
        /// <summary>
        /// Search transform (recursive) for all instances of POI. 
        /// </summary>
        /// <param name="rootTransform"> Transform to start search from. Will be returned if it is a valid POI whose name matches with name being searched</param>
        /// <param name="poiName">Case-insensitive name to search for</param>
        /// <returns>Returns first POI with EXACT name. Warning: Infinitely recursive search peformed, likely to be slow with deeper hierarchy </returns>
        public static POI GetPoiByName(Transform rootTransform, string poiName)
        {
            poiName.Replace(" ", "_");
            Transform poiTransform = FindPoiInChildren(rootTransform, poiName);
        
            if (poiTransform) return new POI(poiTransform);

            Debug.LogError("POI not found: "+poiName);
            return null;
        }
        /// <summary>
        /// Search transform (non-recursive) for all instances of POI. 
        /// </summary>
        /// <param name="rootTransform"> Warning: Only direct children will searched </param>
        /// <param name="poiName">Name to search for, all POIs containing string will be returned</param>
        /// <returns>Returns all POIs containing name.</returns>
        public static POI[] GetPoiByNameAll(Transform rootTransform, string poiName)
        {
            poiName.Replace(" ", "_");
            int numChildren = rootTransform.GetChildCount();

            return (from Transform child in rootTransform where IsValidPoiName(child.name) let parsedName = ParseTransformName(child.name) where parsedName.Contains(poiName) select new POI(child)).ToArray();
        }
        /// <summary>
        /// Internal: Old school XML-style recursive search. 
        /// </summary>
        /// <param name="parent"> Current transform being searched </param>
        /// <param name="name">Name to search for, first POI with name equal to this string will be returned</param>
        /// <returns>Returns first POI with exact name.</returns>

        private static Transform FindPoiInChildren(Transform parent, string name)
        {
            if(IsValidPoiName(parent.name))
                if (ParseTransformName(parent.name).ToLower().Equals(name)) return parent;

            return (from Transform child in parent select FindPoiInChildren(child, name)).FirstOrDefault(result => result != null);
        }
        /// <summary>
        /// Prefix to use inside 3D modeling program
        /// </summary>
        private const string PoiPrefix = "poi_";
        /// <summary>
        /// Remove all instances of PoiPrefix from string. (ex. "poi_root" becomes "root")
        /// </summary>
        /// <param name="transformName"> Name with POI Prefix included </param>
        /// <returns>Returns transformName with all instances of PoiPrefix removed.</returns>
        public static string ParseTransformName(string transformName)
        {
            return transformName.Replace(PoiPrefix, string.Empty);
        }
        /// <summary>
        /// Check if name contains POI Prefix. (ex. "poi_root" is valid, "root" is not)
        /// </summary>
        /// <param name="name"> Name with POI Prefix included </param>
        /// <returns>Returns true if name contains POI Prefix.</returns>
        private static bool IsValidPoiName(string name)
        {
            return name.ToLower().Contains(PoiPrefix);
        }
        /// <summary>
        /// Turn off Mesh Renders for transform and all its children that have POI formatted names (non-recursive). 
        /// </summary>
        /// <param name="rootTransform"> Warning: Only direct children will searched </param>
        public static void HidePOIs(Transform rootTransform)
        {
            if (rootTransform == null) throw new ArgumentNullException("rootTransform");
            foreach (var child in rootTransform.GetComponentsInChildren<Transform>().Where(child => IsValidPoiName(child.name)).Where(child => child.renderer))
            {
                child.renderer.enabled = false;
            }
        }

        /// <summary>
        /// Turn on Mesh Renders for transform and all its children that have POI formatted names (non-recursive). 
        /// </summary>
        /// <param name="rootTransform"> Warning: Only direct children will searched </param>
        public static void ShowPOIs(Transform rootTransform)
        {
            foreach (var child in rootTransform.GetComponentsInChildren<Transform>().Where(child => IsValidPoiName(child.name)).Where(child => child.renderer))
            {
                child.renderer.enabled = true;
            }
        }

        public Vector3 Position
        {
            get
            {
                return Transform.position;
            }
        }
    }

