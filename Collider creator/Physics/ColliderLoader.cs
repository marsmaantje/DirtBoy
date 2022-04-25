using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using GXPEngine;
using Newtonsoft.Json;
using System.Windows.Forms;

namespace Physics
{
    /// <summary>
    /// Loads and saves colliders to a JSON file
    /// </summary>
    public class ColliderLoader
    {
        static ColliderLoader _main;
        public static ColliderLoader main
        {
            get
            {
                if (_main == null)
                    _main = new ColliderLoader();
                return _main;
            }            
        }
        

        string colliderFilePath;
        JSONColliderList colliders;

        public ColliderLoader()
        {
            colliders = new JSONColliderList();
        }

        /// <summary>
        /// try to load the given file into the colliders list
        /// </summary>
        /// <param name="filePath">File to load</param>
        /// <returns>Success</returns>
        public bool loadFile(string filePath)
        {
            colliderFilePath = filePath;
            try
            {
                string json = System.IO.File.ReadAllText(colliderFilePath);
                colliders = JsonConvert.DeserializeObject<JSONColliderList>(json);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error loading collider file: " + e.Message);
                return false;
            }
        }

        /// <summary>
        /// Saves the currently stores colliders to a JSON file
        /// </summary>
        /// <param name="filePath">File to save the colliders to</param>
        /// <returns>success</returns>
        public bool SaveFile(string filePath)
        {
            try
            {
                string json = JsonConvert.SerializeObject(colliders);
                System.IO.File.WriteAllText(filePath, json);
                return true;
            }
            catch (Exception e)
            {
                Console.WriteLine("Error saving collider file: " + e.Message);
                return false;
            }
        }

        /// <summary>
        /// Save the current colliders to the file that was loaded
        /// </summary>
        /// <returns>Success</returns>
        public bool SaveFile()
        {
            return SaveFile(colliderFilePath);
        }

        /// <summary>
        /// Add a collider to the list
        /// </summary>
        /// <param name="name">Name of the collider</param>
        /// <param name="hasEnd">whether the collider is closed or open</param>
        /// <param name="collider">the collider to add to the list</param>
        /// <param name="overwrite">if the name already exists, overwrite with this new one</param>
        /// <returns>written yes/no</returns>
        public bool AddCollider(string name, MultiSegmentCollider collider, bool overwrite = false)
        {
            if (colliders == null)
            {
                colliders = new JSONColliderList();
            }
            if (colliders.HasCollider(name) && !overwrite)
            {
                return false;
            }
            else
            {
                colliders.colliders.Remove(colliders.GetCollider(name));
                colliders.colliders.Add(new JSONCollider(name, collider));
                return true;
            }
        }

        public bool RemoveCollider(string name)
        {
            if (colliders == null)
            {
                return false;
            }
            if (colliders.HasCollider(name))
            {
                colliders.colliders.Remove(colliders.GetCollider(name));
                return true;
            }
            else
            {
                return false;
            }
        }

        /// <summary>
        /// Creates the collider based on the given name and attaches it to the given gameobject
        /// </summary>
        /// <param name="name">name of the collider</param>
        /// <param name="owner">gameobject to attach to</param>
        /// <returns>Collider if the name exists, null otherwise</returns>
        public MultiSegmentCollider GetCollider(string name, GameObject owner)
        {
            if (colliders != null)
                return colliders.ToCollider(name, owner);
            else
                return null;
        }

        /// <summary>
        /// Get the names of all currently loaded colliders
        /// </summary>
        /// <returns></returns>
        public List<string> GetColliderNames()
        {
            return colliders.GetNames();
        }

        /// <summary>
        /// Saves the current list of colliders to the file specified by the user
        /// </summary>
        public void Save()
        {
            SaveFileDialog dialog = new SaveFileDialog();
            dialog.Filter = "Collider files(*.txt;*.json)|*.txt;*.json|All files (*.*)|*.*";
            if (dialog.ShowDialog() == DialogResult.OK)
            {
                SaveFile(dialog.FileName);
            }
        }

        /// <summary>
        /// List of colliders
        /// </summary>
        internal class JSONColliderList
        {
            public List<JSONCollider> colliders;

            public bool HasCollider(string name)
            {
                foreach (JSONCollider collider in colliders)
                {
                    if (collider.name == name)
                    {
                        return true;
                    }
                }
                return false;
            }

            /// <summary>
            /// Get the collider with the specific name
            /// </summary>
            /// <param name="name">name of the collider</param>
            /// <returns>collider with that name, null if otherwise</returns>
            public JSONCollider GetCollider(string name)
            {
                foreach(JSONCollider collider in colliders)
                {
                    if (collider.name == name)
                        return collider;
                }
                return null;
            }

            public List<string> GetNames()
            {
                List<string> returnList = new List<string>();
                foreach(JSONCollider collider in colliders)
                {
                    returnList.Add(collider.name);
                }
                return returnList;
            }

            public JSONColliderList()
            {
                colliders = new List<JSONCollider>();
            }

            public MultiSegmentCollider ToCollider(string name, GameObject owner)
            {
                foreach (JSONCollider collider in colliders)
                {
                    if (collider.name == name)
                    {
                        return collider.ToCollider(owner);
                    }
                }
                return null;
            }
        }

        /// <summary>
        /// A collider with multiple segments
        /// </summary>
        internal class JSONCollider
        {
            public string name;
            public List<Vec2> points;

            public JSONCollider(string name, MultiSegmentCollider collider)
            {
                this.name = name;
                points = new List<Vec2>();

                if (collider.segments != null && collider.segments.Count > 0)
                {
                    points.Add(collider.segments[0].start);
                    foreach (Segment segment in collider.segments)
                    {
                        points.Add(segment.end);
                    }
                }
            }

            public JSONCollider(string name, List<Vec2> points)
            {
                this.name = name;
                this.points = points;
            }

            public JSONCollider()
            {
                points = new List<Vec2>();
            }

            /// <summary>
            /// Create a collider from the JSONCollider
            /// </summary>
            /// <param name="owner">Owner of the collider</param>
            /// <returns>generated collider</returns>
            public MultiSegmentCollider ToCollider(GameObject owner)
            {
                MultiSegmentCollider collider = new MultiSegmentCollider(owner);
                for (int i = 0; i < points.Count - 1; i++)
                {
                    collider.AddSegment(points[i], points[i + 1]);
                }
                return collider;
            }
        }
    }
}
