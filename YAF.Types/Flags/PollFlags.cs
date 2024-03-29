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
namespace YAF.Types.Flags
{
  using System;

  /// <summary>
  /// The poll flags.
  /// </summary>
  [Serializable]
  public class PollFlags : FlagsBase
  {
    #region Constructors

    /// <summary>
    /// Initializes a new instance of the <see cref="PollFlags"/> class.
    /// </summary>
    public PollFlags()
      : this(0)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PollFlags"/> class.
    /// </summary>
    /// <param name="flags">
    /// The flags.
    /// </param>
    public PollFlags(Flags flags)
      : this((int) flags)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PollFlags"/> class.
    /// </summary>
    /// <param name="bitValue">
    /// The bit value.
    /// </param>
    public PollFlags(object bitValue)
      : this((int) bitValue)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PollFlags"/> class.
    /// </summary>
    /// <param name="bitValue">
    /// The bit value.
    /// </param>
    public PollFlags(int bitValue)
      : base(bitValue)
    {
    }

    /// <summary>
    /// Initializes a new instance of the <see cref="PollFlags"/> class.
    /// </summary>
    /// <param name="bits">
    /// The bits.
    /// </param>
    public PollFlags(params bool[] bits)
      : base(bits)
    {
    }

    #endregion

    #region Operators

    /// <summary>
    /// The op_ implicit.
    /// </summary>
    /// <param name="newBitValue">
    /// The new bit value.
    /// </param>
    /// <returns>
    /// </returns>
    public static implicit operator PollFlags(int newBitValue)
    {
      return new PollFlags(newBitValue);
    }

    /// <summary>
    /// The op_ implicit.
    /// </summary>
    /// <param name="flags">
    /// The flags.
    /// </param>
    /// <returns>
    /// </returns>
    public static implicit operator PollFlags(Flags flags)
    {
      return new PollFlags(flags);
    }

    #endregion

    #region Flags Enumeration

    /// <summary>
    /// Use for bit comparisons
    /// </summary>
    [Flags]
    public enum Flags : int
    {
      None = 0,

      /// <summary>
      /// The is admin.
      /// </summary>
      // = 1, 

      /// <summary>
      /// The is guest.
      /// </summary>
      // = 2, 

      /// <summary>
      /// The is start.
      /// </summary>
      IsClosedBound = 4,


      /// <summary>
      /// The allow multiple choices.
      /// </summary>
      AllowMultipleChoices = 8 
   
      /* for future use
          xxxxx = 1,
          xxxxx = 2,        
		  xxxxx = 16,
			xxxxx = 32,
			xxxxx = 64,
			xxxxx = 128,
			xxxxx = 256,
			xxxxx = 512
			 */
    }

    #endregion

    #region Single Flags (can be 32 of them)


    /// <summary>
    /// Gets or sets whether the poll is closed bound.
    /// </summary>
    public bool IsClosedBound
    {
      // int value 4
      get
      {
        return this[0];
      }

      set
      {
        this[0] = value;
      }
    }
   

    #endregion
  }
}