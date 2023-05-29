// -----------------------------------------------------------------------
// Company:"Appearition Pty Ltd"
// File: "LearnNode.cs" 
// Copyright (c) 2019. All rights reserved.
// -----------------------------------------------------------------------

#pragma warning disable 0649

namespace Appearition.Learn
{
    [System.Serializable]
    public class LearnNode
    {
        //Variables
        public int nodeId;
        public int productId;
        public string name;
        [UnityEngine.SerializeField] int parentNodeId;
        public int? ParentNodeId => parentNodeId == 0 ? default(int?) : parentNodeId;
        public LearnNode[] childNodes;
        public NodeProficiencyLevel[] nodeProficiencyLevels;
        public AssociatedNode[] associatedNodes;
        public AntagonisingNode[] antagonisingNodes;
        public RelatedNode[] relatedNodes;
        public NodeCategory[] nodeCategories;
        public NodeProperty[] nodeProperties;
    }
}