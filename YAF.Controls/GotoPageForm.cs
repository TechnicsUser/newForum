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
    using System.Collections.Generic;
    using System.Linq;
    using System.Web.UI;
    using System.Web.UI.HtmlControls;
    using System.Web.UI.WebControls;

    using YAF.Core;
    using YAF.Types;
    using YAF.Types.Extensions;
    using YAF.Types.Interfaces;

    #endregion

    /// <summary>
    ///     The goto page forum event args.
    /// </summary>
    public class GotoPageForumEventArgs : EventArgs
    {
        #region Constructors and Destructors

        /// <summary>
        ///     Initializes a new instance of the <see cref="GotoPageForumEventArgs" /> class.
        /// </summary>
        /// <param name="gotoPage">
        ///     The goto page.
        /// </param>
        public GotoPageForumEventArgs(int gotoPage)
        {
            this.GotoPage = gotoPage;
        }

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets or sets GotoPage.
        /// </summary>
        public int GotoPage { get; set; }

        #endregion
    }

    /// <summary>
    ///     The goto page form.
    /// </summary>
    public class GotoPageForm : BaseControl
    {
        #region Fields

        /// <summary>
        ///     The _div inner.
        /// </summary>
        private readonly HtmlGenericControl _divInner = new HtmlGenericControl();

        /// <summary>
        ///     The _goto button.
        /// </summary>
        private readonly Button _gotoButton = new Button();

        /// <summary>
        ///     The _goto text box.
        /// </summary>
        private readonly TextBox _gotoTextBox = new TextBox();

        /// <summary>
        ///     The _header text.
        /// </summary>
        private readonly Label _headerText = new Label();

        /// <summary>
        ///     The _main panel.
        /// </summary>
        private readonly Panel _mainPanel = new Panel();

        /// <summary>
        ///     The _goto page value.
        /// </summary>
        private int _gotoPageValue;

        #endregion

        #region Public Events

        /// <summary>
        ///     The goto page click.
        /// </summary>
        public event EventHandler<GotoPageForumEventArgs> GotoPageClick;

        #endregion

        #region Public Properties

        /// <summary>
        ///     Gets GotoButtonClientID.
        /// </summary>
        [NotNull]
        public string GotoButtonClientID
        {
            get
            {
                return this._gotoButton.ClientID;
            }
        }

        /// <summary>
        ///     Gets or sets GotoPageValue.
        /// </summary>
        public int GotoPageValue
        {
            get
            {
                return this._gotoPageValue;
            }

            set
            {
                this._gotoPageValue = value;
            }
        }

        /// <summary>
        ///     Gets GotoTextBoxClientID.
        /// </summary>
        [NotNull]
        public string GotoTextBoxClientID
        {
            get
            {
                return this._gotoTextBox.ClientID;
            }
        }

        /// <summary>
        ///     Gets InnerDiv.
        /// </summary>
        public HtmlGenericControl InnerDiv
        {
            get
            {
                return this._divInner;
            }
        }

        /// <summary>
        ///     Gets MainPanel.
        /// </summary>
        public Panel MainPanel
        {
            get
            {
                return this._mainPanel;
            }
        }

        #endregion

        #region Methods

        /// <summary>
        ///     The build form.
        /// </summary>
        protected void BuildForm()
        {
            this.Controls.Add(this._mainPanel);

            this._mainPanel.CssClass = "gotoBase";
            this._mainPanel.ID = this.GetExtendedID("gotoBase");

            var divHeader = new HtmlGenericControl("div");

            divHeader.Attributes.Add("class", "gotoHeader");
            divHeader.ID = this.GetExtendedID("divHeader");

            this._mainPanel.Controls.Add(divHeader);

            this._headerText.ID = this.GetExtendedID("headerText");

            divHeader.Controls.Add(this._headerText);

            this._divInner.Attributes.Add("class", "gotoInner");
            this._divInner.ID = this.GetExtendedID("gotoInner");

            this._mainPanel.Controls.Add(this._divInner);

            this._gotoButton.ID = this.GetExtendedID("GotoButton");
            this._gotoButton.Style.Add(HtmlTextWriterStyle.Width, "70px");
            this._gotoButton.CausesValidation = false;
            this._gotoButton.UseSubmitBehavior = false;
            this._gotoButton.Click += this.GotoButtonClick;

            // text box...
            this._gotoTextBox.ID = this.GetExtendedID("GotoTextBox");
            this._gotoTextBox.Style.Add(HtmlTextWriterStyle.Width, "30px");

            this._divInner.Controls.Add(this._gotoTextBox);
            this._divInner.Controls.Add(this._gotoButton);

            var replaceItems = new Dictionary<string, string>()
                               {
                                   { "TEXTBOXID", this._gotoTextBox.ClientID },
                                   { "BUTTONID", this._gotoButton.ClientID }
                               };

            var script = replaceItems.Aggregate(@"(function(app, $){
                app.add_load(function() {
                    $('#TEXTBOXID').bind('keydown', function(e) {        
                        if (e.keyCode == 13) { 
                            $('#BUTTONID').click();
                            return false;
                        } 
                    });
                });
                })(Sys.Application, jQuery);", (current, replaceItem) => current.Replace(replaceItem.Key, replaceItem.Value));

            this.PageContext.PageElements.RegisterJsBlockStartup(@"GotoPageFormKeyUp_{0}".FormatWith(this.ClientID), script);
        }

        /// <summary>
        ///     The goto button click.
        /// </summary>
        /// <param name="sender">
        ///     The sender.
        /// </param>
        /// <param name="e">
        ///     The e.
        /// </param>
        protected void GotoButtonClick([NotNull] object sender, [NotNull] EventArgs e)
        {
            if (this.GotoPageClick != null)
            {
                // attempt to parse the page value...
                if (int.TryParse(this._gotoTextBox.Text.Trim(), out this._gotoPageValue))
                {
                    // valid, fire the event...
                    this.GotoPageClick(this, new GotoPageForumEventArgs(this.GotoPageValue));
                }
            }

            // clear the old value...
            this._gotoTextBox.Text = string.Empty;
        }

        /// <summary>
        ///     The on init.
        /// </summary>
        /// <param name="e">
        ///     The e.
        /// </param>
        protected override void OnInit([NotNull] EventArgs e)
        {
            base.OnInit(e);

            this.BuildForm();
        }

        /// <summary>
        ///     The on load.
        /// </summary>
        /// <param name="e">
        ///     The e.
        /// </param>
        protected override void OnLoad([NotNull] EventArgs e)
        {
            base.OnLoad(e);

            // localization has to be done in here so as to not attempt
            // to localize before the class has been created
            if (this.Get<ILocalization>().TransPage.IsSet())
            {
                this._headerText.Text = this.GetText("COMMON", "GOTOPAGE_HEADER");
                this._gotoButton.Text = this.GetText("COMMON", "GO");
            }
            else
            {
                // non-localized for admin pages
                this._headerText.Text = "Goto Page...";
                this._gotoButton.Text = "Go";
            }
        }

        /// <summary>
        ///     The render.
        /// </summary>
        /// <param name="writer">
        ///     The writer.
        /// </param>
        protected override void Render([NotNull] HtmlTextWriter writer)
        {
            writer.WriteLine(@"<div id=""{0}"" style=""display:none"" class=""gotoPageForm"">".FormatWith(this.ClientID));

            base.Render(writer);

            writer.WriteLine("</div>");
        }

        #endregion
    }
}