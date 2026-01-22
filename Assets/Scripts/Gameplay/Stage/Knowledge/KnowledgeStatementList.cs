using SimpleJSON;
using System.Collections;
using System.Collections.Generic;
using System.Security.Cryptography;
using UnityEngine;

namespace RM_EDU
{
    // The list of knowledge statemnets.
    public class KnowledgeStatementList : MonoBehaviour
    {
        // A statement that is paired with a group.
        public class Statement
        {
            // Data for a statement.
            [System.Serializable]
            public class StatementData
            {
                // The ID number.
                public int idNumber;

                // The group resource.
                public NaturalResources.naturalResource groupResource;

                // The statement resource.
                public NaturalResources.naturalResource statementResource;
            }

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

            // Generates the statement data. The group resource is the same as the statement's resource.
            public StatementData GenerateStatementData()
            {
                StatementData statementData = new StatementData();
                statementData.idNumber = idNumber;
                statementData.statementResource = resource;
                statementData.groupResource = statementData.statementResource;

                return statementData;
            }

            // Generates the statement data and gives it the provided group resource.
            public StatementData GenerateStatementData(NaturalResources.naturalResource groupResource)
            {
                StatementData statementData = new StatementData();
                statementData.idNumber = idNumber;
                statementData.statementResource = resource;
                statementData.groupResource = groupResource;

                return statementData;
            }

            // Returns true if the provided data matches this statement. Cannot check group resource.
            public bool MatchesData(StatementData data)
            {
                return data.idNumber == idNumber && data.statementResource == resource;
            }
        }

        // A group for a given resource. It holds all the statements that this resource uses.
        public class StatementGroup
        {
            // Data for a statement group.
            [System.Serializable]
            public class StatementGroupData
            {
                // The resource for this group.
                public NaturalResources.naturalResource resource;

                // The statement data.
                public List<Statement.StatementData> statementDatas = new List<Statement.StatementData>();

                // Constructor
                public StatementGroupData()
                {
                    // ...
                }

                // Constructor
                public StatementGroupData(NaturalResources.naturalResource resource)
                {
                    this.resource = resource;
                }

                // Constructor
                public StatementGroupData(NaturalResources.naturalResource resource, List<Statement.StatementData> statementDatas)
                {
                    this.resource = resource;

                    // Clears the statement data and adds the passed data.
                    this.statementDatas.Clear();
                    this.statementDatas.AddRange(statementDatas);
                }
            }

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

            // Generates a copy of the group.
            public StatementGroup GenerateGroupCopy()
            {
                StatementGroup groupCopy = new StatementGroup(resource);
                groupCopy.statements = new List<Statement>(statements);
                groupCopy.SetAllStatementsToGroupResource();
                return groupCopy;
            }

            // Generates the statement group data.
            public StatementGroupData GenerateStatementGroupData()
            {
                // The statement group data and the provided resource.
                StatementGroupData statementGroupData = new StatementGroupData();
                statementGroupData.resource = resource;

                // Generates the data from each statement.
                foreach(Statement statement in statements)
                {
                    statementGroupData.statementDatas.Add(statement.GenerateStatementData(statementGroupData.resource));
                }

                // Returns the group data.
                return statementGroupData;
            }
        }

        // The singleton instance.
        private static KnowledgeStatementList instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        // Natural Resource Groups
        // Renewable
        private StatementGroup biomassGroup;
        private StatementGroup hydroGroup;
        private StatementGroup geothermalGroup;
        private StatementGroup solarGroup;
        private StatementGroup waveGroup;
        private StatementGroup windGroup;

        // Non-renewable
        private StatementGroup coalGroup;
        private StatementGroup oilGroup;
        private StatementGroup naturalGasGroup;
        private StatementGroup nuclearGroup;

        // Gets set to 'true' if the groups have all been generated.
        private bool groupsGenerated = false;

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
            biomassGroup.statements.Add(new Statement(0, "Biomass 0", "kst_bms_00"));
            biomassGroup.statements.Add(new Statement(1, "Biomass 1", "kst_bms_01"));
            biomassGroup.statements.Add(new Statement(2, "Biomass 2", "kst_bms_02"));
            biomassGroup.statements.Add(new Statement(3, "Biomass 3", "kst_bms_03"));
            biomassGroup.statements.Add(new Statement(4, "Biomass 4", "kst_bms_04"));
            biomassGroup.SetAllStatementsToGroupResource();

            // Geothermal
            geothermalGroup = new StatementGroup(NaturalResources.naturalResource.geothermal);
            geothermalGroup.statements.Add(new Statement(0, "Geothermal 0", "kst_gtl_00"));
            geothermalGroup.statements.Add(new Statement(1, "Geothermal 1", "kst_gtl_01"));
            geothermalGroup.statements.Add(new Statement(2, "Geothermal 2", "kst_gtl_02"));
            geothermalGroup.statements.Add(new Statement(3, "Geothermal 3", "kst_gtl_03"));
            geothermalGroup.statements.Add(new Statement(4, "Geothermal 4", "kst_gtl_04"));
            geothermalGroup.SetAllStatementsToGroupResource();

            // Hydro
            hydroGroup = new StatementGroup(NaturalResources.naturalResource.hydro);
            hydroGroup.statements.Add(new Statement(0, "Hydro 0", "kst_hdo_00"));
            hydroGroup.statements.Add(new Statement(1, "Hydro 1", "kst_hdo_01"));
            hydroGroup.statements.Add(new Statement(2, "Hydro 2", "kst_hdo_02"));
            hydroGroup.statements.Add(new Statement(3, "Hydro 3", "kst_hdo_03"));
            hydroGroup.statements.Add(new Statement(4, "Hydro 4", "kst_hdo_04"));
            hydroGroup.SetAllStatementsToGroupResource();

            // Solar
            solarGroup = new StatementGroup(NaturalResources.naturalResource.solar);
            solarGroup.statements.Add(new Statement(0, "Solar 0", "kst_slr_00"));
            solarGroup.statements.Add(new Statement(1, "Solar 1", "kst_slr_01"));
            solarGroup.statements.Add(new Statement(2, "Solar 2", "kst_slr_02"));
            solarGroup.statements.Add(new Statement(3, "Solar 3", "kst_slr_03"));
            solarGroup.statements.Add(new Statement(4, "Solar 4", "kst_slr_04"));
            solarGroup.SetAllStatementsToGroupResource();

            // Wave
            waveGroup = new StatementGroup(NaturalResources.naturalResource.wave);
            waveGroup.statements.Add(new Statement(0, "Wave 0", "kst_wve_00"));
            waveGroup.statements.Add(new Statement(1, "Wave 1", "kst_wve_01"));
            waveGroup.statements.Add(new Statement(2, "Wave 2", "kst_wve_02"));
            waveGroup.statements.Add(new Statement(3, "Wave 3", "kst_wve_03"));
            waveGroup.statements.Add(new Statement(4, "Wave 4", "kst_wve_04"));
            waveGroup.SetAllStatementsToGroupResource();

            // Wind
            windGroup = new StatementGroup(NaturalResources.naturalResource.wind);
            windGroup.statements.Add(new Statement(0, "Wind 0", "kst_wnd_00"));
            windGroup.statements.Add(new Statement(1, "Wind 1", "kst_wnd_01"));
            windGroup.statements.Add(new Statement(2, "Wind 2", "kst_wnd_02"));
            windGroup.statements.Add(new Statement(3, "Wind 3", "kst_wnd_03"));
            windGroup.statements.Add(new Statement(4, "Wind 4", "kst_wnd_04"));
            windGroup.SetAllStatementsToGroupResource();

            // Non-renewable
            // Coal
            coalGroup = new StatementGroup(NaturalResources.naturalResource.coal);
            coalGroup.statements.Add(new Statement(0, "Coal 0", "kst_col_00"));
            coalGroup.statements.Add(new Statement(1, "Coal 1", "kst_col_01"));
            coalGroup.statements.Add(new Statement(2, "Coal 2", "kst_col_02"));
            coalGroup.statements.Add(new Statement(3, "Coal 3", "kst_col_03"));
            coalGroup.statements.Add(new Statement(4, "Coal 4", "kst_col_04"));
            coalGroup.SetAllStatementsToGroupResource();

            // Natural Gas
            naturalGasGroup = new StatementGroup(NaturalResources.naturalResource.naturalGas);
            naturalGasGroup.statements.Add(new Statement(0, "Natural Gas 0", "kst_ngs_00"));
            naturalGasGroup.statements.Add(new Statement(1, "Natural Gas 1", "kst_ngs_01"));
            naturalGasGroup.statements.Add(new Statement(2, "Natural Gas 2", "kst_ngs_02"));
            naturalGasGroup.statements.Add(new Statement(3, "Natural Gas 3", "kst_ngs_03"));
            naturalGasGroup.statements.Add(new Statement(4, "Natural Gas 4", "kst_ngs_04"));
            naturalGasGroup.SetAllStatementsToGroupResource();

            // Nuclear
            nuclearGroup = new StatementGroup(NaturalResources.naturalResource.nuclear);
            nuclearGroup.statements.Add(new Statement(0, "Nuclear 0", "kst_nlr_00"));
            nuclearGroup.statements.Add(new Statement(1, "Nuclear 1", "kst_nlr_01"));
            nuclearGroup.statements.Add(new Statement(2, "Nuclear 2", "kst_nlr_02"));
            nuclearGroup.statements.Add(new Statement(3, "Nuclear 3", "kst_nlr_03"));
            nuclearGroup.statements.Add(new Statement(4, "Nuclear 4", "kst_nlr_04"));
            nuclearGroup.SetAllStatementsToGroupResource();

            // Oil
            oilGroup = new StatementGroup(NaturalResources.naturalResource.oil);
            oilGroup.statements.Add(new Statement(0, "Oil 0", "kst_oil_00"));
            oilGroup.statements.Add(new Statement(1, "Oil 1", "kst_oil_01"));
            oilGroup.statements.Add(new Statement(2, "Oil 2", "kst_oil_02"));
            oilGroup.statements.Add(new Statement(3, "Oil 3", "kst_oil_03"));
            oilGroup.statements.Add(new Statement(4, "Oil 4", "kst_oil_04"));
            oilGroup.SetAllStatementsToGroupResource();

            // All the groups have been generated.
            groupsGenerated = true;
        }
        
        // Returns 'true' if the groups have been generated.
        public bool AreGroupsGenerated()
        {
            return groupsGenerated;
        }

        // Gets the requested group as a copy.
        protected StatementGroup GetGroup(NaturalResources.naturalResource res)
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

                case NaturalResources.naturalResource.geothermal:
                    group = geothermalGroup;
                    break;

                case NaturalResources.naturalResource.hydro:
                    group = hydroGroup;
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

        // Gets a group copy so that the original group won't be edited.
        public StatementGroup GetGroupCopy(NaturalResources.naturalResource res)
        {
            // The original group and the group copy.
            StatementGroup group = GetGroup(res);
            StatementGroup groupCopy = null;

            // If the group exists, create a group copy.
            if(group != null)
            {
                groupCopy = group.GenerateGroupCopy();
            }

            return groupCopy;
        }

        // Gets the groups as a list.
        protected List<StatementGroup> GetAllGroups()
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

        // Gets the groups copy as a list.
        protected List<StatementGroup> GetAllGroupsCopy()
        {
            // The statement group copy.
            // Put in the same order as their enum in 'NaturalResources' class.
            List<StatementGroup> groups = new List<StatementGroup>()
            {
                biomassGroup.GenerateGroupCopy(),
                hydroGroup.GenerateGroupCopy(),
                geothermalGroup.GenerateGroupCopy(),
                solarGroup.GenerateGroupCopy(),
                waveGroup.GenerateGroupCopy(),
                windGroup.GenerateGroupCopy(),
                coalGroup.GenerateGroupCopy(),
                oilGroup.GenerateGroupCopy(),
                naturalGasGroup.GenerateGroupCopy(),
                nuclearGroup.GenerateGroupCopy()
            };

            return groups;
        }

        // Gets the group data.
        public StatementGroup.StatementGroupData GetGroupData(NaturalResources.naturalResource res)
        {
            // The group data and the group.
            StatementGroup.StatementGroupData groupData;
            StatementGroup group = GetGroup(res);

            // If the group is set, get its data.
            if(group != null)
            {
                groupData = group.GenerateStatementGroupData();
            }
            // No group, so return null.
            else
            {
                groupData = null;
            }

            return groupData;
        }

        // Gets all the group datas.
        public List<StatementGroup.StatementGroupData> GetAllGroupDatas()
        {
            // The statement group datas.
            List<StatementGroup.StatementGroupData> groupDatas = new List<StatementGroup.StatementGroupData>()
            {
                biomassGroup.GenerateStatementGroupData(),
                hydroGroup.GenerateStatementGroupData(),
                geothermalGroup.GenerateStatementGroupData(),
                solarGroup.GenerateStatementGroupData(),
                waveGroup.GenerateStatementGroupData(),
                windGroup.GenerateStatementGroupData(),
                coalGroup.GenerateStatementGroupData(),
                oilGroup.GenerateStatementGroupData(),
                naturalGasGroup.GenerateStatementGroupData(),
                nuclearGroup.GenerateStatementGroupData()
            };

            return groupDatas;
        }
    }
}