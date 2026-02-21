using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using util;

namespace RM_EDU
{
    // The knowledge manager.
    public class KnowledgeManager : StageManager
    {
        // The singleton instance.
        private static KnowledgeManager instance;

        // Gets set to 'true' when the singleton has been instanced.
        // This isn't needed, but it helps with the clarity.
        private static bool instanced = false;

        [Header("Knowledge")]

        // The knowledge UI.
        public KnowledgeUI knowledgeUI = null;

        // The knowledge audio.
        public KnowledgeAudio knowledgeAudio = null;

        // The section text.
        private string sectionString = "Section";

        // The section key.
        private string sectionStringKey = "kwd_section";

        // The statements text.
        private string statementsString = "Statements";

        // The statements text key.
        private string statementsStringKey = "kwd_statements";

        // The resources text.
        private string resourcesStringString = "Resources";

        // The resources text key.
        private string resourcesStringKey = "kwd_resources";

        // The selected knowledge statement.
        public KnowledgeStatement selectedStatement = null;

        // The selected knowledge resource.
        public KnowledgeResource selectedResource = null;

        // The knowledge statement list.
        public KnowledgeStatementList knowledgeStatementList;

        // The statement groups that the knowledge stages pulls from.
        public List<KnowledgeStatementList.StatementGroup> statementGroups = new List<KnowledgeStatementList.StatementGroup>();

        // If 'true', random natural resources from the list are selected.
        [Tooltip("Selects random natural resources from the list when initializing buttons instead of going in list order if true.")]
        public bool randomResourcesOrder = true;

        // If 'true', the statements are randomized if a match failed.
        [Tooltip("Randomizes a statement if a match failed.")]
        public bool randomizeStatementsOnFail = true;

        // The number of verification attempts, which determines the energy bonus.
        [Tooltip("The number of verification attempts.")]
        public int verifyAttempts = 0;

        // If 'true', saved matched statements are allowed to be reused.
        // If 'false', they won't be used. If a group has no statements left after removing...
        // All the saved matched statements they're reused anyway.
        private bool useSavedMatchedStatements = false;

        // Gets set to 'true' when the knowledge manager has been intialized.
        protected bool initializedKnowledge = false;

        // Constructor
        private KnowledgeManager()
        {
            // ...
        }

        // Awake is called when the script is being loaded
        protected override void Awake()
        {
            // If the instance hasn't been set, set it to this object.
            if (instance == null)
            {
                instance = this;
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

            base.Awake();
        }

        // Start is called before the first frame update
        protected override void Start()
        {
            base.Start();

            // Sets the knowledge UI if it isn't already set.
            if(knowledgeUI == null)
            {
                knowledgeUI = KnowledgeUI.Instance;
            }
                
            // If the knowledge audio isn't set, grab the instance.
            if(knowledgeAudio == null)
            {
                knowledgeAudio = KnowledgeAudio.Instance;
            }

            // If the knowledge statement list is not set, try to get the instance.
            // This also instantiates the object if it hasn't been instantiated already.
            if (knowledgeStatementList == null)
                knowledgeStatementList = KnowledgeStatementList.Instance;

            // Tries to find the start info. The object must be active for it to be gotten.
            KnowledgeStageStartInfo startInfo = FindObjectOfType<KnowledgeStageStartInfo>(false);

            // Found start info, so set the default values.
            if (startInfo != null)
            {
                // Applies the start info.
                startInfo.ApplyStartInfo(this);

                // Destroys the start info object.
                Destroy(startInfo.gameObject);
            }

            // Initializes the knowledge stage.
            InitializeStage();
        }

        // Gets the instance.
        public static KnowledgeManager Instance
        {
            get
            {
                // Checks if the instance exists.
                if (instance == null)
                {
                    // Tries to find the instance.
                    instance = FindAnyObjectByType<KnowledgeManager>(FindObjectsInactive.Include);


                    // The instance doesn't already exist.
                    if (instance == null)
                    {
                        // Generate the instance.
                        GameObject go = new GameObject("Knowledge Manager (singleton)");
                        instance = go.AddComponent<KnowledgeManager>();
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

        // Initializes the knowledge stage.
        public override void InitializeStage()
        {
            // The number of statements to use.
            int statementCount = 0;

            // The base number of statements used.
            int baseStatementCount = 0;

            // Makes sure both sections are active.
            SetSection(0);

            // Determines the statement count by the difficulty.
            switch(difficulty)
            {
                default:
                case 0:
                    baseStatementCount = knowledgeUI.statements.Count;
                    break;

                case 1:
                case 2:
                case 3:
                    baseStatementCount = 3;
                    break;

                case 4:
                case 5:
                case 6:
                    baseStatementCount = 4;
                    break;

                case 7:
                case 8:
                case 9:
                    baseStatementCount = knowledgeUI.statements.Count;
                    break;
            }

            // Set the base statement count to the statement count.
            statementCount = baseStatementCount;

            // NOTE: the statements used to set here. They have been moved after...
            // The resource setting so that there won't be more statements than there are resources.

            // If there are no natural resources in a list, get a list of all of them.
            if(naturalResources.Count <= 0)
            {
                SetNaturalResourceListToTypeList();
            }

            // The list of resources that will be used.
            List<NaturalResources.naturalResource> resourceList = naturalResources;

            // Checks if the resources should be put in a random order form the list.
            if (randomResourcesOrder)
            {
                // The resources queue.
                Queue<NaturalResources.naturalResource> resQueue = new Queue<NaturalResources.naturalResource>();

                // While there are elements left, add them to the queue at random.
                while(resourceList.Count > 0)
                {
                    // Get a random index.
                    int randIndex = Random.Range(0, resourceList.Count);

                    // Add the element to the queue and remove it from the list.
                    resQueue.Enqueue(resourceList[randIndex]);
                    resourceList.RemoveAt(randIndex);
                }

                // Put the elements from the queue in the list.
                while(resQueue.Count > 0)
                {
                    // Remove from the queue and add it to the list.
                    resourceList.Add(resQueue.Dequeue());
                }
            }

            // Counts the amount of resources that will be used.
            int resourceCount = 0;

            // Applies the natural resources to the buttons.
            // If there are more resources than there are buttons, the remaining resources in the list are unused.
            for (int i = 0; i < knowledgeUI.resources.Count; i++)
            {
                // If the button won't be used, turn it off.
                if (i < resourceList.Count)
                {
                    // Sets the resource of the button.
                    knowledgeUI.resources[i].gameObject.SetActive(true);
                    knowledgeUI.resources[i].SetResource(naturalResources[i]);
                    resourceCount++;
                }
                else
                {
                    // The button won't be used, so turn it off.
                    knowledgeUI.resources[i].gameObject.SetActive(false);
                }
            }

            // If the statement count is greater than the reosurce count...
            // Make the statement count match the resource count.
            if(statementCount > resourceCount)
            {
                statementCount = resourceCount;
            }

            
            // So that you can make sure there are never any more statements than there are resources...
            // Even if such a case should never be encountered.

            // Enables the statements that will be used.
            for(int i = 0; i < knowledgeUI.statements.Count; i++)
            {
                // If the statement will be used, enable it.
                // If it won't be used, disable it.
                if(i < statementCount)
                {
                    knowledgeUI.statements[i].gameObject.SetActive(true);
                    knowledgeUI.statements[i].matchText.text = (i + 1).ToString();
                }
                else
                {
                    knowledgeUI.statements[i].gameObject.SetActive(false);
                    knowledgeUI.statements[i].matchText.text = "0";
                }
            }

            // Statement Groups
            // Clears the group list and gets the knowledge statement list.
            statementGroups.Clear();
            KnowledgeStatementList ksl = KnowledgeStatementList.Instance;

            // Get the needed statement groups.
            foreach(KnowledgeResource resource in knowledgeUI.resources)
            {
                // Adds the group to the list if its button is active and enabled.
                // If the button is active and enabled, it means the button will be used.
                if(resource.isActiveAndEnabled)
                {
                    statementGroups.Add(ksl.GetGroupCopy(resource.resource));
                }
            }

            // Randomizes the order of statements in the groups and removes matching statements.
            for(int i = statementGroups.Count - 1; i >= 0; i--)
            {
                // Grabs the statement group.
                KnowledgeStatementList.StatementGroup group = statementGroups[i];

                // If saved matched statements shouldn't be used and the data logger has been instantiated...
                // Use the data logger to take out statements that have already been matched in other stages.
                if(!useSavedMatchedStatements && DataLogger.Instantiated)
                {
                    // Creates a list that'll hold the unmatched statements.
                    List<KnowledgeStatementList.Statement> unmatchedList = new List<KnowledgeStatementList.Statement>(group.statements);

                    // Goes through all the matched statements, removing a statement if it was found.
                    foreach (KnowledgeStatementList.Statement.StatementData statementData in dataLogger.matchedStatementDatas)
                    {
                        // If the statements match, remove the statement.
                        for (int j = unmatchedList.Count - 1; j >= 0; j--)
                        {
                            // If the group resource matches, it means that this statement data belongs to this group.
                            if(group.resource == statementData.groupResource)
                            {
                                // If the unmatched list contains a statement that matches the provided data...
                                // Remove that statment from the unmatched list.
                                if (unmatchedList[j].MatchesData(statementData))
                                {
                                    unmatchedList.RemoveAt(j);
                                    break;
                                }
                            } 
                        }
                    }

                    // If the unmatched list isn't empty, override the group statements list.
                    // If all the statements in a group have been matched, keep the list the same...
                    // So that there are still statements to pull from.
                    if(unmatchedList.Count > 0)
                    {
                        // Give the group the unmatched list.
                        group.statements = unmatchedList;
                    }
                }
                

                // If there are statements left...
                if (group.statements.Count > 0)
                {
                    // Randomize the order of statements in the group.
                    group.statements = ListHelper.RandomizeListOrder(group.statements);
                }
                else
                {
                    // The 'RandomizeStatements' function will ignore groups that have no statements.
                    // statementGroups.RemoveAt(i);
                }
            }
            
            // Randomizes the statements.
            RandomizeStatements(false);

            // Sets verification attempts to 0.
            verifyAttempts = 0;

            // Activates the statement section as the default section.
            ActivateStatementsSection();

            // Call the base function to mark that the stage has been initialized successfully.
            base.InitializeStage();

            // The knowledge has been initialized.
            initializedKnowledge = true;
        }

        // Returns 'true' if the knowledge manager has been initialized.
        public bool KnowledgeInitialized
        {
            get { return initializedKnowledge; }
        }

        // TUTORIALS
        // Checks for tutorials.
        public override void CheckTutorials()
        {
            // Check for knowledge specific tutorials.
            if (IsUsingTutorials() && !IsTutorialActive())
            {
                // Gets set to true when a tutorial has started.
                bool startedTutorial = false;

                // First Knowledge Stage
                if (!startedTutorial && !tutorials.Data.clearedFirstKnowledgeTutorial)
                {
                    tutorials.LoadFirstKnowledgeTutorial();
                    startedTutorial = true;
                }
            }

            // Calls base to check for resource tutorials.
            base.CheckTutorials();
        }


        // SECTIONS 
        // Gets the section text string translated.
        public string GetSectionStringTranslated()
        {
            string result;

            // If the LOL SDK is available, translate the text.
            if(LOLManager.IsInstantiatedAndIsLOLSDKInitialized())
            {
                result = LOLManager.GetLanguageTextStatic(sectionStringKey);
            }
            // Use default text.
            else
            {
                result = sectionString;
            }

            return result;
        }

        // Gets the statements string translated.
        public string GetStatementsStringTranslated()
        {
            string result;

            // If the LOL SDK is available, translate the text.
            if (LOLManager.IsInstantiatedAndIsLOLSDKInitialized())
            {
                result = LOLManager.GetLanguageTextStatic(statementsStringKey);
            }
            // Use default text.
            else
            {
                result = statementsString;
            }

            return result;
        }

        // Gets the resources string translated.
        public string GetResourcesStringTranslated()
        {
            string result;

            // If the LOL SDK is available, translate the text.
            if (LOLManager.IsInstantiatedAndIsLOLSDKInitialized())
            {
                result = LOLManager.GetLanguageTextStatic(resourcesStringKey);
            }
            // Use default text.
            else
            {
                result = resourcesStringString;
            }

            return result;
        }

        // Sets what section is active.
        // An inactive section has a scale of (0, 0, 0), but still has its parent active.
        // 0 = both, 1 = statmenet, 2 = resource
        public void SetSection(int section)
        {
            // Sets the instance.
            if (knowledgeUI == null)
                knowledgeUI = KnowledgeUI.Instance;

            // The section text.
            TMP_Text sectionText = knowledgeUI.sectionText;

            // Gets the parents.
            GameObject sp = knowledgeUI.statementsParent;
            GameObject rp = knowledgeUI.resourcesParent;

            // Makes sure both are active.
            sp.SetActive(true);
            rp.SetActive(true);

            // Checks what section to active.
            switch (section)
            {
                default:
                case 0: // Both
                    sp.transform.localScale = Vector3.one;
                    rp.transform.localScale = Vector3.one;

                    sectionText.text = GetSectionStringTranslated();

                    break;

                case 1: // Statements
                    sp.transform.localScale = Vector3.one;
                    rp.transform.localScale = Vector3.zero;

                    sectionText.text = GetStatementsStringTranslated();

                    break;

                case 2: // Resources
                    sp.transform.localScale = Vector3.zero;
                    rp.transform.localScale = Vector3.one;

                    sectionText.text = GetResourcesStringTranslated();

                    break;
            }


        }

        // Returns true if the statements section is active.
        public bool IsStatementsSectionActive()
        {
            GameObject sp = KnowledgeUI.Instance.statementsParent;

            return sp.activeSelf && sp.transform.localScale == Vector3.one;
        }

        // Activates the statements section.
        public void ActivateStatementsSection()
        {
            SetSection(1);
        }

        // Returns true if the resources section is active.
        public bool IsResourcesSectionActive()
        {
            GameObject rp = KnowledgeUI.Instance.resourcesParent;

            return rp.activeSelf && rp.transform.localScale == Vector3.one;
        }

        // Activates the resources section.
        public void ActivateResourcesSection()
        {
            SetSection(2);
        }

        // Swaps the sections.
        public void SwapSections()
        {
            // If the statements section is active, activate the resources section.
            if(IsStatementsSectionActive())
            {
                ActivateResourcesSection();
            }
            // Activate the statements section.
            else
            {
                ActivateStatementsSection();
            }
        }


        // RANDOMIZE
        // Randomizes the statements using the groups.
        // If 'includeInactive' is true, inactive statements and resources are included.
        public void RandomizeStatements(bool includeInactive = false)
        {
            // If there are no statement groups, there's no statements to pull from.
            // As such, don't do anything.
            if (statementGroups.Count <= 0)
                return;

            // The used resources.
            List<KnowledgeResource> usedResources = new List<KnowledgeResource>();

            // Goes through each resource and finds ones have been used.
            foreach(KnowledgeResource resource in knowledgeUI.resources)
            {
                // If the resource is active and enabled, or if inactive elements should be included.
                if(resource.isActiveAndEnabled || includeInactive)
                {
                    // If the resource has been matched correctly, include it.
                    if (resource.IsAttachmentMatchedCorrectly())
                    {
                        usedResources.Add(resource);
                    }
                }
            }


            // The list of usable groups. The order is randomized.
            List<KnowledgeStatementList.StatementGroup> usableGroups = ListHelper.RandomizeListOrder(statementGroups);

            // The index of the usable groups list.
            int usableGroupsIndex = 0;
            
            // Removes groups that have no elements.
            for (int i = usableGroups.Count - 1;  i >= 0; i--)
            {
                // If there are no statements, remove this as a usable group.
                if (usableGroups[i].statements.Count <= 0)
                {
                    usableGroups.RemoveAt(i);
                }
                else
                {
                    // Checks all the usable resources.
                    foreach(KnowledgeResource usedResource in usedResources)
                    {
                        // If this resource has been used, remove it, as there shouldn't be any buttons for it.
                        if(usedResource.resource == usableGroups[i].resource)
                        {
                            usableGroups.RemoveAt(i);
                            break;
                        }
                    }
                }
            }

            // If there are usable groups, choose random statements.
            if(usableGroups.Count > 0)
            {
                // Goes through each knowledge statement and gives them a random statement.
                foreach (KnowledgeStatement statement in knowledgeUI.statements)
                {
                    // If the statement is active and enabled, it's usable.
                    // If 'includeInactive' is true, then use the statement regardless.
                    if(statement.isActiveAndEnabled || includeInactive)
                    {
                        // If the attachment doesn't match correctly or there is no attachment, randomize it.
                        if (!statement.IsAttachmentMatchedCorrectly())
                        {
                            // Gets the group's index in the original group list.
                            int groupOrigIndex = statementGroups.IndexOf(usableGroups[usableGroupsIndex]);

                            // Gets the statement at index 0 from the statement groups and gives it to the knowledge statement.
                            KnowledgeStatementList.Statement listStatement = statementGroups[groupOrigIndex].statements[0];
                            statement.Statement = listStatement;

                            // Removes the statement at index 0 and puts it back in at the end of the list.
                            statementGroups[groupOrigIndex].statements.RemoveAt(0);
                            statementGroups[groupOrigIndex].statements.Add(listStatement);

                            // Increases the value in the usable groups index.
                            usableGroupsIndex++;

                            // If the index has reached the end of the usableGroups list, loop back to the start.
                            // NOTE: there shouldn't be cases of the multiples of the same resource showing up at the same time...
                            // In the knowledge stage. However, the code does account for it.
                            if (usableGroupsIndex >= usableGroups.Count)
                            {
                                usableGroupsIndex = 0;
                            }

                            // NOTE: while this case shouldn't occur, the program could theoretically use the same statement...
                            // Multiple times in one match, which would soft lock the game. However, there should be enough...
                            // Statements and no group duplications in the list, meaning such a circumstance shouldn't happen.
                        }
                    }
                    
                }
            }
            // There are no usable groups, pick of already used statements.
            else
            {
                // Gets set to 'true' if statements were filled when checking with the data logger.
                bool filledStatements = false;

                // If the data logger has been instantiated, try reusing a statement.
                if(DataLogger.Instantiated)
                {
                    // There are statemetns that can be reused.
                    if(dataLogger.matchedStatementDatas.Count > 0)
                    {
                        // Goes through each knowledge statement and gives it a random statement that has been used.
                        foreach (KnowledgeStatement statement in knowledgeUI.statements)
                        {
                            // If the statement is active and enabled or inactive should be included.
                            if (statement.isActiveAndEnabled || includeInactive)
                            {
                                // Gets the data and gets the group that it would come from.
                                KnowledgeStatementList.Statement.StatementData tempData = dataLogger.matchedStatementDatas[Random.Range(0, dataLogger.matchedStatementDatas.Count)];
                                KnowledgeStatementList.StatementGroup tempGroup = KnowledgeStatementList.Instance.GetGroupCopy(tempData.groupResource);

                                // If the temp group exists, reuse a statement from it.
                                if (tempGroup != null)
                                {
                                    // The new statement.
                                    KnowledgeStatementList.Statement newStatement = null;

                                    // Goes through the statements to find the one with the matching ID.
                                    foreach (KnowledgeStatementList.Statement ksls in tempGroup.statements)
                                    {
                                        // If the ID number and resource match, grab the statement.
                                        if (ksls.idNumber == tempData.idNumber && ksls.resource == tempData.statementResource)
                                        {
                                            newStatement = ksls;
                                            break;
                                        }
                                    }

                                    // If a new statement wasn't found, just load the test statement.
                                    if(newStatement == null)
                                    {
                                        newStatement = KnowledgeStatementList.GenerateTestStatement();
                                    }

                                    // Set the new statement to the knowledge statement.
                                    statement.Statement = newStatement;
                                }
                                else
                                {
                                    // Use test statement.
                                    statement.Statement = KnowledgeStatementList.GenerateTestStatement();
                                }

                            }

                        }

                        // Marks that statements were reused.
                        filledStatements = true;
                    }
                }

                // If the statements haven't been filled yet, load test statements.
                if(!filledStatements)
                {
                    // Give the test statement to all statements.
                    foreach (KnowledgeStatement statement in knowledgeUI.statements)
                    {
                        // NOTE: the test statement is in the unknown group.
                        // Since there's no button for the unknown group in the final game, this softlocks the stage.
                        // This case should never be encountered since the player shouldn't end up using up...
                        // All the statements in a single group.

                        // If the statement is active and enabled, it's usable.
                        // If 'includeInactive' is true, then use the statement regardless.
                        if (statement.isActiveAndEnabled || includeInactive)
                        {
                            statement.Statement = KnowledgeStatementList.GenerateTestStatement();
                        }
                    }
                }
                    
            }

            // Clears the current selection.
            ClearCurrentSelection();
        }

        // Verifies the matches to see if they're correct.
        public bool VerifyMatches()
        {
            // If not set, try to get the instance.
            if(knowledgeUI == null)
            {
                knowledgeUI = KnowledgeUI.Instance;
            }


            // Increases the verification attempts count.
            // This was moved here so that any energy calculation checks take this into account.
            verifyAttempts++;

            // Saves if all the statements are matching correctly.
            bool allMatch = true;

            // Go through all the statements.
            if (knowledgeUI.statements.Count > 0)
            {
                // Goes through all statements to see if they all match.
                foreach(KnowledgeStatement statement in knowledgeUI.statements)
                {
                    // If the statement object is active and enabled.
                    if(statement.isActiveAndEnabled)
                    {
                        // If the statement is matched correctly, disable the buttons since they shouldn't be used anymore.
                        if(statement.IsAttachmentMatchedCorrectly())
                        {
                            // If the statement matches, disable the buttons since they don't need to be used.
                            statement.button.interactable = false;
                            statement.attachedResource.button.interactable = false;
                        }
                        else
                        {
                            allMatch = false;
                            statement.DetachResource();
                        }
                    }
                }
            }
            else
            {
                // If there are no statements, consider all of the matched.
                allMatch = true;
            }

            // Different behaviour based on if everything matches or not.
            // If all the statements match, the stage can be finished. If not, the stage cannot finish.
            if(allMatch)
            {
                // NOTE: it does this with all knowledge statement and resource objects, even those that aren't active.

                // Disable all knowledge statement buttons.
                // Also saves all statements that were matched correctly in the data logger.
                foreach (KnowledgeStatement statement in knowledgeUI.statements)
                {
                    // If the statement isn't active and enabled, move onto the next stagement.
                    if (!statement.isActiveAndEnabled)
                        continue;

                    statement.button.interactable = false;

                    // Adds the statement to the matched statements list in the data logger so that the player...
                    // Won't get this statement again.
                    if(DataLogger.Instantiated)
                    {
                        // Generates the statement data.
                        KnowledgeStatementList.Statement.StatementData data = statement.Statement.GenerateStatementData();

                        // Gives it the resource from the attached resource, as it's also the group this data belongs to.
                        data.groupResource = statement.attachedResource.resource;

                        // Adds the data to the data logger.
                        dataLogger.matchedStatementDatas.Add(data);
                    }
                }

                // Disable all knowledge resource buttons.
                foreach(KnowledgeResource resource in knowledgeUI.resources)
                {
                    resource.button.interactable = false;
                }

                // The finish button is now in a dialog, so it's interactability doesn't need to be changed.
                // The finish button is interactable.
                // knowledgeUI.finishButton.interactable = true;

                // Calls function to indicate that the stage is over.
                OnStageOver();
            }
            // Not all matching.
            else
            {
                // The finish button is now in a dialog, so it's interactability doesn't need to be changed.
                // knowledgeUI.finishButton.interactable = false;

                // If the statements should be randomized if a match fails, randomize them.
                // If not, keep them the same.
                if (randomizeStatementsOnFail)
                {
                    RandomizeStatements(false);
                }
            }

            // CLear the current selection.
            ClearCurrentSelection();
            
            // Returns result of all statements matching.
            return allMatch;
        }

        // Clears the selections for the knowledge manager.
        public void ClearCurrentSelection()
        {
            // The current selected game object from the event system.
            GameObject currentSelectedGameObject = EventSystem.current.currentSelectedGameObject;

            // Checks if there's a selected statement or resource.
            // If so, remove the selection so that the button becomes deselected.
            if ((currentSelectedGameObject == selectedStatement && selectedStatement != null) ||
                (currentSelectedGameObject == selectedResource && selectedResource != null))
            {
                // Deselects the button.
                EventSystem.current.SetSelectedGameObject(null);
            }

            // Since the selections are being cleared, their buttons should be their normal colors...
            // Resets the buttons to their normal colors, as they might be their selected colors.
            if (selectedStatement != null)
                selectedStatement.SetButtonToNormalColor();

            if (selectedResource != null)
                selectedResource.SetButtonToNormalColor();

            // Clear the selections.
            selectedStatement = null;
            selectedResource = null;
        }

        // Clears all unverified selections.
        public void ClearUnverifiedMatches()
        {
            // Goes through each statement and clears all statements that aren't verified.
            foreach(KnowledgeStatement statement in knowledgeUI.statements)
            {
                // Checks if the statement object is active.
                if(statement.gameObject.activeSelf)
                {
                    // If the button is interactable, that means it hasn't been verified as correct yet.
                    // If a statement is verified as correct, its button is rendered uninteractable.
                    if(statement.button.interactable)
                    {
                        statement.DetachResource();
                    }
                }
            }

            // Clears the current selection.
            ClearCurrentSelection();
        }

        // Resets all matches, undoing all of them.
        public void ResetAllMatches()
        {
            // NOTE: you shouldn't need to call detach on both statements and resources...
            // But you do it anyway just to be sure.

            // Buttons that're currently off stay off. Once a stage is initialized...
            // The active parameters shouldn't be changed.

            // Disable all knowledge statement buttons.
            foreach (KnowledgeStatement statement in knowledgeUI.statements)
            {
                statement.DetachResource();
                statement.button.interactable = true;
                statement.SetButtonToNormalColor();
            }

            // Disable all knowledge resource buttons.
            foreach (KnowledgeResource resource in knowledgeUI.resources)
            {
                resource.DetachStatement();
                resource.button.interactable = true;
                resource.SetButtonToNormalColor();
            }

            // Clears the current selection.
            ClearCurrentSelection();

            // Verify button is interactable, but the finish button isn't.
            knowledgeUI.verifyButton.interactable = true;

            // The finish button is now in a dialog, so it's interactability doesn't need to be changed.
            // The button is now kept interactable.
            // knowledgeUI.finishButton.interactable = false;
        }

        // Called when the stage is over.
        public override void OnStageOver()
        {
            base.OnStageOver();

            // Open the end UI.
            knowledgeUI.OpenStageEndDialog();
        }

        // Returns the stage score.
        public override float GetStageScore()
        {
            // TODO: you'll likely change the way this is calculated later.
            // Calculates the stage score and sets it.
            CalculateAndSetGameScore();

            // Returns the game score.
            return gameScore;
        }

        // Calculates the stage score.
        public override float CalculateStageScore()
        {
            // The local base score.
            float scoreBase = 50.0F;

            // Goes through all the statements.
            foreach(KnowledgeStatement statement in KnowledgeUI.Instance.statements)
            {
                // If the statement is active and enabled.
                if(statement.isActiveAndEnabled)
                {
                    // If the statement is matched correctly, add it to the local score.
                    if(statement.IsAttachmentMatchedCorrectly())
                    {
                        scoreBase += 50.0F;
                    }
                }
            }

            // A bonus applied to the local score.
            float scoreBonus;

            // The energy bonus.
            float energyBonus = CalculateEnergyBonus();

            // Checks the number of verification attempts.
            switch(verifyAttempts)
            {
                case 5: // 5 attempts.
                    scoreBonus = 50.0F;
                    // energyBonus = 50.0F;
                    break;

                case 4: // 4 attempts
                    scoreBonus = 100.0F;
                    // energyBonus = 75.0F;
                    break;

                case 3: // 3 attempts
                    scoreBonus = 150.0F;
                    // energyBonus = 100.0F;
                    break;

                case 2: // 2 attempts
                    scoreBonus = 200.0F;
                    // energyBonus = 125.0F;
                    break;

                case 1: // 1 attempt (lowest).
                case 0: // 0 attempts (not possible).
                    scoreBonus = 250.0F;
                    // energyBonus = 150.0F;
                    break;

                default: // No bonus since too many attempts.
                    scoreBonus = 0.0F;
                    // energyBonus = 0.0F;
                    break;
            }

            // Calculates the local total score.
            float scoreTotal = scoreBase + scoreBonus;

            // If the data logger exists, set the energy bonus.
            if(DataLogger.Instantiated)
            {
                DataLogger.Instance.energyStartBonus = energyBonus;
            }    
            
            // Returns the local score total.
            return scoreTotal;
        }

        // Gets the stage energy total.
        public override float GetStageEnergyTotal()
        {
            // Returns the energy bonus.
            return CalculateEnergyBonus();
        }

        // Calculates the current energy bonus.
        // verifications: the number of verifications that have been performed.
        public float CalculateEnergyBonus(int verifications)
        {
            // The energy bonus.
            float energyBonus;

            // Checks the number of verification attempts.
            switch (verifications)
            {
                case 5: // 5 attempts.
                    energyBonus = 50.0F;
                    break;

                case 4: // 4 attempts
                    energyBonus = 75.0F;
                    break;

                case 3: // 3 attempts
                    energyBonus = 100.0F;
                    break;

                case 2: // 2 attempts
                    energyBonus = 125.0F;
                    break;

                case 1: // 1 attempt (lowest).
                case 0: // 0 attempts (not possible).
                    energyBonus = 150.0F;
                    break;

                default: // No bonus.
                    energyBonus = 0.0F;
                    break;
            }

            // Returns the energy bonus.
            return energyBonus;
        }

        // Calculates the energy bonus using the saved verify attempts.
        public float CalculateEnergyBonus()
        {
            return CalculateEnergyBonus(verifyAttempts);
        }

        // Returns 'true' if the stage is complete.
        public override bool IsComplete()
        {
            // Gets set to false if not all matching.
            bool allMatch = true;

            // Goes through all statements to see if they match.
            foreach(KnowledgeStatement statement in knowledgeUI.statements)
            {
                // If the statement is active and enabled, check it.
                if(statement.isActiveAndEnabled)
                {
                    // If the statement isn't matched correctly, the stage isn't complete.
                    if(!statement.IsAttachmentMatchedCorrectly())
                    {
                        allMatch = false;
                    }
                }
            }

            // Return result.
            return allMatch;
        }

        // Resets the stage.
        // This function will never be used since matched statements shouldn't be undone.
        public override void ResetStage()
        {
            base.ResetStage();

            // Resets all matches.
            ResetAllMatches();

            // Sets verification attempts to 0.
            verifyAttempts = 0;
        }

        // Finishes the stage. Only call this if the stage is complete.
        public override void FinishStage()
        {
            base.FinishStage();

            // Gets the instance of the data logger if it isn't set.
            if (dataLogger == null)
                dataLogger = DataLogger.Instance;

            // Saves all the statements to the data logger.
            foreach (KnowledgeStatement statement in knowledgeUI.statements)
            {
                // If the statement is active and enabled.
                if (statement.isActiveAndEnabled)
                {
                    // If the statement isn't null, save it as a used statement since the stage is finished.
                    if (statement.Statement != null)
                    {
                        // Gets the statement data.
                        KnowledgeStatementList.Statement.StatementData statementData = statement.Statement.GenerateStatementData();

                        // If the statement data isn't in the matched statements list...
                        // Add it to the matched statements list.
                        // Compares values instead of doing reference compares.
                        if (!dataLogger.MatchedStatementDatasContainsData(statementData, true))
                        {
                            // Generate the statement data.
                            dataLogger.matchedStatementDatas.Add(statementData);
                        }
                    }
                }
            }

            // NOTE: this is only for testing, and isn't necessary since the code above...
            // Already ignores duplicates. Only uncomment if you need to test.
            // Optimize the data logger's matched statements list.
            // dataLogger.OptimizeMatchedStatementDatas();

            // Makes sure the energy start bonus isn't negative.
            if (dataLogger.energyStartBonus < 0)
                dataLogger.energyStartBonus = 0;

            // Calculates the energy bonus and addss it to the data logger.
            dataLogger.energyStartBonus += CalculateEnergyBonus();

            // Generates a world start info object. The function gives it the data.
            WorldStartInfo startInfo = GenerateWorldStartInfo(true);

            // Loads the world scene.
            LoadWorldScene();
        }

        // Update is called once per frame
        protected override void Update()
        {
            base.Update();

            // // Grabs the current event system.
            // EventSystem eventSystem = EventSystem.current;
            // 
            // // If the event system has been set, check
            // if(eventSystem != null)
            // {
            // }

            // A statement and resource have both been selected.
            if (selectedStatement != null && selectedResource != null)
            {
                // Attaches the statement to the provided resource.
                selectedResource.AttachToStatement(selectedStatement);

                // Clears the selections.
                ClearCurrentSelection();
            }
            else
            {
                // Clearing the selection broke when you added more buttons. Try to fix that.

                // // EventSystem.currentSelectedGameObject shows the last object that was selected.
                // // This object is saved until something else is selected.
                // // If the player selects empty space, this is set to null.
                // if (EventSystem.current.currentSelectedGameObject == null)
                // {
                //     // If one of the statements is selected, clear the selections, since a knowledge button...
                //     // Has been deselected without connecting to anything.
                //     // If one of these things is not null, but not both, clear the selections.
                //     if(selectedStatement != null ^ selectedResource != null)
                //     {
                //         ClearCurrentSelection();
                //     }
                // }
            }
            
        }

        // This function is called when the MonoBehaviour will be destroyed.
        protected override void OnDestroy()
        {
            // If the saved instance is being deleted, set 'instanced' to false.
            if (instance == this)
            {
                instanced = false;
            }

            base.OnDestroy();
        }
    }
}