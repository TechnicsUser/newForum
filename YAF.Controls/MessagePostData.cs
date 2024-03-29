/* Yet Another Forum.NET
 * Copyright (C) 2003-2005 Bjørnar Henden
 * Copyright (C) 2006-2013 Jaben Cargman
* Copyright (C) 2014-2019 Ingo Herbote
 * http://www.yetanotherforum.net/
 * 
 * Licensed to the Apache Software Foundation (ASF) under one
 * or more contributor license agreements.  See the NOTICE file
 * distributed with this work for additional information
 * regarding copyright ownership.  The ASF licenses this file
 * to you under the Apache License, Version 2.0 (the
 * "License"); you may not use this file except in compliance
 * with the License.  You may obtain a copy of the License at

 * http://www.apache.org/licenses/LICENSE-2.0

 * Unless required by applicable law or agreed to in writing,
 * software distributed under the License is distributed on an
 * "AS IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY
 * KIND, either express or implied.  See the License for the
 * specific language governing permissions and limitations
 * under the License.
 */
namespace YAF.Controls
{
    #region Using

    using System;
    using System.Data;
    using System.Web;
    using System.Web.UI;

    using YAF.Classes;
    using YAF.Core;
    using YAF.Core.Extensions;
    using YAF.Types;
    using YAF.Types.Constants;
    using YAF.Types.Extensions;
    using YAF.Types.Flags;
    using YAF.Types.Interfaces;
    using YAF.Types.Models;

    #endregion

    /// <summary>
    /// Shows a Message Post
    /// </summary>
    public class MessagePostData : MessagePost
    {
        #region Constants and Fields

        /// <summary>
        ///   The _row.
        /// </summary>
        private Message _message;

        /// <summary>
        ///   The _show attachments.
        /// </summary>
        private bool _showAttachments = true;

        /// <summary>
        ///   The _show signature.
        /// </summary>
        private bool _showSignature = true;

        #endregion

        #region Properties

        /// <summary>
        ///   Gets or sets a value indicating whether IsAlt.
        /// </summary>
        public bool IsAltMessage { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether Col Span is.
        /// </summary>
        public string ColSpan { get; set; }

        /// <summary>
        ///   Sets the DataRow.
        /// </summary>
        public DataRow DataRow
        {
            set
            {
                this.CurrentMessage = value != null ? value.ToTyped<Message>() : null;
            }
        }

        /// <summary>
        /// Gets or sets the current message.
        /// </summary>
        /// <value>
        /// The current message.
        /// </value>
        public Message CurrentMessage
        {
            get
            {
                return this._message;
            }

            set
            {
                this._message = value ?? new Message();
                this.MessageFlags = new MessageFlags(this._message.Flags);
            }
        }

        /// <summary>
        /// Sets the search result.
        /// </summary>
        /// <value>
        /// The search result.
        /// </value>
        public SearchResult SearchResult
        {
            set
            {
                this.CurrentMessage = value != null ? value.ToTyped<Message>() : null;
            }
        }

        /// <summary>
        ///   Gets Edited.
        /// </summary>
        public DateTime Edited
        {
            get
            {
                return this.CurrentMessage.Edited ?? DateTime.UtcNow;
            }
        }

        /// <summary>
        ///   Gets Message.
        /// </summary>
        public override string Message
        {
            get
            {
                return TruncateMessage(this.CurrentMessage.MessageText ?? string.Empty);
            }
        }

        /// <summary>
        ///   Gets Message Id.
        /// </summary>
        public int? MessageId
        {
            get
            {
                return this.CurrentMessage.ID == 0 ? null : (int?)this.CurrentMessage.ID;
            }
        }

        /// <summary>
        ///   Gets Posted.
        /// </summary>
        public DateTime Posted
        {
            get
            {
                return this.CurrentMessage.Posted;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether Show the Edit Message if needed.
        /// </summary>
        public bool ShowEditMessage { get; set; }

        /// <summary>
        ///   Gets or sets a value indicating whether ShowAttachments.
        /// </summary>
        public bool ShowAttachments
        {
            get
            {
                return this._showAttachments;
            }

            set
            {
                this._showAttachments = value;
            }
        }

        /// <summary>
        ///   Gets or sets a value indicating whether ShowSignature.
        /// </summary>
        public bool ShowSignature
        {
            get
            {
                return this._showSignature;
            }

            set
            {
                this._showSignature = value;
            }
        }

        /// <summary>
        ///   Gets Signature.
        /// </summary>
        [CanBeNull]
        public override string Signature
        {
            get
            {
                if (this.ShowSignature && this.Get<YafBoardSettings>().AllowSignatures
                    && this.CurrentMessage.Signature.IsSet()
                    && this.CurrentMessage.Signature.ToLower() != "<p>&nbsp;</p>")
                {
                    return this.CurrentMessage.Signature;
                }

                return null;
            }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Truncates the message.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <returns>
        /// The truncate message.
        /// </returns>
        public static string TruncateMessage([NotNull] string message)
        {
            CodeContracts.VerifyNotNull(message, "message");

            int maxPostSize = Math.Max(YafContext.Current.Get<YafBoardSettings>().MaxPostSize, 0);

            // 0 == unlimited
            return maxPostSize == 0 || message.Length <= maxPostSize ? message : message.Truncate(maxPostSize);
        }

        #endregion

        #region Methods

        /// <summary>
        /// Raises the <see cref="E:System.Web.UI.Control.PreRender"/> event.
        /// </summary>
        /// <param name="e">An <see cref="T:System.EventArgs"/> object that contains the event data.</param>
        protected override void OnPreRender([NotNull] EventArgs e)
        {
            CodeContracts.VerifyNotNull(this.MessageFlags, "MessageFlags");

            this.MessageID = this.CurrentMessage.ID;

            if (!this.MessageFlags.IsDeleted)
            {
                // populate DisplayUserID
                if (!UserMembershipHelper.IsGuestUser(this.CurrentMessage.UserID))
                {
                    this.DisplayUserID = this.CurrentMessage.UserID;
                }

                this.IsAlt = this.IsAltMessage;

                this.RowColSpan = this.ColSpan;

                if (this.ShowAttachments)
                {
                    if (this.CurrentMessage.HasAttachments ?? false)
                    {
                        // add attached files control...
                        var attached = new MessageAttached { MessageID = this.CurrentMessage.ID };

                        if (this.CurrentMessage.UserID > 0
                            && YafContext.Current.Get<YafBoardSettings>().EnableDisplayName)
                        {
                            attached.UserName = UserMembershipHelper.GetDisplayNameFromID(this.CurrentMessage.UserID);
                        }
                        else
                        {
                            attached.UserName = this.CurrentMessage.UserName;
                        }

                        this.Controls.Add(attached);
                    }
                }
            }

            base.OnPreRender(e);
        }

        /// <summary>
        /// The render message.
        /// </summary>
        /// <param name="writer">The writer.</param>
        protected override void RenderMessage([NotNull] HtmlTextWriter writer)
        {
            CodeContracts.VerifyNotNull(writer, "writer");
            CodeContracts.VerifyNotNull(this.MessageFlags, "MessageFlags");
            CodeContracts.VerifyNotNull(this.CurrentMessage, "CurrentMessage");

            if (this.MessageFlags.IsDeleted)
            {
                this.IsModeratorChanged = this.CurrentMessage.IsModeratorChanged ?? false;

                var deleteText = this.Get<HttpContextBase>().Server.HtmlDecode(this.CurrentMessage.DeleteReason).IsSet()
                                     ? this.Get<IFormatMessage>().RepairHtml(this.CurrentMessage.DeleteReason, true)
                                     : this.GetText("EDIT_REASON_NA");

                // deleted message text...
                this.RenderDeletedMessage(writer, deleteText);
            }
            else if (this.MessageFlags.NotFormatted)
            {
                // just write out the message with no formatting...
                writer.Write(this.Message);
            }
            else if (this.CurrentMessage.Edited.HasValue)
            {
                this.IsModeratorChanged = this.CurrentMessage.IsModeratorChanged ?? false;

                // handle a message that's been edited...
                var editedMessageDateTime = this.Posted;

                if (this.Edited > this.Posted)
                {
                    editedMessageDateTime = this.Edited;
                }

                var formattedMessage =
                    this.Get<IFormatMessage>()
                        .FormatMessage(
                            this.HighlightMessage(this.Message, true),
                            this.MessageFlags,
                            false,
                            editedMessageDateTime);

                // tha_watcha : Since html message and bbcode can be mixed now, message should be always replace bbcode
                this.RenderModulesInBBCode(
                    writer,
                    formattedMessage,
                    this.MessageFlags,
                    this.DisplayUserID,
                    this.MessageId);

                // Render Edit Message
                if (this.ShowEditMessage
                    && this.Edited > this.Posted.AddSeconds(this.Get<YafBoardSettings>().EditTimeOut))
                {
                    this.RenderEditedMessage(writer, this.Edited, this.CurrentMessage.EditReason, this.MessageId);
                }
            }
            else
            {
                var formattedMessage =
                    this.Get<IFormatMessage>()
                        .FormatMessage(this.HighlightMessage(this.Message, true), this.MessageFlags);

                // tha_watcha : Since html message and bbcode can be mixed now, message should be always replace bbcode
                this.RenderModulesInBBCode(
                    writer,
                    formattedMessage,
                    this.MessageFlags,
                    this.DisplayUserID,
                    this.MessageID);
            }
        }

        #endregion
    }
}