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
            // The id number of the statement.
            public int idNumber = 0;

            // The text for the statement.
            public string text = string.Empty;

            // The language key for the statement.
            public string key = string.Empty;

            // The resource this statement belongs to.
            public NaturalResources.naturalResource resource = NaturalResources.naturalResource.unknown;

            // Constructor
            public Statement()
            {

            }

            // Constructor
            public Statement(int idNumber, string text)
            {
                this.idNumber = idNumber;
                this.text = text;
            }

            // Constructor
            public Statement(int idNumber, string text, string key)
            {
                this.idNumber = idNumber;
                this.text = text;
                this.key = key;
            }

            // Constructor
            public Statement(int idNumber, string text, string key, NaturalResources.naturalResource resource)
            {
                this.idNumber = idNumber;
                this.text = text;
                this.key = key;
                this.resource = resource;
            }
        }

        // A group for a given resource. It holds all the statements that this resource uses.
        public class StatementGroup
        {
            // The resource this group is for.
            public NaturalResources.naturalResource resource;

            // The statements in this group.
            public List<Statement> statements = new List<Statement>();

            // Constructor
            public StatementGroup()
            {
                
            }

            // Constructor
            public StatementGroup(NaturalResources.naturalResource resource)
            {
                this.resource = resource;
            }

            // Gets the statements as a list.
            public List<Statement> GetStatementList()
            {
                List<Statement> statementList = new List<Statement>(statements);
                return statementList;
            }

            // Sets all statements to the group resource.
            public void SetAllStatementsToGroupResource()
            {
                // Sets the statement resources to the group resource.
                foreach(Statement statement in statements)
                {
                    statement.resource = resource;
                }
            }
        }

        // The singleton instance.
        private static KnowledgeStatementList instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // Natural Resource Groups
        // Renewable
        protected StatementGroup biomassGroup;
        protected StatementGroup hydroGroup;
        protected StatementGroup geothermalGroup;
        protected StatementGroup solarGroup;
        protected StatementGroup waveGroup;
        protected StatementGroup windGroup;

        // Non-renewable
        protected StatementGroup coalGroup;
        protected StatementGroup oilGroup;
        protected StatementGroup naturalGasGroup;
        protected StatementGroup nuclearGroup;

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
            statement.idNumber = 0;
            statement.text = "This is a test statement.";
            statement.key = "";
            statement.resource = NaturalResources.naturalResource.unknown;

            return statement;
        }

        // Generates the groups.
        public void GenerateGroups()
        {
            // TODO: add language keys.

            // Renewable
            // Biomass
            biomassGroup = new StatementGroup(NaturalResources.naturalResource.biomass);
            biomassGroup.statements.Add(new Statement(0, "Biomass 0"));
            biomassGroup.statements.Add(new Statement(1, "Biomass 1"));
            biomassGroup.statements.Add(new Statement(2, "Biomass 2"));
            biomassGroup.statements.Add(new Statement(3, "Biomass 3"));
            biomassGroup.statements.Add(new Statement(4, "Biomass 4"));
            biomassGroup.SetAllStatementsToGroupResource();


            // Hydro
            hydroGroup = new StatementGroup(NaturalResources.naturalResource.hydro);
            hydroGroup.statements.Add(new Statement(0, "Hydro 0"));
            hydroGroup.statements.Add(new Statement(1, "Hydro 1"));
            hydroGroup.statements.Add(new Statement(2, "Hydro 2"));
            hydroGroup.statements.Add(new Statement(3, "Hydro 3"));
            hydroGroup.statements.Add(new Statement(4, "Hydro 4"));
            hydroGroup.SetAllStatementsToGroupResource();

            // Geothermal
            geothermalGroup = new StatementGroup(NaturalResources.naturalResource.geothermal);
            geothermalGroup.statements.Add(new Statement(0, "Geothermal 0"));
            geothermalGroup.statements.Add(new Statement(1, "Geothermal 1"));
            geothermalGroup.statements.Add(new Statement(2, "Geothermal 2"));
            geothermalGroup.statements.Add(new Statement(3, "Geothermal 3"));
            geothermalGroup.statements.Add(new Statement(4, "Geothermal 4"));
            geothermalGroup.SetAllStatementsToGroupResource();

            // Solar
            solarGroup = new StatementGroup(NaturalResources.naturalResource.solar);
            solarGroup.statements.Add(new Statement(0, "Solar 0"));
            solarGroup.statements.Add(new Statement(1, "Solar 1"));
            solarGroup.statements.Add(new Statement(2, "Solar 2"));
            solarGroup.statements.Add(new Statement(3, "Solar 3"));
            solarGroup.statements.Add(new Statement(4, "Solar 4"));
            solarGroup.SetAllStatementsToGroupResource();

            // Wave
            waveGroup = new StatementGroup(NaturalResources.naturalResource.wave);
            waveGroup.statements.Add(new Statement(0, "Wave 0"));
            waveGroup.statements.Add(new Statement(1, "Wave 1"));
            waveGroup.statements.Add(new Statement(2, "Wave 2"));
            waveGroup.statements.Add(new Statement(3, "Wave 3"));
            waveGroup.statements.Add(new Statement(4, "Wave 4"));
            waveGroup.SetAllStatementsToGroupResource();

            // Wind
            windGroup = new StatementGroup(NaturalResources.naturalResource.wind);
            windGroup.statements.Add(new Statement(0, "Wind 0"));
            windGroup.statements.Add(new Statement(1, "Wind 1"));
            windGroup.statements.Add(new Statement(2, "Wind 2"));
            windGroup.statements.Add(new Statement(3, "Wind 3"));
            windGroup.statements.Add(new Statement(4, "Wind 4"));
            windGroup.SetAllStatementsToGroupResource();

            // Non-renewable
            // Coal
            coalGroup = new StatementGroup(NaturalResources.naturalResource.coal);
            coalGroup.statements.Add(new Statement(0, "Coal 0"));
            coalGroup.statements.Add(new Statement(1, "Coal 1"));
            coalGroup.statements.Add(new Statement(2, "Coal 2"));
            coalGroup.statements.Add(new Statement(3, "Coal 3"));
            coalGroup.statements.Add(new Statement(4, "Coal 4"));
            coalGroup.SetAllStatementsToGroupResource();

            // Oil
            oilGroup = new StatementGroup(NaturalResources.naturalResource.oil);
            oilGroup.statements.Add(new Statement(0, "Oil 0"));
            oilGroup.statements.Add(new Statement(1, "Oil 1"));
            oilGroup.statements.Add(new Statement(2, "Oil 2"));
            oilGroup.statements.Add(new Statement(3, "Oil 3"));
            oilGroup.statements.Add(new Statement(4, "Oil 4"));
            oilGroup.SetAllStatementsToGroupResource();

            // Natural Gas
            naturalGasGroup = new StatementGroup(NaturalResources.naturalResource.naturalGas);
            naturalGasGroup.statements.Add(new Statement(0, "Natural Gas 0"));
            naturalGasGroup.statements.Add(new Statement(1, "Natural Gas 1"));
            naturalGasGroup.statements.Add(new Statement(2, "Natural Gas 2"));
            naturalGasGroup.statements.Add(new Statement(3, "Natural Gas 3"));
            naturalGasGroup.statements.Add(new Statement(4, "Natural Gas 4"));
            naturalGasGroup.SetAllStatementsToGroupResource();

            // Nuclear
            nuclearGroup = new StatementGroup(NaturalResources.naturalResource.nuclear);
            nuclearGroup.statements.Add(new Statement(0, "Nuclear 0"));
            nuclearGroup.statements.Add(new Statement(1, "Nuclear 1"));
            nuclearGroup.statements.Add(new Statement(2, "Nuclear 2"));
            nuclearGroup.statements.Add(new Statement(3, "Nuclear 3"));
            nuclearGroup.statements.Add(new Statement(4, "Nuclear 4"));
            nuclearGroup.SetAllStatementsToGroupResource();
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

        // Gets the requested group.
        public StatementGroup GetGroup(NaturalResources.naturalResource res)
        {
            // The group to be returned.
            StatementGroup group;

            // Checks what group to return.
            switch(res)
            {
                default:
                    group = null;
                    break;

                case NaturalResources.naturalResource.biomass:
                    group = biomassGroup;
                    break;

                case NaturalResources.naturalResource.hydro:
                    group = hydroGroup;
                    break;

                case NaturalResources.naturalResource.geothermal:
                    group = geothermalGroup;
                    break;

                case NaturalResources.naturalResource.solar:
                    group = solarGroup;
                    break;

                case NaturalResources.naturalResource.wave:
                    group = waveGroup;
                    break;

                case NaturalResources.naturalResource.wind:
                    group = windGroup;
                    break;

                case NaturalResources.naturalResource.coal:
                    group = coalGroup;
                    break;

                case NaturalResources.naturalResource.oil:
                    group = oilGroup;
                    break;

                case NaturalResources.naturalResource.naturalGas:
                    group = naturalGasGroup;
                    break;

                case NaturalResources.naturalResource.nuclear:
                    group = nuclearGroup;
                    break;
            }

            return group;
        }
    }
}