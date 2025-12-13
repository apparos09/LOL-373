using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RM_EDU
{
    // The list of knowledge statemnets.
    public class KnowledgeStatementList : MonoBehaviour
    {
        // A statement that is paired with a group.
        public class Statement
        {
            // The language key for the statement.
            public string key = string.Empty;

            // The text for the statement.
            public string text = string.Empty;

            // The resource this statement belongs to.
            public NaturalResources.naturalResource resource = NaturalResources.naturalResource.unknown;

            // Set to 'true' if this statement has already been matched correctly.
            public bool matchedCorrectly = false;
        }

        // A group for a given resource. It holds all the statements that this resource uses.
        public class StatementGroup
        {
            // The resource this group is for.
            public NaturalResources.naturalResource resource;

            // The statements in this group.
            public Statement[] statements = new Statement[5];

            // Gets the statements that have matches.
            public List<Statement> GetMatchedStatements()
            {
                // Resulting list.
                List<Statement> result = new List<Statement>();

                // Goes through all the statements and gets the matched statements.
                foreach (Statement statement in statements)
                {
                    // Checks if statement exists, then checks if it's been matched correctly.
                    if (statement != null)
                    {
                        if (statement.matchedCorrectly)
                        {
                            result.Add(statement);
                        }
                    }
                }

                return result;
            }

            // Gets the statements that don't have matches yet.
            public List<Statement> GetUnmatchedStatements()
            {
                // Resulting list.
                List<Statement> result = new List<Statement>();

                // Goes through all the statements and gets the unmatched statements.
                foreach(Statement statement in statements)
                {
                    // Checks if statement exists, then checks if it hasn't been matched correctly.
                    if (statement != null)
                    {
                        if(!statement.matchedCorrectly)
                        {
                            result.Add(statement);
                        }
                    }
                }

                return result;
            }
        }

        // The singleton instance.
        private static KnowledgeStatementList instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // Natural Resource Groups
        // Renewable
        public StatementGroup biomassGroup;
        public StatementGroup hydroGroup;
        public StatementGroup geothermalGroup;
        public StatementGroup solarGroup;
        public StatementGroup waveGroup;
        public StatementGroup windGroup;

        // Non-renewable
        public StatementGroup coalGroup;
        public StatementGroup oilGroup;
        public StatementGroup naturalGasGroup;
        public StatementGroup nuclearGroup;

        // Constructor
        private KnowledgeStatementList()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected virtual void Awake()
        {
            // If the instance hasn't been set, set it to this object.
            if (instance == null)
            {
                instance = this;

                // Generates the groups.
                GenerateGroups();
            }
            // If the instance isn't this, destroy the game object.
            else if (instance != this)
            {
                Destroy(gameObject);
                return;
            }

            // Run code for initialization.
            if (!instanced)
            {
                instanced = true;
            }
        }

        // Start is called before the first frame update
        void Start()
        {
            // ...
        }

        // Gets the instance.
        public static KnowledgeStatementList Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<KnowledgeStatementList>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("KnowledgeStatementList (singleton)");
                        instance = go.AddComponent<KnowledgeStatementList>();
                    }

                }

                // Return the instance.
                return instance;
            }
        }

        // Returns 'true' if the object has been instanced.
        public static bool Instantiated
        {
            get
            {
                return instanced;
            }
        }

        // Generates a test statement.
        public Statement GenerateTestStatement()
        {
            Statement statement = new Statement();

            // Default values.
            statement.key = "";
            statement.text = "This is a test statement.";
            statement.resource = NaturalResources.naturalResource.unknown;
            statement.matchedCorrectly = false;

            return statement;
        }

        // Generates the groups.
        public void GenerateGroups()
        {
            // ... TODO: generate the groups.
        }    

        // Applies the data for all the groups to set which ones have been marked correctly.
        public void ApplyGroupDatas()
        {
            // TODO: implement.
        }

        // Gets the groups as a list.
        public List<StatementGroup> GetGroupsAsList()
        {
            // The statement groups.
            // Put in the same order as their enum in 'NaturalResources' class.
            List<StatementGroup> groups = new List<StatementGroup>()
            {
                biomassGroup,
                hydroGroup,
                geothermalGroup,
                solarGroup,
                waveGroup,
                windGroup,
                coalGroup,
                oilGroup,
                naturalGasGroup,
                nuclearGroup
            };

            return groups;
        }
    }
}