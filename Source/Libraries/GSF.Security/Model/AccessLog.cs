//******************************************************************************************************
//  AccessLog.cs - Gbtc
//
//  Copyright � 2016, Grid Protection Alliance.  All Rights Reserved.
//
//  Licensed to the Grid Protection Alliance (GPA) under one or more contributor license agreements. See
//  the NOTICE file distributed with this work for additional information regarding copyright ownership.
//  The GPA licenses this file to you under the MIT License (MIT), the "License"; you may
//  not use this file except in compliance with the License. You may obtain a copy of the License at:
//
//      http://opensource.org/licenses/MIT
//
//  Unless agreed to in writing, the subject software distributed under the License is distributed on an
//  "AS-IS" BASIS, WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied. Refer to the
//  License for the specific language governing permissions and limitations.
//
//  Code Modification History:
//  ----------------------------------------------------------------------------------------------------
//  02/27/2016 - J. Ritchie Carroll
//       Generated original version of source code.
//
//******************************************************************************************************

using System;
using System.ComponentModel.DataAnnotations;
using GSF.ComponentModel;
using GSF.Data.Model;

namespace GSF.Security.Model
{
    /// <summary>
    /// Model for AccessLog table.
    /// </summary>
    public class AccessLog
    {
        /// <summary>
        /// Unique ID field.
        /// </summary>
        [PrimaryKey(true)]
        public int ID { get; set; }

        /// <summary>
        /// User name field.
        /// </summary>
        [Required]
        [StringLength(200)]
        public string UserName { get; set; }

        /// <summary>
        /// Access granted flag field.
        /// </summary>
        public bool AccessGranted { get; set; }


        /// <summary>
        /// Node ID of the Application.
        /// </summary>
        public Guid NodeID { get; set; }

        /// <summary>
        /// Created on field.
        /// </summary>
        [DefaultValueExpression("DateTime.UtcNow")]
        public DateTime CreatedOn { get; set; }
    }
}
