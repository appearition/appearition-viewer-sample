using UnityEngine;

namespace Appearition.BuildingsAndFloors
{
    [System.Serializable]
    public class FloorPlanStructure
    {
        [System.Serializable]
        class Rectangle
        {
            public float X;
            public float Y;
            public float Z;
        }

        public string name;
        public string type;
        public string material;
        [SerializeField] Rectangle[] rectangle = new Rectangle[2];

        public Vector3[] GetRectangle()
        {
            if (rectangle == null || rectangle.Length != 2)
                return null;
            return new Vector3[2] {new Vector3(rectangle[0].X, rectangle[0].Y, rectangle[0].Z), new Vector3(rectangle[1].X, rectangle[1].Y, rectangle[1].Z)};
        }

        public void SetRectangle(Vector3 point1, Vector3 point2)
        {
            rectangle = new Rectangle[2] {
                new Rectangle {X = point1.x, Y = point1.y, Z = point1.z},
                new Rectangle {X = point2.x, Y = point2.y, Z = point2.z},
            };
        }

        public FloorPlanStructure(string newName, string newType = "Wall")
        {
            name = newName;
            type = newType;
        }

        public FloorPlanStructure(Vector3[] rectangles)
        {
            name = AppearitionGate.GenerateNewGUID();
            type = "Wall";
            if (rectangles != null && rectangles.Length == 2)
                SetRectangle(rectangles[0], rectangles[1]);
        }

        public FloorPlanStructure(FloorPlanStructure cc)
        {
            CopyFrom(cc);
        }

        public void CopyFrom(FloorPlanStructure cc)
        {
            name = cc.name;
            type = cc.type;

            Vector3[] rectangles = cc.GetRectangle();
            if (rectangles != null)
                SetRectangle(rectangles[0], rectangles[1]);
        }
    }
}