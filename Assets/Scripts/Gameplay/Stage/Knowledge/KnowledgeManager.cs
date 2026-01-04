using System.Collections;
using System.Collections.Generic;
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
                knowledgeUI = KnowledgeUI.Instance;

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

            // Randomizes the order of statements in the groups.
            for(int i = statementGroups.Count - 1; i >= 0; i--)
            {
                // Grabs the statement group.
                KnowledgeStatementList.StatementGroup group = statementGroups[i];

                // If the data logger should be used, use it to take out statements that have already been matched in other stages.
                if(useDataLogger)
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

                    // Give the group the unmatched list.
                    group.statements = unmatchedList;
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

            // TODO: remove all statements that have already been used.
            
            // Randomizes the statements.
            RandomizeStatements(false);

            // Call the base function to mark that the stage has been initialized successfully.
            base.InitializeStage();
        }

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

                // If the data logger is being used and the data logger has been instantiated, try reusing a statement.
                if(useDataLogger && DataLogger.Instantiated)
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
        }

        // Verifies the matches to see if they're correct.
        public bool VerifyMatches()
        {
            // If not set, try to get the instance.
            if(knowledgeUI == null)
            {
                knowledgeUI = KnowledgeUI.Instance;
            }

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
                    if(useDataLogger)
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

                knowledgeUI.finishButton.interactable = true;

                // Pause the stage timer since the stage is finished.
                PauseGameTimer();

                // Calculates and sets the game score.
                // TODO: move this function call to a more appropriate place.
                CalculateAndSetGameScore();
            }
            // Not all matching.
            else
            {
                knowledgeUI.finishButton.interactable = false;

                // If the statements should be randomized if a match fails, randomize them.
                // If not, keep them the same.
                if(randomizeStatementsOnFail)
                {
                    RandomizeStatements(false);
                }
            }
            
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

            // Verify button is interactable, but the finish button isn't.
            knowledgeUI.verifyButton.interactable = true;
            knowledgeUI.finishButton.interactable = false;
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
        public float CalculateStageScore()
        {
            // The local score.
            float localScore = 0.0F;

            // Goes through all the statements.
            foreach(KnowledgeStatement statement in KnowledgeUI.Instance.statements)
            {
                // If the statement is active and enabled.
                if(statement.isActiveAndEnabled)
                {
                    // If the statement is matched correctly, add it to the local score.
                    if(statement.IsAttachmentMatchedCorrectly())
                    {
                        localScore += 50.0F;
                    }
                }
            }

            // TODO: expand.

            return localScore;
        }

        // Calculates and sets the game score.
        public void CalculateAndSetGameScore()
        {
            gameScore = CalculateStageScore();
        }

        // Returns 'true' if the stage is complete.
        public override bool IsComplete()
        {
            // TODO: save the complete status to a variable.

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

        // Finishes the stage. Only call this if the stage is complete.
        public override void FinishStage()
        {
            base.FinishStage();

            // If the data logger should be used.
            if(useDataLogger)
            {
                // Gets the instance.
                if (dataLogger == null)
                    dataLogger = DataLogger.Instance;

                // Saves all the statements to the data logger.
                foreach (KnowledgeStatement statement in knowledgeUI.statements)
                {
                    // If the statement is active and enabled.
                    if(statement.isActiveAndEnabled)
                    {
                        // If the statement isn't null, save it as a used statement since the stage is finished.
                        if(statement.Statement != null)
                        {
                            // Generate the statement data.
                            dataLogger.matchedStatementDatas.Add(statement.Statement.GenerateStatementData());
                        }
                    }
                }
            }

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
    }
}