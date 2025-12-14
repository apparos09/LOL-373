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
            // The text for the statement.
            public string text = string.Empty;

            // The language key for the statement.
            public string key = string.Empty;

            // The resource this statement belongs to.
            public NaturalResources.naturalResource resource = NaturalResources.naturalResource.unknown;

            // Set to 'true' if this statement has already been matched correctly.
            public bool matchedCorrectly = false;

            // Constructor
            public Statement()
            {

            }

            // Constructor
            public Statement(string text)
            {
                this.text = text;
            }

            // Constructor
            public Statement(string text, string key)
            {
                this.text = text;
                this.key = key;
            }

            // Constructor
            public Statement(string text, string key, NaturalResources.naturalResource resource, bool matchedCorrectly)
            {
                this.text = text;
                this.key = key;
                this.resource = resource;
                this.matchedCorrectly = matchedCorrectly;
            }
        }

        // A group for a given resource. It holds all the statements that this resource uses.
        public class StatementGroup
        {
            // The resource this group is for.
            public NaturalResources.naturalResource resource;

            // The statements in this group.
            public Statement[] statements = new Statement[5];

            // Constructor
            public StatementGroup()
            {
                
            }

            // Constructor
            public StatementGroup(NaturalResources.naturalResource resource)
            {
                this.resource = resource;
            }

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
        public static Statement GenerateTestStatement()
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
            // Renewable
            // Biomass
            biomassGroup = new StatementGroup(NaturalResources.naturalResource.biomass);
            biomassGroup.statements[0] = new Statement("Biomass 0");
            biomassGroup.statements[1] = new Statement("Biomass 1");
            biomassGroup.statements[2] = new Statement("Biomass 2");
            biomassGroup.statements[3] = new Statement("Biomass 3");
            biomassGroup.statements[4] = new Statement("Biomass 4");


            // Hydro
            hydroGroup = new StatementGroup(NaturalResources.naturalResource.hydro);
            hydroGroup.statements[0] = new Statement("Hydro 0");
            hydroGroup.statements[1] = new Statement("Hydro 1");
            hydroGroup.statements[2] = new Statement("Hydro 2");
            hydroGroup.statements[3] = new Statement("Hydro 3");
            hydroGroup.statements[4] = new Statement("Hydro 4");

            // Geothermal
            geothermalGroup = new StatementGroup(NaturalResources.naturalResource.geothermal);
            geothermalGroup.statements[0] = new Statement("Geothermal 0");
            geothermalGroup.statements[1] = new Statement("Geothermal 1");
            geothermalGroup.statements[2] = new Statement("Geothermal 2");
            geothermalGroup.statements[3] = new Statement("Geothermal 3");
            geothermalGroup.statements[4] = new Statement("Geothermal 4");

            // Solar
            solarGroup = new StatementGroup(NaturalResources.naturalResource.solar);
            solarGroup.statements[0] = new Statement("Solar 0");
            solarGroup.statements[1] = new Statement("Solar 1");
            solarGroup.statements[2] = new Statement("Solar 2");
            solarGroup.statements[3] = new Statement("Solar 3");
            solarGroup.statements[4] = new Statement("Solar 4");

            // Wave
            waveGroup = new StatementGroup(NaturalResources.naturalResource.wave);
            waveGroup.statements[0] = new Statement("Wave 0");
            waveGroup.statements[1] = new Statement("Wave 1");
            waveGroup.statements[2] = new Statement("Wave 2");
            waveGroup.statements[3] = new Statement("Wave 3");
            waveGroup.statements[4] = new Statement("Wave 4");

            // Wind
            windGroup = new StatementGroup(NaturalResources.naturalResource.wind);
            windGroup.statements[0] = new Statement("Wind 0");
            windGroup.statements[1] = new Statement("Wind 1");
            windGroup.statements[2] = new Statement("Wind 2");
            windGroup.statements[3] = new Statement("Wind 3");
            windGroup.statements[4] = new Statement("Wind 4");

            // Non-renewable
            // Coal
            coalGroup = new StatementGroup(NaturalResources.naturalResource.coal);
            coalGroup.statements[0] = new Statement("Coal 0");
            coalGroup.statements[1] = new Statement("Coal 1");
            coalGroup.statements[2] = new Statement("Coal 2");
            coalGroup.statements[3] = new Statement("Coal 3");
            coalGroup.statements[4] = new Statement("Coal 4");

            // Oil
            oilGroup = new StatementGroup(NaturalResources.naturalResource.oil);
            oilGroup.statements[0] = new Statement("Oil 0");
            oilGroup.statements[1] = new Statement("Oil 1");
            oilGroup.statements[2] = new Statement("Oil 2");
            oilGroup.statements[3] = new Statement("Oil 3");
            oilGroup.statements[4] = new Statement("Oil 4");

            // Natural Gas
            naturalGasGroup = new StatementGroup(NaturalResources.naturalResource.naturalGas);
            naturalGasGroup.statements[0] = new Statement("Natural Gas 0");
            naturalGasGroup.statements[1] = new Statement("Natural Gas 1");
            naturalGasGroup.statements[2] = new Statement("Natural Gas 2");
            naturalGasGroup.statements[3] = new Statement("Natural Gas 3");
            naturalGasGroup.statements[4] = new Statement("Natural Gas 4");

            // Nuclear
            nuclearGroup = new StatementGroup(NaturalResources.naturalResource.nuclear);
            nuclearGroup.statements[0] = new Statement("Nuclear 0");
            nuclearGroup.statements[1] = new Statement("Nuclear 1");
            nuclearGroup.statements[2] = new Statement("Nuclear 2");
            nuclearGroup.statements[3] = new Statement("Nuclear 3");
            nuclearGroup.statements[4] = new Statement("Nuclear 4");
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