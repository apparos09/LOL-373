using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // A resource in the knowledge stage. A knowledge statement is matched to it.
    public class KnowledgeResource : MonoBehaviour
    {
        // The natural resource for this knowledge resource.
        public NaturalResources.naturalResource resource = NaturalResources.naturalResource.unknown;

        // The statement that is attached to this resource.
        public KnowledgeResource attachedStatement;

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