using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

namespace RM_EDU
{
    // A resource in the knowledge stage. A knowledge statement is matched to it.
    public class KnowledgeResource : MonoBehaviour
    {
        // The natural resource for this knowledge resource.
        public NaturalResources.naturalResource resource = NaturalResources.naturalResource.unknown;

        // The statement that is attached to this resource.
        public KnowledgeStatement attachedStatement;

        [Header("UI")]

        // The button.
        public Button button;

        // The description for this resource. This is just the name of the resource.
        public TMP_Text resourceText;

        // The text that gets changed to show what statmenet this resource is matched with.
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
        }

        // Called when the resource has been selected.
        public void Select()
        {
            // Set as the selected resource.
            KnowledgeManager.Instance.selectedResource = this;
        }

        // Sets the resource and the display text.
        public void SetResource(NaturalResources.naturalResource resource)
        {
            this.resource = resource;
            resourceText.text = NaturalResources.GetNaturalResourceName(resource);
        }

        // Checks if the resource is attached to a statement.
        public bool IsAttachedToStatement()
        {
            return attachedStatement != null;
        }

        // Attaches to the following statement.
        public void AttachToStatement(KnowledgeStatement newAttachment)
        {
            // If a statement is attached, detach it.
            if(attachedStatement != null)
            {
                DetachStatement();
            }

            // Set the new attachments.
            attachedStatement = newAttachment;
            attachedStatement.attachedResource = this;

            // Calls when the attachement has been changed.
            // If a statement was detached earlier this is technically called twice, but this should be fine.
            OnAttachmentChange();
        }

        // Detaches the statement.
        public void DetachStatement()
        {
            // If there's already an attachment, remove it.
            if (attachedStatement != null)
            {
                // Removes the attachments.
                attachedStatement.attachedResource = null;
                attachedStatement = null;
            }

            // Calls when the attachement has been changed.
            OnAttachmentChange();
        }

        // Called when the attachment has changed.
        public void OnAttachmentChange()
        {
            // If there is an attachment, change the text to match it.
            if (attachedStatement != null)
            {
                matchText.text = attachedStatement.matchText.text;
            }
            // If there isn't a statement, remove the text.
            else
            {
                matchText.text = "";
            }
        }


    }
}