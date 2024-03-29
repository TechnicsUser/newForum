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
namespace YAF.Types.Models
{
    using System;

    using ServiceStack.DataAnnotations;

    using YAF.Types.Interfaces;
    using YAF.Types.Interfaces.Data;

    /// <summary>
    /// A class which represents the BBCode table.
    /// </summary>
    [Serializable]
    public partial class BBCode : IEntity, IHaveBoardID, IHaveID
    {
        partial void OnCreated();

        public BBCode()
        {
            this.OnCreated();
        }

        #region Properties

        [AutoIncrement]
        [Alias("BBCodeID")]
        public int ID { get; set; }

        public int BoardID { get; set; }

        public string Name { get; set; }

        public string Description { get; set; }

        public string OnClickJS { get; set; }

        public string DisplayJS { get; set; }

        public string EditJS { get; set; }

        public string DisplayCSS { get; set; }

        public string SearchRegex { get; set; }

        public string ReplaceRegex { get; set; }

        public string Variables { get; set; }

        public bool? UseModule { get; set; }

        public string ModuleClass { get; set; }

        public int ExecOrder { get; set; }

        #endregion
    }
}