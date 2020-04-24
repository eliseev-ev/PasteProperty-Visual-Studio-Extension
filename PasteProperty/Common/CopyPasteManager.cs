using EnvDTE;
using Microsoft.Build.Tasks;
using Microsoft.VisualStudio.Shell;

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace PasteProperty.Common
{
    public static class CopyPasteManager
    {
        public static void CopySelected(ValueRepository valueRepository, int position)
        {
            DTE dte = (DTE)Package.GetGlobalService(typeof(DTE));

            if (dte.ActiveDocument != null)
            {
                var selection = (TextSelection)dte.ActiveDocument.Selection;
                string text = selection.Text;

                valueRepository.SetValue(position, text);
            }
        }
    }
}
