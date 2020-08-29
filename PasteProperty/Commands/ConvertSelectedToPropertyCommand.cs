using System;
using System.ComponentModel.Design;
using System.Globalization;
using System.Threading;
using System.Threading.Tasks;
using EnvDTE;
using Microsoft.VisualStudio.Debugger.Interop;
using Microsoft.VisualStudio.Shell;
using Microsoft.VisualStudio.Shell.Interop;
using PasteProperty.Common;
using PasteProperty.Common.Extentions;
using Task = System.Threading.Tasks.Task;

namespace PasteProperty
{
    /// <summary>
    /// Command handler
    /// </summary>
    internal sealed class ConvertSelectedToPropertyCommand
    {
        /// <summary>
        /// Command ID.
        /// </summary>
        public const int CommandId = 4129;

        /// <summary>
        /// Command menu group (command set GUID).
        /// </summary>
        public static readonly Guid CommandSet = new Guid("bf5a8902-3958-497d-b07a-2dae843b50b1");

        /// <summary>
        /// VS Package that provides this command, not null.
        /// </summary>
        private readonly AsyncPackage package;

        ClipBoardRepository _values;


        private readonly OleMenuCommand _myCommand;

        /// <summary>
        /// Initializes a new instance of the <see cref="ConvertSelectedToPropertyCommand"/> class.
        /// Adds our command handlers for menu (commands must exist in the command table file)
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        /// <param name="commandService">Command service to add command to, not null.</param>
        private ConvertSelectedToPropertyCommand(AsyncPackage package, OleMenuCommandService commandService, ClipBoardRepository values)
        {
            this.package = package ?? throw new ArgumentNullException(nameof(package));
            commandService = commandService ?? throw new ArgumentNullException(nameof(commandService));

            var menuCommandID = new CommandID(CommandSet, CommandId);
            _myCommand = new OleMenuCommand(this.Execute, menuCommandID);
            _myCommand.BeforeQueryStatus += (_, __) => { ChangeText(); };
            commandService.AddCommand(_myCommand);
            _values = values;
        }

        /// <summary>
        /// Gets the instance of the command.
        /// </summary>
        public static ConvertSelectedToPropertyCommand Instance
        {
            get;
            private set;
        }

        /// <summary>
        /// Gets the service provider from the owner package.
        /// </summary>
        private Microsoft.VisualStudio.Shell.IAsyncServiceProvider ServiceProvider
        {
            get
            {
                return this.package;
            }
        }

        /// <summary>
        /// Initializes the singleton instance of the command.
        /// </summary>
        /// <param name="package">Owner package, not null.</param>
        public static async Task InitializeAsync(AsyncPackage package, ClipBoardRepository values)
        {
            // Switch to the main thread - the call to AddCommand in ConvertSelectedToPropertyCommand's constructor requires
            // the UI thread.
            await ThreadHelper.JoinableTaskFactory.SwitchToMainThreadAsync(package.DisposalToken);

            OleMenuCommandService commandService = await package.GetServiceAsync(typeof(IMenuCommandService)) as OleMenuCommandService;
            Instance = new ConvertSelectedToPropertyCommand(package, commandService, values);
        }

        /// <summary>
        /// This function is the callback used to execute the command when the menu item is clicked.
        /// See the constructor to see how the menu item is associated with this function using
        /// OleMenuCommandService service and MenuCommand class.
        /// </summary>
        /// <param name="sender">Event sender.</param>
        /// <param name="e">Event args.</param>
        private void Execute(object sender, EventArgs e)
        {
            DTE dte = (DTE)Package.GetGlobalService(typeof(DTE));

            if (dte.ActiveDocument != null)
            {
                var selection = (TextSelection)dte.ActiveDocument.Selection;
                string text = selection.Text;
                _values.Insert(text);
                selection.Text = text.ToProperty();
            }
        }

        private void ChangeText()
        {
            DTE dte = (DTE)Package.GetGlobalService(typeof(DTE));

            if (dte.ActiveDocument != null)
            {
                var selection = (TextSelection)dte.ActiveDocument.Selection;
                string text = selection.Text;

                _myCommand.Text = $"Convert \"{text.ToShortString().ToProperty()}\"";
            }
        }
    }
}
