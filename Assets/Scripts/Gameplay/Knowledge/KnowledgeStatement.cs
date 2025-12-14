using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RM_EDU
{
    // A statement for the knowledge stage. It gets matched to a knowledge resource.
    public class KnowledgeStatement : MonoBehaviour
    {
        // The statement that this KnowledgeStatment is using from the list.
        public KnowledgeStatementList.Statement statement;

        // The resource attached to this statement.
        public KnowledgeResource attachedResource;

        [Header("UI")]

        // The button.
        public Button button;

        // The text for the displayed statement.
        public TMP_Text statementText;

        // The match text, which is changed to show what the knowledge statement is attached to.
        public TMP_Text matchText;

        // Start is called before the first frame update
        void Start()
        {
            // Adds a Select() call on the button.
            if (button != null)
            {
                // Listener for the tutorial toggle.
                button.onClick.AddListener(delegate
                {
                    Select();
                });
            }

            // NOTE: should probably be commented out when not running tests.
            // Generate the test statement and set it to this object.
            if(statement == null)
            {
                SetStatement(KnowledgeStatementList.GenerateTestStatement());
            }
        }

        // Called when the statement has been selected.
        public void Select()
        {
            // Set as the selected statement.
            KnowledgeManager.Instance.selectedStatement = this;
        }

        // Sets the statement.
        public void SetStatement(KnowledgeStatementList.Statement statement)
        {
            this.statement = statement;
            statementText.text = statement.text;
        }

        // Checks if the statement is attached to a resource.
        public bool IsAttachedToResource()
        {
            return attachedResource != null;
        }

        // Attaches to the following resource.
        public void AttachToStatement(KnowledgeResource newResource)
        {
            // If a resource is attached, detach it.
            if (attachedResource != null)
            {
                DetachResource();
            }

            // Set the new attachments.
            attachedResource = newResource;
            attachedResource.attachedStatement= this;

            // Calls when the attachement has been changed.
            // If a statement was detached earlier this is technically called twice, but this should be fine.
            OnAttachmentChange();
        }

        // Detaches the resource.
        public void DetachResource()
        {
            // If there's already an attachment, remove it.
            if (attachedResource != null)
            {
                // Clears the matchText for attachedResource and removes the attachments.
                attachedResource.matchText.text = "";
                attachedResource.attachedStatement = null;
                attachedResource = null;
            }

            // Calls when the attachement has been changed.
            OnAttachmentChange();
        }

        // Called when the attachment has changed.
        public void OnAttachmentChange()
        {
            // If there is an attachment, change the text to match it.
            if (attachedResource != null)
            {
                // Changes the attached resource's text to match the statement's text.
                attachedResource.matchText.text = matchText.text;
            }

        }

        // Returns 'true' if the attachment has the same resource.
        // If nothing is attached, returns false.
        // This does NOT call OnAttachmentMatchedCorrectly(). That happens in the verify function in the knowledge manager.
        public bool AttachmentMatchesCorrectly()
        {
            // If a resource is attached.
            if(attachedResource != null)
            {
                return statement.resource == attachedResource.resource;
            }
            else
            {
                return false;
            }
        }
    }
}