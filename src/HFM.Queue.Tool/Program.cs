﻿/*
 * HFM.NET - Queue.Tool
 * Copyright (C) 2009-2015 Ryan Harlamert (harlam357)
 *
 * This program is free software; you can redistribute it and/or
 * modify it under the terms of the GNU General Public License
 * as published by the Free Software Foundation; version 2
 * of the License. See the included file GPLv2.TXT.
 * 
 * This program is distributed in the hope that it will be useful,
 * but WITHOUT ANY WARRANTY; without even the implied warranty of
 * MERCHANTABILITY or FITNESS FOR A PARTICULAR PURPOSE.  See the
 * GNU General Public License for more details.
 * 
 * You should have received a copy of the GNU General Public License
 * along with this program; if not, write to the Free Software
 * Foundation, Inc., 51 Franklin Street, Fifth Floor, Boston, MA  02110-1301, USA.
 */

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Reflection;

namespace HFM.Queue.Tool
{
   internal class Program
   {
      private static void Main(string[] args)
      {
         const string app = "HFM.Queue.Tool";

         FileVersionInfo fi = FileVersionInfo.GetVersionInfo(Assembly.GetExecutingAssembly().Location);
         Console.WriteLine("{0} version {1} (qd clone)", app, fi.FileVersion);
         Console.WriteLine("Copyright (C) 2002-2005 Richard P. Howell IV.");
         Console.WriteLine("Copyright (C) 2005-2008 Sebastiaan Couwenberg");
         Console.WriteLine("Copyright (C) 2009-2015 Ryan Harlamert");
         
         ICollection<Argument> arguments;
         try
         {
            arguments = Arguments.Parse(args);
         }
         catch (Exception ex)
         {
            Console.WriteLine();
            Console.WriteLine("{0}", ex.Message);
            return;
         }

         bool versionOnly = arguments.FirstOrDefault(a => a.Type == ArgumentType.VersionOnly) != null;
         if (versionOnly)
         {
            return;
         }

         bool showUsage = arguments.FirstOrDefault(a => a.Type == ArgumentType.Usage) != null;
         bool unknown = arguments.FirstOrDefault(a => a.Type == ArgumentType.Unknown) != null;
         if (showUsage || unknown)
         {
            Console.WriteLine(Arguments.Usage);
            return;
         }
         
         string filePath = "queue.dat";
         var queueFile = arguments.FirstOrDefault(a => a.Type == ArgumentType.QueueFile);
         if (queueFile != null)
         {
            filePath = queueFile.Data;
         }

         if (!(File.Exists(filePath)))
         {
            Console.WriteLine();
            Console.WriteLine("File '{0}' does not exist.", filePath);
            return;
         }

         try
         {
            var q = QueueReader.ReadQueue(filePath);
            QueueDisplay.Write(q, arguments);
         }
         catch (Exception ex)
         {
            Console.WriteLine();
            Console.WriteLine(ex.Message);
         }
      }
   }
}
