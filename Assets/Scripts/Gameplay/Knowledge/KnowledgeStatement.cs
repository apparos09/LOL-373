using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // A statement for the knowledge stage. It gets matched to a knowledge resource.
    public class KnowledgeStatement : MonoBehaviour
    {
        // The statement that this KnowledgeStatment is using from the list.
        public KnowledgeStatementList.Statement statement;

        // The resource attached to this statement.
        public KnowledgeResource attachedResource;

        // Start is called before the first frame update
        void Start()
        {

        }


        // Update is called once per frame
        void Update()
        {

        }
    }
}