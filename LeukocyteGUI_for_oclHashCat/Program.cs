﻿//  Copyright (C) 2015  Igor Kotlerman <igor.kotlerman@gmail.com>

//  This file is part of LeukocyteGUI_for_oclHashCat.

//  LeukocyteGUI_for_oclHashCat is free software: you can
//  redistribute it and/or modify it under the terms of the
//  GNU General Public License as published by the
//  Free Software Foundation, either version 3
//  of the License, or (at your option) any later version.

//  LeukocyteGUI_for_oclHashCat is distributed in the hope
//  that it will be useful, but WITHOUT ANY WARRANTY;
//  without even the implied warranty of MERCHANTABILITY or
//  FITNESS FOR A PARTICULAR PURPOSE.  
//  See the GNU General Public License for more details.

//  You should have received a copy of the
//  GNU General Public License along with
//  LeukocyteGUI_for_oclHashCat. 
//  If not, see <http://www.gnu.org/licenses/>.

using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace LeukocyteGUI_for_oclHashCat
{
    static class Program
    {
        /// <summary>
        /// Главная точка входа для приложения.
        /// </summary>
        [STAThread]
        static void Main()
        {
            Application.EnableVisualStyles();
            Application.SetCompatibleTextRenderingDefault(false);
            Application.Run(new MainForm());
        }
    }
}
